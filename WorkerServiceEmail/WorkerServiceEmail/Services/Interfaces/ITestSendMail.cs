
namespace WorkerServiceEmail.Email
{
    public interface ITestSendMail
    {
        Task<bool> SendEmailAsync();
    }
}