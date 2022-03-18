using MimeKit;
using MailKit.Net.Smtp;
using WorkerServiceEmail.Infrastructure.Logging;

namespace WorkerServiceEmail.Email.SMTP.Client
{
    public class SmtpClientGoogleAsync : ISmtpClientGoogleAsync
    {
        private readonly IRunner _runner;
      
        public SmtpClientGoogleAsync(IRunner runner)
        {
            _runner = runner;

        }
        public async Task<bool> SendAsync(MimeMessage emailMessage)
        {
            try
            {
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 25, false);
                    await client.AuthenticateAsync("dogsitterclub2022@gmail.com", "devedu2022!");
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                    _runner.WarningAction("Письмо отправилось 1111!");
                    return true;
                 
                }
            }
            catch (Exception ex)
            {
                _runner.CriticalAction("Письмо не отправилось!");
                return false;
            }

        }

        public bool Send1(MimeMessage emailMessage)
        {
            try
            {
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                     client.ConnectAsync("smtp.gmail.com", 25, false);
                     client.AuthenticateAsync("dogsitterclub2022@gmail.com", "devedu2022!");
                     client.SendAsync(emailMessage);
                     client.DisconnectAsync(true);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _runner.CriticalAction("Письмо не отправилось!");
                return false;
            }

        }
    }
}
