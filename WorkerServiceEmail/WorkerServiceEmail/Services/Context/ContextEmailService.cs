using MimeKit;
using WorkerServiceEmail.Email.SMTP.Client;
using WorkerServiceEmail.EntityMessage;

namespace WorkerServiceEmail.Services
{
    public class ContextEmailService
    {
        private IClientSmtp _clientSmtp;

        public ContextEmailService()
        {
        }

        public ContextEmailService(IClientSmtp clientSmtp)
        {
            _clientSmtp = clientSmtp;
        }

        public void SetClientSmtp(IClientSmtp clientSmtp)
        {
            _clientSmtp = clientSmtp;
        }

        public async Task<bool> SendMail(MimeMessage emailMessage)
        {
            return await _clientSmtp.SendAsync(emailMessage);
        }
        public async Task<OutputStatusSmtp> StatusConnect()
        {
            return await _clientSmtp.StatusSmtpConnectAsync();
        }

    }
}
