using Marvelous.Contracts.Configurations;
using MassTransit;
using WorkerServiceEmail.Email;
using WorkerServiceEmail.EntityMessage;
using WorkerServiceEmail.Infrastructure.Logging;

namespace WorkerServiceEmail.Services.MTConsumer
{
    public class RecevieCfgConsumer : IConsumer<EmailSendlerCfg>
    {
        private readonly IEmailService _emailService;
        private readonly IRunner _runner;
        private string? _emailAdmin = Environment.GetEnvironmentVariable("ADMIN_MAIL");

        public RecevieCfgConsumer(IEmailService emailService, IRunner runner)
        {
            _emailService = emailService;
            _runner = runner;
        }
        public async Task Consume(ConsumeContext<EmailSendlerCfg> context)
        {
            try
            {
                Console.WriteLine($"{context.Message.Key} | {context.Message.Key}");
                //MessageEmail message = new MessageEmail
                //{
                //    EmailFrom = "admin@marvelous.com",
                //    NameFrom = "Email Alarm System",
                //    EmailTo = _emailAdmin,
                //    NameTo = "Administrator",
                //    Subject = $"1",
                //    MessageText = "1"
                //};

                //await _emailService.SendEmailAsync(message);
                //_runner.CriticalAction($"1");
            }
            catch (Exception ex)
            {
                //_runner.WarningAction($@"Mail delivery error: ""admin@marvelous.com"". Error: {ex.Message}");
                throw new Exception();
            }

        }
    }
}
