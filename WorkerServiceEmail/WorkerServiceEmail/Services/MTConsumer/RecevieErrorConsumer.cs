using Marvelous.Contracts.EmailMessageModels;
using MassTransit;
using WorkerServiceEmail.Email;
using WorkerServiceEmail.EntityMessage;
using WorkerServiceEmail.Infrastructure.Logging;

namespace WorkerServiceEmail.Services.Consumer
{
    public class RecevieErrorConsumer : IConsumer<EmailErrorMessage>
    {
        private readonly IEmailService _emailService;
        private readonly IRunner _runner;
        private string? _emailAdmin = Environment.GetEnvironmentVariable("ADMIN_MAIL");
      
        public RecevieErrorConsumer(IEmailService emailService, IRunner runner)
        {
            _emailService = emailService;
            _runner = runner;
        }
        public async Task Consume(ConsumeContext<EmailErrorMessage> context)
        {
            try
            {
                MessageEmail message = new MessageEmail
                {
                    EmailFrom = "admin@marvelous.com",
                    NameFrom = "Email Alarm System",
                    EmailTo = _emailAdmin,
                    NameTo = "Administrator",
                    Subject = $"Critical! System microservice {context.Message.ServiceName?.ToUpper()} is down!",
                    MessageText = context.Message.TextMessage
                };

                await _emailService.SendEmailAsync(message);
                _runner.CriticalAction($"A letter was sent to the administrator about the fall of the service: {context.Message.ServiceName}");
            }
            catch(Exception ex)
            {
                _runner.WarningAction($@"Mail delivery error: ""admin@marvelous.com"". Error: {ex.Message}");
                throw new Exception();
            }
           
        }
    }
}
