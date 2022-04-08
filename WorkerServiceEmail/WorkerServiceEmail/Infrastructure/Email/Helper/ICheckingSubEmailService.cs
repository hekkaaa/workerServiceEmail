
namespace WorkerServiceEmail.Services
{
    public interface ICheckingSubEmailService
    {
        Task<bool> Start();
        Task<Task> Stop();
    }
}