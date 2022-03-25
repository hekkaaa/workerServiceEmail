using MailKit.Net.Smtp;
using WorkerServiceEmail.EntityMessage;

namespace WorkerServiceEmail.Services.ClientSmtp
{
    public class SmtpClientYandexStatusConnectAsync : IClientSmtpConnect
    {
        private string? _loginYandex = Environment.GetEnvironmentVariable("LOGIN_EMAIL_YANDEX");
        private string? _passwordYandex = Environment.GetEnvironmentVariable("PASSWORD_EMAIL_YANDEX");
        public async Task<OutputStatusSmtp> StatusSmtpConnectAsync()
        {
            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.yandex.com", 25, false);
                    await client.AuthenticateAsync(_loginYandex, _passwordYandex);
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
