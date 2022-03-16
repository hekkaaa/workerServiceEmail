using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerServiceEmail.Email.SMTP.Client
{
    public class SmtpClientYandexAsync
    {
        public SmtpClientYandexAsync()
        {
            // тут будут настройки для Яндекса async

            //using (var client = new SmtpClient())
            //{
            //    await client.Connect("smtp.yandex.ru", 25, false);
            //    await client.Authenticate("login@yandex.ru", "password");
            //    await client.Send(emailMessage);
            //    await client.Disconnect(true);
            //}
        }

    }
}
