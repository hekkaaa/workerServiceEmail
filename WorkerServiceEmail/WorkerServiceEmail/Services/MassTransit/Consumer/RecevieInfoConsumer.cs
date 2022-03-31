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

        public Task Consume(ConsumeContext<EmailInfoMessage> context)
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

            _emailService.SendEmailAsync(message);
            _runner.InfoAction($"Message sent to client: {context.Message.EmailTo}");
            return Task.CompletedTask;
        }
    }
}
