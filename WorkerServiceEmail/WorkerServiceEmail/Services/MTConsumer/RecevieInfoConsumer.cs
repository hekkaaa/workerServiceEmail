using Marvelous.Contracts.EmailMessageModels;
using MassTransit;
using WorkerServiceEmail.Email;
using WorkerServiceEmail.EntityMessage;
using WorkerServiceEmail.Infrastructure.Logging;

namespace WorkerServiceEmail.Services.Consumer
{
    public class RecevieInfoConsumer : IConsumer<EmailInfoMessage>
    { 
        private readonly IEmailService _emailService;
        private readonly IRunner _runner;
        public RecevieInfoConsumer(IEmailService emailService, IRunner runner)
        {
            _emailService = emailService;
            _runner = runner;
        }

        public async Task Consume(ConsumeContext<EmailInfoMessage> context)
        {
            try
            {
                MessageEmail message = new MessageEmail
                {
                    EmailFrom = "help@marvelous.com",
                    NameFrom = "Bank of Marvelous",
                    EmailTo = context.Message.EmailTo,
                    NameTo = context.Message.NameTo,
                    Subject = context.Message.Subject,
                    MessageText = context.Message.TextMessage

                };

                await _emailService.SendEmailAsync(message);
                _runner.InfoAction($"Message sent to client: {context.Message.EmailTo}");
            }
            catch (Exception ex)
            {
                _runner.WarningAction($@"Mail delivery error: ""{context.Message.EmailTo}"". Error: {ex.Message}");
                throw new Exception();
            }
        }
    }
}
