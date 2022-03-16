using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerServiceEmail.Email.SMTP.Client;

namespace WorkerServiceEmail.Email
{
    public class EmailService
    {
        //public async Task SendEmailAsync(string email, string subject, string message)
        //{
            
        //}

        public bool SendEmail(string nameFrom, string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Daemon Service Email", "dogsitterclub2022@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            try
            {
                new SmtpClientGoogle(emailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("HELP!");
                return false;
            }
        }
    }
}
