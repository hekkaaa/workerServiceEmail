using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerServiceEmail.Email
{
    public class TestSendMail : ITestSendMail
    {
        public async Task<bool> SendEmailAsync()
        {
            string message = "Test message from Class TestSendMail";
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
                await client.AuthenticateAsync("@gmail.com", "!");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
            return true;
        }


        public bool SendEmail()
        {
            string message = "LEROOOOOY !!!";
            string subject = "workerday";

            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("TestEmail Worker", "dogsitterclub2022@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("Alice", "silencemyalise@gmail.com"));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.ConnectAsync("smtp.gmail.com", 25, false);
                client.AuthenticateAsync("", "!");
                client.SendAsync(emailMessage);
                client.DisconnectAsync(true);
            }
            return true;
        }
    }
}
