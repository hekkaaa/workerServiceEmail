using MassTransit;
using MassTransit.Middleware;
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
                //cfg.UseScheduledRedelivery(r => r.Intervals(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(2)));
                //cfg.UseDelayedRedelivery(r => r.Interval(3,TimeSpan.FromSeconds(10)));
                //cfg.UseMessageRetry(r => r.Immediate(1));
             
                cfg.Host("rabbitmq://localhost", hst =>
                {
                    hst.Username("guest");
                    hst.Password("guest");
                });

                cfg.ReceiveEndpoint("Email-service-info", e =>
                {
                    e.ConcurrentMessageLimit = 6;
                    e.UseDelayedRedelivery(r => r.Intervals(TimeSpan.FromSeconds(10), TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(2)));
                    e.ConfigureConsumer<RecevieInfoConsumer>(context);
                });

                cfg.ReceiveEndpoint("Email-service-error", e =>
                {
                    e.ConcurrentMessageLimit = 2;
                    e.UseDelayedRedelivery(r => r.Intervals(TimeSpan.FromSeconds(20), TimeSpan.FromMinutes(15), TimeSpan.FromMilliseconds(60)));
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
        services.AddSingleton<ISubService, CheckingSubEmailService>();
    })
    .UseWindowsService()
    .Build();

await host.RunAsync();