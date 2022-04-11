using MassTransit;
using NLog.Extensions.Logging;
using WorkerServiceEmail;
using WorkerServiceEmail.Email;
using WorkerServiceEmail.Email.SMTP.Client;
using WorkerServiceEmail.Infrastructure;
using WorkerServiceEmail.Infrastructure.Logging;
using WorkerServiceEmail.Services;
using WorkerServiceEmail.Services.Consumer;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        RequestSetting.RequestSettingfromServer();

        services.AddHostedService<Worker>();
        services.AddMassTransit(x =>
        {
            x.AddConsumer<RecevieInfoConsumer>();
            x.AddConsumer<RecevieErrorConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {

                cfg.ReceiveEndpoint("Email-service-info", e =>
                {
                    e.UseKillSwitch(options =>
                    {
                        options.TrackingPeriod = TimeSpan.FromSeconds(30);
                        options.SetActivationThreshold(10);
                        options.SetTripThreshold(0.15);
                        options.SetRestartTimeout(m: 3);
                    });
                    e.UseDelayedRedelivery(r => r.Intervals(TimeSpan.FromSeconds(40), TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(40)));
                    e.ConfigureConsumer<RecevieInfoConsumer>(context);

                });

                cfg.ReceiveEndpoint("Email-service-error", e =>
                {
                    e.UseDelayedRedelivery(r => r.Intervals(TimeSpan.FromSeconds(20), TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10)));
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
            loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
            loggingBuilder.AddNLog(config);
        });
        services.AddSingleton<IEmailService, EmailService>();
        services.AddSingleton<IClientSmtp, SmtpClientGoogleAsync>();
        services.AddSingleton<ICheckingSubEmailService, CheckingSubEmailService>();
    })
    .UseWindowsService()
    .Build();

await host.RunAsync();