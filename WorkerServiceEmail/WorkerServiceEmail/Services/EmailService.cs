using MimeKit;
using WorkerServiceEmail.Email.SMTP.Client;
using WorkerServiceEmail.EntityMessage;

namespace WorkerServiceEmail.Email
{
    public class EmailService : IEmailService
    {
        private readonly ISmtpClientGoogleAsync _smtpClientGoogleAsync;

        public EmailService(ISmtpClientGoogleAsync smtpClientGoogleAsync)
        {
            _smtpClientGoogleAsync = smtpClientGoogleAsync;
        }
        public async Task<bool> SendEmailAsync(MessageEmail message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(message.NameFrom, message.EmailFrom));
            emailMessage.To.Add(new MailboxAddress(message.NameTo, message.EmailFrom));
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message.MessageText
            };

            // Использование SMTP Service и их резервных вариатов.
            var res = _smtpClientGoogleAsync.SendAsync(emailMessage);

            if (!res.Result)
            {
                var res1 = new SmtpClientYandexAsync(emailMessage);
                if (!res1.SendAsync().Result)
                {
                    throw new Exception("Ошибка отправки через все варианты SMTP Client");
                }
                return res1.SendAsync().Result;
            }
            return res.Result;
        }
    }
}
