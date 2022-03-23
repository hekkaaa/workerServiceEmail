using MimeKit;

namespace WorkerServiceEmail.Email.SMTP.Client
{
    public class SmtpClientGoogleAsync : IClientSmtp
    {
        private string? _login = Environment.GetEnvironmentVariable("LOGIN_EMAIL_GMAIL");
        private string? _password = Environment.GetEnvironmentVariable("PASSWORD_EMAIL_GMAIL");

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
                return false;
            }
        }
    }
}
