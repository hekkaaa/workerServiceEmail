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


        public Worker(
            IRunner runner,
            IEmailService emailService,
            IStartingSubService startingSubService)
        {
            _runner = runner;
            _emailService = emailService;
            _startingSubService = startingSubService;
        }

        //public override async Task StartAsync(CancellationToken stoppingToken)
        //{
        //    await CheckingPreparationLogToWork.CheckLogFileForSystem(_emailService, _runner);

        //    _runner.WarningAction("Service Email Get Started!");

        //    var startEmailService = _startingSubService.Start();

        //    if (!startEmailService.Result)
        //    {
        //        await StopAsync(stoppingToken);
        //    }
        //    ExecuteAsync(stoppingToken).Wait();
        //}

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
                //_runner.InfoAction($"Worker running at: {DateTimeOffset.Now}");

                // судя по всему тут будут слушаться RabbitMQ


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