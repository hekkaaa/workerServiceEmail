
namespace WorkerServiceEmail.Services
{
    public interface ISubService
    {
        Task<bool> Start();
        Task<Task> Stop();
    }
}