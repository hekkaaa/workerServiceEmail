using MimeKit;
using WorkerServiceEmail.EntityMessage;

namespace WorkerServiceEmail.Email.SMTP.Client
{
    public interface IClientSmtp
    {
        Task<bool> SendAsync(MimeMessage emailMessage);
        Task<OutputStatusSmtp> StatusSmtpConnectAsync();
    }
}