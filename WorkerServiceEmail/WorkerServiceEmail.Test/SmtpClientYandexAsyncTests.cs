using Microsoft.Extensions.Logging;
using MimeKit;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using WorkerServiceEmail.Email.SMTP.Client;
using WorkerServiceEmail.EntityMessage;
using WorkerServiceEmail.Infrastructure.Logging;

namespace WorkerServiceEmail.Test
{
    public class SmtpClientYandexAsyncTests
    {
        private IRunner? _runner;

        [SetUp]
        public void Setup()
        {
            var mock = new Mock<ILogger<Runner>>();
            _runner = new Runner(mock.Object);
        }

        [Test]
        public void SendAsyncTest()
        {
            //given
            EntitySettings.LoginEmailYandex = "Laiserk24SKA@yandex.ru";
            EntitySettings.PasswordEmailYandex = "gcpummvbwooiesjj";
            var message = CollectMessageTest();
            var item = new SmtpClientYandexAsync(_runner);

            ////when

            var result = item.SendAsync(message);

            ////then 
            Assert.IsTrue(result.Result);

        }

        [Test]
        public void SendAsyncErrorAuthenticationNegativeTest()
        {
            //given
            EntitySettings.LoginEmailYandex = "Laiserk24SKA@yandex.ru";
            EntitySettings.PasswordEmailYandex = "gadssad111";
            var message = CollectMessageTest();
            var item = new SmtpClientYandexAsync(_runner);

            ////when
            var result = item.SendAsync(message);

            ////then 
            Assert.IsFalse(result.Result);
        }

        [Test]
        public void StatusSmtpConnectAsyncTest()
        {
            //given
            EntitySettings.LoginEmailYandex = "Laiserk24SKA@yandex.ru";
            EntitySettings.PasswordEmailYandex = "gcpummvbwooiesjj";
            var item = new SmtpClientYandexAsync(_runner);
            var expected = new OutputStatusSmtp { SmtpServer = "smtp.yandex.com", Status = true };

            ////when
            var actual = item.StatusSmtpConnectAsync();
           
            ////then 
            Assert.NotNull(actual);
            Assert.AreEqual(expected.SmtpServer, actual.Result.SmtpServer);
            Assert.AreEqual(expected.Status, actual.Result.Status);
        }

        [Test]
        public async Task StatusSmtpConnectAsyncExceptionNegativeTest()
        {
            //given
            EntitySettings.LoginEmailYandex = "Laiserk24SKA@yandex.ru";
            EntitySettings.PasswordEmailYandex = "gcpummv";
            var item = new SmtpClientYandexAsync(_runner);
            var expected = new OutputStatusSmtp { SmtpServer = "smtp.yandex.com", Status = false };

            ////when
            var actual = await item.StatusSmtpConnectAsync();

            ////then 
            Assert.NotNull(actual);
            Assert.AreEqual(expected.SmtpServer, actual.SmtpServer);
            Assert.AreEqual(expected.Status, actual.Status);
        }

        private MimeMessage CollectMessageTest()
        {
            MimeMessage emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("test", "test@testgmail.com"));
            emailMessage.To.Add(new MailboxAddress("Jon Doe", "test@textexample.com"));
            emailMessage.Subject = "Test Message";
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = "<b> Test </b>"
            };

            return emailMessage;
        }
    }
}
