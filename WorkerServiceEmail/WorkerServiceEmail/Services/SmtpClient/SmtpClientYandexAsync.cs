using MailKit.Net.Smtp;
using MimeKit;

namespace WorkerServiceEmail.Email.SMTP.Client
{
    public class SmtpClientYandexAsync
    {
        private MimeMessage _emailMessage;
        public SmtpClientYandexAsync(MimeMessage emailMessage)
        {
            _emailMessage = emailMessage;
        }

        public async Task<bool> SendAsync()
        {
            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.yandex.com", 25, false);
                    await client.AuthenticateAsync("", "");
                    await client.SendAsync(_emailMessage);
                    await client.DisconnectAsync(true);
                    return true;
                }
            }
            catch (Exception ex)
            {
                // тут будет logger 
                return false;
            }

        }

    }
}
