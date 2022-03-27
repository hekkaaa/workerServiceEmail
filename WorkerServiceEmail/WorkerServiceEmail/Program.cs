using NLog.Extensions.Logging;
using WorkerServiceEmail;
using WorkerServiceEmail.Email;
using WorkerServiceEmail.Email.SMTP.Client;
using WorkerServiceEmail.Infrastructure.Logging;
using WorkerServiceEmail.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        var config = new ConfigurationBuilder()
              .SetBasePath(System.IO.Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .Build();

        services.AddHostedService<Worker>();
        services.AddSingleton<IRunner, Runner>()
        .AddLogging(loggingBuilder =>
        {
            // configure Logging with NLog
            loggingBuilder.ClearProviders();
            loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
            loggingBuilder.AddNLog(config);
        });
        services.AddSingleton<IEmailService, EmailService>();
        services.AddSingleton<IClientSmtp, SmtpClientGoogleAsync>();
        services.AddSingleton<IStartingSubService, StartingSubService>();
        services.AddSingleton<IRabbitReceiveService, RabbitReceiveService>();
    })
    .Build();

await host.RunAsync();
