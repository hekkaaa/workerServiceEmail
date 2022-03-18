using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerServiceEmail.Infrastructure.Logging;

namespace WorkerServiceEmail.Email
{
    public class TestSendMail : ITestSendMail
    {
        private readonly ILogger _logger;
        private readonly IRunner _runner;

        public TestSendMail(ILogger logger, IRunner runner)
        {
            _logger = logger;
            _runner = runner;
        }

        public async Task<bool> SendEmailAsync()
        {
            string message = "Test message from Class TestSendMail Async";
            string subject = "workerday";

            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("TestEmail Worker", "dogsitterclub2022@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("Developer", "silencemyalise@gmail.com"));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 25, false);
                await client.AuthenticateAsync("dogsitterclub2022@gmail.com", "devedu2022!");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
            _runner.WarningAction("Письмо отправлено");
            _logger.LogWarning("Письмо отправлено", DateTimeOffset.Now);
            return true;
        }
    }
}
