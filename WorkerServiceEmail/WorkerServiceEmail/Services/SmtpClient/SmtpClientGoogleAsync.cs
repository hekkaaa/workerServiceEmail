using MimeKit;
using MailKit.Net.Smtp;
using WorkerServiceEmail.Infrastructure.Logging;

namespace WorkerServiceEmail.Email.SMTP.Client
{
    public class SmtpClientGoogleAsync : ISmtpClientGoogleAsync
    {
        private readonly IRunner _runner;
        private string? _login =  Environment.GetEnvironmentVariable("LOGIN_EMAIL_GMAIL");
        private string? _password = Environment.GetEnvironmentVariable("PASSWORD_EMAIL_GMAIL");
      
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
                    await client.AuthenticateAsync(_login, _password);
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
    }
}
