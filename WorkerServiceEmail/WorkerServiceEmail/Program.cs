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
        services.AddSingleton<IRunner, Runner>();
        services.AddSingleton<IEmailService, EmailService>();
        services.AddSingleton<ISmtpClientGoogleAsync, SmtpClientGoogleAsync>();
    })
    .Build();

await host.RunAsync();
