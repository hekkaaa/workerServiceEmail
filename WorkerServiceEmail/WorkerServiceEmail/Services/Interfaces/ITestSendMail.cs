
namespace WorkerServiceEmail.Email
{
    public interface ITestSendMail
    {
        bool SendEmail();
        Task<bool> SendEmailAsync();
    }
}