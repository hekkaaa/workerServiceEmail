using MimeKit;

namespace WorkerServiceEmail.Email.SMTP.Client
{
    public class SmtpClientGoogle
    {
        public SmtpClientGoogle(MimeMessage emailMessage)
        {
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.ConnectAsync("smtp.gmail.com", 25, false);
                client.AuthenticateAsync("dogsitterclub2022@gmail.com", "devedu2022!");
                client.SendAsync(emailMessage);
                client.DisconnectAsync(true);
            }
        }
    }
}
