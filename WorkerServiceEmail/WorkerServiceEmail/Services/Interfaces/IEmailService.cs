using WorkerServiceEmail.EntityMessage;

namespace WorkerServiceEmail.Email
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(MessageEmail message);
    }
}