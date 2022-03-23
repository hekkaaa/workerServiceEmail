using MimeKit;

namespace WorkerServiceEmail.Email.SMTP.Client
{
    public interface IClientSmtp
    {
        Task<bool> SendAsync(MimeMessage emailMessage);
    }
}