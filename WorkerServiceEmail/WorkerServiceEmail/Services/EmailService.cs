using MimeKit;
using WorkerServiceEmail.Email.SMTP.Client;
using WorkerServiceEmail.EntityMessage;
using WorkerServiceEmail.Infrastructure.Logging;
using WorkerServiceEmail.Services;

namespace WorkerServiceEmail.Email
{
    public class EmailService : IEmailService
    {
        private readonly IRunner _runner;

        public EmailService(IRunner runner)
        {
            _runner = runner;
        }
        public async Task<bool> SendEmailAsync(MessageEmail message)
        {
            _runner.InfoAction("Начало отправки письма");

            MimeMessage emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(message.NameFrom, message.EmailFrom));
            emailMessage.To.Add(new MailboxAddress(message.NameTo, message.EmailTo));
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message.MessageText
            };

            ContextEmailService context = new ContextEmailService();
            context.SetClientSmtp(new SmtpClientGoogleAsync());
            Task<bool> res = context.Test(emailMessage);

            if (!res.Result)
            {
                context.SetClientSmtp(new SmtpClientYandexAsync());
                res = context.Test(emailMessage);
                if (!res.Result)
                {
                    throw new Exception("Ошибка отправки через все варианты SMTP Client");
                }
            }

            _runner.WarningAction("Письмо отправлено");

             return res.Result;
        }
    }
}
