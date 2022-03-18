using MimeKit;

namespace WorkerServiceEmail.Email.SMTP.Client
{
    public interface ISmtpClientGoogleAsync
    {
        Task<bool> SendAsync(MimeMessage emailMessage);
    }
}