using WorkerServiceEmail.EntityMessage;
using MailKit.Net.Smtp;

namespace WorkerServiceEmail.Services.ClientSmtp
{
    public class SmtpClientGoogleStatusConnectAsync : IClientSmtpConnect
    {
        private string? _loginGoogle = Environment.GetEnvironmentVariable("LOGIN_EMAIL_GMAIL");
        private string? _passwordGoogle = Environment.GetEnvironmentVariable("PASSWORD_EMAIL_GMAIL");

        public async Task<OutputStatusSmtp> StatusSmtpConnectAsync()
        {
            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 25, false);
                    await client.AuthenticateAsync(_loginGoogle, _passwordGoogle);
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
