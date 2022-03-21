using WorkerServiceEmail.Email;
using WorkerServiceEmail.EntityMessage;
using WorkerServiceEmail.Infrastructure;
using WorkerServiceEmail.Infrastructure.Logging;

namespace WorkerServiceEmail
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IRunner _runner;
        private readonly IEmailService _emailService;
      

        public Worker(ILogger<Worker> logger, IRunner runner, IEmailService emailService)
        {
            _logger = logger;
            _runner = runner;
            _emailService = emailService;
        }

        public override async Task StartAsync(CancellationToken stoppingToken)
        {
            await CheckFileLog.CheckLogFileForSystem(_emailService);

            _runner.WarningAction("Service Email Get Started!");

            MessageEmail startMessage = new MessageEmail
            {
                EmailFrom = "dogsitterclub2022@gmail.com",
                NameFrom = "Daemon Start Service",
                EmailTo = "silencemyalise@gmail.com",
                NameTo = "Administrator Service",
                Subject = "Service Email Alert!",
                MessageText = "<b>Service Email from Windows Server</b><br>" +
                $"<b>IP:</b> {GetIpAddresHost.GetIpThisHost()} - Successfully launched!"
            };

            await _emailService.SendEmailAsync(startMessage);

            ExecuteAsync(stoppingToken).Wait();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                // суд€ по всему тут будут слушатьс€ RabbitMQ
                try
                {
                    // ѕришло сообщение 
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                await Task.Delay(5000, stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {

            _runner.WarningAction("Service Email Stopped!");

            MessageEmail startMessage = new MessageEmail
            {
                EmailFrom = "dogsitterclub2022@gmail.com",
                NameFrom = "Daemon Start Service",
                EmailTo = "silencemyalise@gmail.com",
                NameTo = "Administrator Service",
                Subject = "Service Email Alert!",
                MessageText = "<b>Service Email from Windows Server</b><br>" +
             $"<b>IP:</b> {GetIpAddresHost.GetIpThisHost()} - Service stopped!"
            };

            await _emailService.SendEmailAsync(startMessage);

            await base.StopAsync(stoppingToken);
        }
    }
}