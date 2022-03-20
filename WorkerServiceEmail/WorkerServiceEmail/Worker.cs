using NLog.Extensions.Logging;
using WorkerServiceEmail.Email;
using WorkerServiceEmail.Email.SMTP.Client;
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
        readonly string? _userDirectory = Environment.GetEnvironmentVariable("LOG_DIRECTORY");

        public Worker(ILogger<Worker> logger, IRunner runner, IEmailService emailService)
        {
            _logger = logger;
            _runner = runner;
            _emailService = emailService;
        }

        public override async Task StartAsync(CancellationToken stoppingToken)
        {
            await CheckFileLog.CheckFileForSystem(_userDirectory, _emailService);

            _runner.WarningAction("Service Email Get Started!");

            MessageEmail startMessage = new MessageEmail
            {
                EmailFrom = "dogsitterclub2022@gmail.com",
                NameFrom = "Daemon Start Service",
                EmailTo = "silencemyalise@gmail.com",
                NameTo = "Administrator Service",
                Subject = "Service Email Alert!",
                MessageText = "<b>Service Email from Windows Server</b><br>" +
                "<b>IP:</b> 1.1.1.1 - Successfully launched!"
            };
            
            //await _emailService.SendEmailAsync(startMessage);

            ExecuteAsync(stoppingToken).Wait();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {   
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                // ���� �� ����� ��� ����� ��������� RabbitMQ
                try
                {   
                    // ������ ��������� 

                    //var res = new EmailService().SendEmailAsync("test", "silencemyalise@gmail.com", "Test Asych", "Hellow here text");
                    //Console.WriteLine($"Result: {res.Result}");
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

            await base.StopAsync(stoppingToken);
        }
    }
}