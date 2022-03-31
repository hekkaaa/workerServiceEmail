using MailKit.Net.Smtp;
using MimeKit;
using WorkerServiceEmail.EntityMessage;
using WorkerServiceEmail.Infrastructure.Logging;

namespace WorkerServiceEmail.Email.SMTP.Client
{
    public class SmtpClientGoogleAsync : IClientSmtp
    {
        private string? _login = Environment.GetEnvironmentVariable("LOGIN_EMAIL_GMAIL");
        private string? _password = Environment.GetEnvironmentVariable("PASSWORD_EMAIL_GMAIL");
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
                    await client.AuthenticateAsync(_login, _password);
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _runner.WarningAction($"Error smtp.gmail.com - {ex.Message}. Letter not delivered: {emailMessage.To}");
                return false;
            }
        }

        public async Task<OutputStatusSmtp> StatusSmtpConnectAsync()
        {
            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 25, false);
                    await client.AuthenticateAsync(_login, _password);
                    await client.DisconnectAsync(true);

                    return new OutputStatusSmtp { SmtpServer = "smtp.gmail.com", Status = true };
                }
            }
            catch (Exception ex)
            {
                return new OutputStatusSmtp { SmtpServer = "smtp.gmail.com", Status = false, ErrorMessage = ex.Message };
            }
        }
    }
}
