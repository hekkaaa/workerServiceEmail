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
        private readonly ICheckingSubEmailService _startingSubService;

        public Worker(
            IRunner runner,
            IEmailService emailService,
            ICheckingSubEmailService startingSubService)
        {
            _runner = runner;
            _emailService = emailService;
            _startingSubService = startingSubService;
        }

        public override async Task StartAsync(CancellationToken stoppingToken)
        {
            (CheckingPreparationLogToWork.CheckLogFileForSystem(_emailService, _runner)).Wait();

            _runner.WarningAction("Service Email Get Started!");

            var startEmailService = _startingSubService.Start();

            if (!startEmailService.Result)
            {
                await StopAsync(stoppingToken);
            }
             ExecuteAsync(stoppingToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _runner.InfoAction($"Worker running at: {DateTimeOffset.Now}");

                await Task.Delay(600000, stoppingToken);
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