using WorkerServiceEmail.Email;
using WorkerServiceEmail.Email.SMTP.Client;
using WorkerServiceEmail.EntityMessage;
using WorkerServiceEmail.Infrastructure.Logging;

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
            ContextEmailService item = new ContextEmailService();

            item.SetClientSmtp(new SmtpClientGoogleAsync());
            outputList.Add(await item.StatusConnect());

            item.SetClientSmtp(new SmtpClientYandexAsync());
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
                _runner.CriticalAction("Все сервисы отпраки писем SMTP не доступны!");
                return false;
            }
        }

    }
}
