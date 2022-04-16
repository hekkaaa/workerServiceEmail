using WorkerServiceEmail.Email;
using WorkerServiceEmail.Email.SMTP.Client;
using WorkerServiceEmail.EntityMessage;
using WorkerServiceEmail.Infrastructure;
using WorkerServiceEmail.Infrastructure.Logging;

namespace WorkerServiceEmail.Services
{
    public class CheckingSubEmailService : ICheckingSubEmailService
    {
        private readonly IEmailService _emailService;
        private readonly IRunner _runner;
        private string? _mailAdmin = EntitySettings.AdminMail;

        public CheckingSubEmailService(IEmailService emailService, IRunner runner)
        {
            _emailService = emailService;
            _runner = runner;
        }
        public async Task<bool> Start()
        {
            List<OutputStatusSmtp> outputList = new List<OutputStatusSmtp>();
            ContextEmailService contextItemEmail = new ContextEmailService(new SmtpClientGoogleAsync(_runner));

            outputList.Add(await contextItemEmail.StatusConnect());

            contextItemEmail.SetClientSmtp(new SmtpClientYandexAsync(_runner));
            outputList.Add(await contextItemEmail.StatusConnect());

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
                    EmailFrom = _mailAdmin,
                    NameFrom = "Daemon Service",
                    EmailTo = _mailAdmin,
                    NameTo = "Email Alarm System",
                    Subject = "Start Service",
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
        public async Task<Task> Stop()
        {
            MessageEmail startMessage = new MessageEmail
            {
                EmailFrom = _mailAdmin,
                NameFrom = "Daemon Service",
                EmailTo = _mailAdmin,
                NameTo = "Email Alarm System",
                Subject = "Stop Service ",
                MessageText = "<b>Service Email from Windows Server</b><br>" +
            $"<b>IP:</b> {IpAddressHelper.GetIpThisHost()} - Service stopped!"
            };

            await _emailService.SendEmailAsync(startMessage);

            return Task.CompletedTask;
        }
    }
}
