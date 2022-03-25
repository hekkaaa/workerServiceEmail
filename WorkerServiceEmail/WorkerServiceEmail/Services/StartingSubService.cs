using WorkerServiceEmail.Email;
using WorkerServiceEmail.EntityMessage;
using WorkerServiceEmail.Infrastructure.Logging;
using WorkerServiceEmail.Services.ClientSmtp;
using WorkerServiceEmail.Services.Context;

namespace WorkerServiceEmail.Services
{
    public class StartingSubService : IStartingSubService
    {
        private readonly IEmailService _emailService;
        private readonly IRunner _runner;

        public StartingSubService(IEmailService emailService, IRunner runner)
        {
            _emailService = emailService;
            _runner = runner;
        }
        public async Task<bool> Start()
        {
            List<OutputStatusSmtp> outputList = new List<OutputStatusSmtp>();
            ContextStartingSubService item = new ContextStartingSubService();

            item.SetClientSmtp(new SmtpClientGoogleStatusConnectAsync());
            outputList.Add(await item.StatusConnect());

            item.SetClientSmtp(new SmtpClientYandexStatusConnectAsync());
            outputList.Add(await item.StatusConnect());


            int nowManyServiceRun = 0;

            foreach (OutputStatusSmtp service in outputList)
            {
                if (service.Status)
                {
                    nowManyServiceRun++;
                }
            }

            if (nowManyServiceRun > 0)
            {
                MessageEmail startMessage = new MessageEmail
                {
                    EmailFrom = "dogsitterclub2022@gmail.com",
                    NameFrom = "Daemon Start Service",
                    EmailTo = "silencemyalise@gmail.com",
                    NameTo = "Administrator Service",
                    Subject = "Service Email Alert!",
                };

                await _emailService.SendEmailStatusSubServiceAsync(startMessage, outputList);

                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
