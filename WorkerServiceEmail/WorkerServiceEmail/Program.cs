using NLog.Extensions.Logging;
using WorkerServiceEmail;
using WorkerServiceEmail.Email;
using WorkerServiceEmail.Email.SMTP.Client;
using WorkerServiceEmail.Infrastructure;
using WorkerServiceEmail.Infrastructure.Logging;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
