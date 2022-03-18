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

        public Worker(ILogger<Worker> logger, IRunner runner, IEmailService emailService)
           
        {
            _logger = logger;
            _runner = runner;
            _emailService = emailService;


            var config = new ConfigurationBuilder()
              .SetBasePath(System.IO.Directory.GetCurrentDirectory()) //From NuGet Package Microsoft.Extensions.Configuration.Json
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .Build();

            using var servicesProvider = new ServiceCollection()
                .AddTransient<Runner>() // Runner is the custom class
                .AddLogging(loggingBuilder =>
                {
                    // configure Logging with NLog
                    loggingBuilder.ClearProviders();
                    loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                    loggingBuilder.AddNLog(config);
                }).BuildServiceProvider();

            _runner = servicesProvider.GetRequiredService<Runner>();
        }

        public override async Task StartAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Start");

            _runner.WarningAction("Service Email Get Started!");

            MessageEmail startMessage = new MessageEmail
            {
                EmailFrom = "dogsitterclub2022@gmail.com",
                NameFrom = "Daemon Start Service",
                EmailTo = "silencemyalise@gmail.com",
                NameTo = "Administrator Service",
                Subject = "Service Email Alert!",
                MessageText = "Service Email from Windows Server Ip: 1.1.1.1 - Successfully launched!"
            };
           await _emailService.SendEmailAsync(startMessage);
            //new TestSendMail(_logger, _runner).SendEmailAsync(); // ѕисьмо о старте сервиса.

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
            Console.WriteLine("Stop");

            await base.StopAsync(stoppingToken);
        }
    }
}