using MimeKit;
using WorkerServiceEmail.EntityMessage;

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
    }
}
