using MimeKit;
using MailKit.Net.Smtp;

namespace WorkerServiceEmail.Email.SMTP.Client
{
    public class SmtpClientGoogleAsync : ISmtpClientGoogleAsync
    {
        public async Task<bool> SendAsync(MimeMessage emailMessage)
        {
            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 25, false);
                    await client.AuthenticateAsync("dogsitterclub2022@gmail.com", "devedu2022!");
                    await client.SendAsync(emailMessage);
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
