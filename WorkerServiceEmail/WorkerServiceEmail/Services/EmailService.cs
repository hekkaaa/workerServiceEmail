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

            MimeMessage emailMessage = new MessageEmail().CollectMessage(message);

            var result = await RouteAndSendMessageInSmptClient(emailMessage);

            _runner.WarningAction("Письмо отправлено");

             return result;
        }

        public async Task<bool> SendEmailStatusSubServiceAsync(MessageEmail message, List<OutputStatusSmtp> messageService)
        {
            _runner.InfoAction("Начало отправки письма");

            MimeMessage emailMessage = new MessageEmail().CollectMessage(message, messageService);

            var result = await RouteAndSendMessageInSmptClient(emailMessage);

            _runner.WarningAction("Письмо отправлено");

            return result;
        }

        private async Task<bool> RouteAndSendMessageInSmptClient(MimeMessage emailMessage)
        {
            ContextEmailService context = new ContextEmailService();
            context.SetClientSmtp(new SmtpClientGoogleAsync());
            Task<bool> res = context.SendMail(emailMessage);

            if (!res.Result)
            {
                context.SetClientSmtp(new SmtpClientYandexAsync());
                res = context.SendMail(emailMessage);
                if (!res.Result)
                {
                    throw new Exception("Ошибка отправки через все варианты SMTP Client");
                }
            }
            return res.Result;
        }
    }
}
