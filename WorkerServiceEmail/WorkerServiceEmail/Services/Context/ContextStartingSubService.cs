using WorkerServiceEmail.EntityMessage;
using WorkerServiceEmail.Services.ClientSmtp;

namespace WorkerServiceEmail.Services.Context
{
    public class ContextStartingSubService
    {
        private IClientSmtpConnect _clientSmtp;

        public ContextStartingSubService()
        {
        }

        public ContextStartingSubService(IClientSmtpConnect clientSmtp)
        {
            _clientSmtp = clientSmtp;
        }

        public void SetClientSmtp(IClientSmtpConnect clientSmtp)
        {
            this._clientSmtp = clientSmtp;
        }

        public async Task<OutputStatusSmtp> StatusConnect()
        {
            return await _clientSmtp.StatusSmtpConnectAsync();
        }
    }
}
