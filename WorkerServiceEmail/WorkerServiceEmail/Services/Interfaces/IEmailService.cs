using WorkerServiceEmail.Email.SMTP.Client;
using WorkerServiceEmail.EntityMessage;

namespace WorkerServiceEmail.Email
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(MessageEmail message);
        Task<bool> SendEmailStatusSubServiceAsync(MessageEmail message, List<OutputStatusSmtp> messageService);
    }
}