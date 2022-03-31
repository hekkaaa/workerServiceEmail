using WorkerServiceEmail.Email;
using WorkerServiceEmail.Infrastructure;
using WorkerServiceEmail.Infrastructure.Logging;
using WorkerServiceEmail.Services;

namespace WorkerServiceEmail
{
    public class Worker : BackgroundService
    {
        private readonly IRunner _runner;
        private readonly IEmailService _emailService;
        private readonly ISubService _startingSubService;

        public Worker(
            IRunner runner,
            IEmailService emailService,
            ISubService startingSubService)
        {
            _runner = runner;
            _emailService = emailService;
            _startingSubService = startingSubService;
        }
       
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await CheckingPreparationLogToWork.CheckLogFileForSystem(_emailService, _runner);

            _runner.WarningAction("Service Email Get Started!");

            var startEmailService = _startingSubService.Start();

            if (!startEmailService.Result)
            {
                await StopAsync(stoppingToken);
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                _runner.InfoAction($"Worker running at: {DateTimeOffset.Now}");

                // судя по всему тут будут слушаться RabbitMQ

                await Task.Delay(300000, stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _runner.WarningAction("Service Email is stopped!");
            await _startingSubService.Stop();
            await base.StopAsync(stoppingToken);
        }
    }
}