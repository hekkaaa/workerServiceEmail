using MailKit.Net.Smtp;

namespace WorkerServiceEmail.Email.SMTP.Client
{
    public class SmtpClientYandex
    {
        public SmtpClientYandex()
        { 
            // тут будут настройки для Яндекса

            //using (var client = new SmtpClient())
            //{
            //    client.Connect("smtp.yandex.ru", 25, false);
            //    client.Authenticate("login@yandex.ru", "password");
            //    client.Send(emailMessage);
            //    client.Disconnect(true);
            //}
        }
    }
}
