using WorkerServiceEmail.EntityMessage;

namespace WorkerServiceEmail.Services.ClientSmtp
{
    public interface IClientSmtpConnect
    {
        Task<OutputStatusSmtp> StatusSmtpConnectAsync();
    }
}