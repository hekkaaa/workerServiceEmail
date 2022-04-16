using MailKit.Net.Smtp;
using MimeKit;
using System.Net.Sockets;
using WorkerServiceEmail.EntityMessage;
using WorkerServiceEmail.Infrastructure.Logging;

namespace WorkerServiceEmail.Email.SMTP.Client
{
    public class SmtpClientGoogleAsync : IClientSmtp
    {
        private string? _login = EntitySettings.LoginEmailGmail;
        private string? _password = EntitySettings.PasswordEmailGmail;
        private readonly IRunner _runner;

        public SmtpClientGoogleAsync(IRunner runner)
        {
            _runner = runner;
        }
        public async Task<bool> SendAsync(MimeMessage message)
        {
            MimeMessage emailMessage = RebuildEmailFromMessage(message);

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
            catch (ArgumentNullException ex)
            {
                _runner.WarningAction($"Error SMTP.gmail.com ArgumentNullException: Text: {ex.Message}");
                return false;
            }
            catch (MailKit.Security.AuthenticationException ex)
            {
                _runner.WarningAction($"Error SMTP.gmail.com Authentication: Text: {ex.Message}");
                return false;
            }
            catch (SocketException ex)
            {
                _runner.WarningAction($"Error SMTP.gmail.com: Text: {ex.Message}, ErrorCore: {ex.ErrorCode}");
                return false;
            }
            catch (Exception ex)
            {
                _runner.WarningAction($"Error SMTP.gmail.com - {ex.Message}. Letter not delivered: {emailMessage.To}");
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

        private MimeMessage RebuildEmailFromMessage(MimeMessage message)
        {
            string tmpname = message.From[0].Name;
            message.From.Remove(message.From.FirstOrDefault());
            message.From.Add(new MailboxAddress(tmpname, _login));
            return message;
        }
    }
}
