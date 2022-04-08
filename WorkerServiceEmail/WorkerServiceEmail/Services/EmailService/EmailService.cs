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

            MimeMessage emailMessage = new MessageEmail().CollectMessage(message);

            try
            {
                await RouteAndSendMessageInSmptClient(emailMessage);
                return true;
            }
            catch(ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public async Task<bool> SendEmailStatusSubServiceAsync(MessageEmail message, List<OutputStatusSmtp> messageService)
        {
            MimeMessage emailMessage = new MessageEmail().CollectMessage(message, messageService);

            var result = await RouteAndSendMessageInSmptClient(emailMessage);

            _runner.WarningAction($"System mail successfully sent to: {message.EmailTo}");

            return result;
        }

        private async Task<bool> RouteAndSendMessageInSmptClient(MimeMessage emailMessage)
        {
            ContextEmailService context = new ContextEmailService();
            context.SetClientSmtp(new SmtpClientGoogleAsync(_runner));
            Task<bool> res = context.SendMail(emailMessage);

            if (!res.Result)
            {
                context.SetClientSmtp(new SmtpClientYandexAsync(_runner));
                res = context.SendMail(emailMessage);
                if (!res.Result)
                {
                    _runner.CriticalAction("Error all's option SMTP Client");
                    throw new ArgumentException("Error all's option SMTP Client");
                }
            }
            return res.Result;
        }
    }
}
