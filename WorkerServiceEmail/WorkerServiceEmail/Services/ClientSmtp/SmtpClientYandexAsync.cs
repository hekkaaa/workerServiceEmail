using MailKit.Net.Smtp;
using MimeKit;
using WorkerServiceEmail.EntityMessage;

namespace WorkerServiceEmail.Email.SMTP.Client
{
    public class SmtpClientYandexAsync : IClientSmtp
    {
        private string? _login = Environment.GetEnvironmentVariable("LOGIN_EMAIL_YANDEX");
        private string? _password = Environment.GetEnvironmentVariable("PASSWORD_EMAIL_YANDEX");
    
        public async Task<bool> SendAsync(MimeMessage emailMessage)
        {
            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.yandex.com", 25, false);
                    await client.AuthenticateAsync(_login, _password);
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<OutputStatusSmtp> StatusSmtpConnectAsync()
        {
            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.yandex.com", 25, false);
                    await client.AuthenticateAsync(_login, _password);
                    await client.DisconnectAsync(true);

                    return new OutputStatusSmtp { SmtpServer = "smtp.yandex.com", Status = true };
                }
            }
            catch (Exception ex)
            {
                return new OutputStatusSmtp { SmtpServer = "smtp.yandex.com", Status = false, ErrorMessage = ex.Message };
            }
        }
    }
}
