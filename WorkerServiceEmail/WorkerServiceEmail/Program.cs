using MassTransit;
using NLog.Extensions.Logging;
using WorkerServiceEmail;
using WorkerServiceEmail.Email;
using WorkerServiceEmail.Email.SMTP.Client;
using WorkerServiceEmail.Infrastructure.Logging;
using WorkerServiceEmail.Services;
using WorkerServiceEmail.Services.Consumer;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();

        services.AddMassTransit(x =>
        {
            x.AddConsumer<RecevieInfoConsumer>();
            x.AddConsumer<RecevieErrorConsumer>();
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq://localhost", hst =>
                {
                    hst.Username("guest");
                    hst.Password("guest");
                });

                cfg.ReceiveEndpoint("Email-service-info", e =>
                {
                    e.ConfigureConsumer<RecevieInfoConsumer>(context);
                });
                cfg.ReceiveEndpoint("Email-service-error", e =>
                {
                    e.ConfigureConsumer<RecevieErrorConsumer>(context);
                });
            });
        });

        var config = new ConfigurationBuilder()
              .SetBasePath(System.IO.Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .Build();

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
      
    })
    .UseWindowsService()
    .Build();

await host.RunAsync();
