using MimeKit;
using WorkerServiceEmail.EntityMessage;
using WorkerServiceEmail.Infrastructure;

namespace WorkerServiceEmail.Services
{
    internal static class CollectMessageExtension
    {
        public static MimeMessage CollectMessage(this MessageEmail tmp, MessageEmail message)
        {
            MimeMessage emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(message.NameFrom, message.EmailFrom));
            emailMessage.To.Add(new MailboxAddress(message.NameTo, message.EmailTo));
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message.MessageText
            };

            return emailMessage;
        }

        public static MimeMessage CollectMessage(this MessageEmail tmp, MessageEmail message, List<OutputStatusSmtp> messageService)
        {
            MimeMessage emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(message.NameFrom, message.EmailFrom));
            emailMessage.To.Add(new MailboxAddress(message.NameTo, message.EmailTo));
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = FillingMessage(messageService)
            };

            return emailMessage;
        }

        private static string FillingMessage(List<OutputStatusSmtp> messageService)
        {
            string test1 = "<b> Status SMTP Connect:</b><br>" +
                $"<b>IP:</b> {IpAddressHelper.GetIpThisHost()} - Successfully launched!<br>";
            
            foreach (OutputStatusSmtp service in messageService)
            {
                if (service.Status)
                {
                    test1 += $@"<b>{service.SmtpServer}</b> = <font color=""green"">{service.Status}</font> {service.ErrorMessage}<br>";
                }
                else
                {
                    test1 += $@"<b>{service.SmtpServer}</b> = <font color=""red"">{service.Status}</font> {service.ErrorMessage}<br>";
                }

            }
            return test1;
        }
    }
}
