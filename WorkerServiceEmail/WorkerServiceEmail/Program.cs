using MassTransit;
using MassTransit.Middleware;
using NLog.Extensions.Logging;
using WorkerServiceEmail;
using WorkerServiceEmail.Email;
using WorkerServiceEmail.Email.SMTP.Client;
using WorkerServiceEmail.Infrastructure.Logging;
using WorkerServiceEmail.Services;
using WorkerServiceEmail.Services.Consumer;
using WorkerServiceEmail.Services.MTConsumer;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();

        services.AddMassTransit(x =>
        {
            //x.AddConsumer<RecevieInfoConsumer>();
            //x.AddConsumer<RecevieErrorConsumer>();
            x.AddConsumer<RecevieCfgConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq://80.78.240.16", hst =>
                {
                    hst.Username("nafanya");
                    hst.Password("qwe!23");
                });

                //cfg.ReceiveEndpoint("Email-service-info", e =>
                //{
                //    e.UseKillSwitch(options =>
                //    {
                //        options.TrackingPeriod = TimeSpan.FromSeconds(30);
                //        options.SetActivationThreshold(10);
                //        options.SetTripThreshold(0.15);
                //        options.SetRestartTimeout(m: 1);
                //    });
                //    e.UseDelayedRedelivery(r => r.Intervals(TimeSpan.FromSeconds(10), TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(2)));
                //    e.ConfigureConsumer<RecevieInfoConsumer>(context);
                    
                //});

                //cfg.ReceiveEndpoint("Email-service-error", e =>
                //{
                //    e.UseDelayedRedelivery(r => r.Intervals(TimeSpan.FromSeconds(20), TimeSpan.FromMinutes(15), TimeSpan.FromMilliseconds(60)));
                //    e.ConfigureConsumer<RecevieErrorConsumer>(context);
                //});

                cfg.ReceiveEndpoint("Email-service-cfg", e =>
                {   
                    e.ConfigureConsumer<RecevieCfgConsumer>(context);
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
        services.AddSingleton<ICheckingSubEmailService, CheckingSubEmailService>();
    })
    .UseWindowsService()
    .Build();

await host.RunAsync();