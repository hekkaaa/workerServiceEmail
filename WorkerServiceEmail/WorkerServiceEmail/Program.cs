using NLog.Extensions.Logging;
using WorkerServiceEmail;
using WorkerServiceEmail.Email;
using WorkerServiceEmail.Email.SMTP.Client;
using WorkerServiceEmail.Infrastructure;
using WorkerServiceEmail.Infrastructure.Logging;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {   
        var config = new ConfigurationBuilder()
              .SetBasePath(System.IO.Directory.GetCurrentDirectory()) //From NuGet Package Microsoft.Extensions.Configuration.Json
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
        services.AddSingleton<ISmtpClientGoogleAsync, SmtpClientGoogleAsync>();
    })
    .Build();

await host.RunAsync();
