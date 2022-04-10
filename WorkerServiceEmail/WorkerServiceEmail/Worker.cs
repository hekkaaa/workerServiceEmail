//using Marvelous.Contracts.Urls;
using WorkerServiceEmail.Email;
using Marvelous.Contracts.Endpoints;
using WorkerServiceEmail.Infrastructure;
using WorkerServiceEmail.Infrastructure.Logging;
using WorkerServiceEmail.Services;

namespace WorkerServiceEmail
{
    public class Worker : BackgroundService
    {
        private readonly IRunner _runner;
        private readonly ICheckingSubEmailService _startingSubService;

        public Worker(
            IRunner runner,
            ICheckingSubEmailService startingSubService)
        {
            _runner = runner;
            _startingSubService = startingSubService;
        }

        public override async Task StartAsync(CancellationToken stoppingToken)
        {
            //_runner.WarningAction("Service Email Get Started!");

            //var startEmailService = _startingSubService.Start();

            //if (!startEmailService.Result)
            //{
            //    await StopAsync(stoppingToken);
            //}


            AuthToken test = new AuthToken();
            var rest = await test.SendRequestAsync<string>(@"https://piter-education.ru:6042", $"{AuthEndpoints.ApiAuth}{AuthEndpoints.TokenForMicroservice}");
            Console.WriteLine("Token: " + rest.Data);

            var alena = await test.SendRequestAsync<IEnumerable<Marvelous.Contracts.ResponseModels.ConfigResponseModel>>(@"https://piter-education.ru:6040", ConfigsEndpoints.Configs, rest.Data);
            var qr = alena.Data.FirstOrDefault(x => x.Key == "LOGIN_EMAIL_GMAIL");
        
            Console.WriteLine(qr.Value);
            Console.WriteLine("---------------");

            foreach (var t in alena.Data)
            {
                Console.WriteLine(t.Key);
                Console.WriteLine(t.Value);
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