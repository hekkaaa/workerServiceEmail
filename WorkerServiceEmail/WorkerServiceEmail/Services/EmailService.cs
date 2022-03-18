using MimeKit;
using WorkerServiceEmail.Email.SMTP.Client;
using WorkerServiceEmail.EntityMessage;
using WorkerServiceEmail.Infrastructure.Logging;

namespace WorkerServiceEmail.Email
{
    public class EmailService : IEmailService
    {
        private readonly ISmtpClientGoogleAsync _smtpClientGoogleAsync;
        private readonly IRunner _runner;

        public EmailService(ISmtpClientGoogleAsync smtpClientGoogleAsync,IRunner runner)
        {
            _smtpClientGoogleAsync = smtpClientGoogleAsync;
            _runner = runner;
        }
        public async Task<bool> SendEmailAsync(MessageEmail message)
        {
            _runner.InfoAction("Начало отправки письма");

            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(message.NameFrom, message.EmailFrom));
            emailMessage.To.Add(new MailboxAddress(message.NameTo, message.EmailTo));
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message.MessageText
            };

            // Использование SMTP Service и их резервных вариатов.
            var res = await _smtpClientGoogleAsync.SendAsync(emailMessage);
            
            if (!res)
            {
                _runner.WarningAction("Письмо с SMTP Google не отправилось");
                var res1 = new SmtpClientYandexAsync(emailMessage);

                if (!res1.SendAsync().Result)
                {
                    throw new Exception("Ошибка отправки через все варианты SMTP Client");
                }
                return res1.SendAsync().Result;
            }

            _runner.WarningAction("Письмо отправлено");

            return res;
        }
    }
}
