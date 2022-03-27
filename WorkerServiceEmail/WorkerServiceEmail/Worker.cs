using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using WorkerServiceEmail.Email;
using WorkerServiceEmail.EntityMessage;
using WorkerServiceEmail.Infrastructure;
using WorkerServiceEmail.Infrastructure.Logging;
using WorkerServiceEmail.Services;

namespace WorkerServiceEmail
{
    public class Worker : BackgroundService
    {
        private readonly IRunner _runner;
        private readonly IEmailService _emailService;
        private readonly IStartingSubService _startingSubService;
      

        public Worker(ILogger<Worker> logger, IRunner runner, IEmailService emailService, IStartingSubService startingSubService)
        {
            _runner = runner;
            _emailService = emailService;
            _startingSubService = startingSubService;
        }

        public override async Task StartAsync(CancellationToken stoppingToken)
        {
            await CheckingPreparationLogToWork.CheckLogFileForSystem(_emailService, _runner);

            _runner.WarningAction("Service Email Get Started!");
          
           var startEmailService = _startingSubService.Start();
          
            if (!startEmailService.Result)
            {
                await StopAsync(stoppingToken);
            }
            ExecuteAsync(stoppingToken).Wait();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _runner.InfoAction($"Worker running at: {DateTimeOffset.Now}");

                // судя по всему тут будут слушаться RabbitMQ
                try
                {
                    var factory = new ConnectionFactory() { HostName = "localhost" };
                    using (var connection = factory.CreateConnection())
                    using (var channel = connection.CreateModel())
                    {
                        channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                        channel.QueueDeclare(queue: "test-que-1",
                                             durable: true,
                                             exclusive: false,
                                             autoDelete: false,
                                             arguments: null);

                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body.ToArray();
                            var message = Encoding.UTF8.GetString(body);
                            Console.WriteLine(" [x] Received {0}", message);


                            MessageEmail infomessage = new MessageEmail
                            {
                                EmailFrom = "dogsitterclub2022@gmail.com",
                                NameFrom = "Daemon Start Service",
                                EmailTo = "silencemyalise@gmail.com",
                                NameTo = "Administrator Service",
                                Subject = "Service Email Alert!",
                                MessageText = "<b>message from rabbitMQ</b><br>" +
                                 $"<b> {message} </b>"
                            };
                            _emailService.SendEmailAsync(infomessage);
                        };
                        channel.BasicConsume(queue: "test-que-1",
                                             autoAck: true,
                                             consumer: consumer);
                        Console.WriteLine(" Press [enter] to exit.");
                        Console.ReadLine();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                await Task.Delay(2000, stoppingToken);
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
             $"<b>IP:</b> {IpAddressHelper.GetIpThisHost()} - Service stopped!"
            };

            Console.WriteLine("Остановить Сервер");
            await _emailService.SendEmailAsync(startMessage);
            await base.StopAsync(stoppingToken);
            NLog.LogManager.DisableLogging();

        }
    }
}