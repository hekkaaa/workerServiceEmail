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
    public class SmtpClientGoogleAsyncTests
    {
        private IRunner? _runner;

        [SetUp]
        public void Setup()
        {
            var mock = new Mock<ILogger<Runner>>();
            _runner = new Runner(mock.Object);
        }

        [Test]
        public async Task SendAsyncTest()
        {
            //given
            EntitySettings.LoginEmailGmail = "dogsitterclub2022@gmail.com";
            EntitySettings.PasswordEmailGmail = "devedu2022!";
            var message = CollectMessageTest();
            var item = new SmtpClientGoogleAsync(_runner);

            ////when
            var result = await item.SendAsync(message);

            ////then 
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task StatusSmtpConnectAsyncTest()
        {
            //given
            EntitySettings.LoginEmailGmail = "dogsitterclub2022@gmail.com";
            EntitySettings.PasswordEmailGmail = "devedu2022!";
            var item = new SmtpClientGoogleAsync(_runner);
            var expected = new OutputStatusSmtp { SmtpServer = "smtp.gmail.com", Status = true };

            ////when
            var actual = await item.StatusSmtpConnectAsync();

            ////then 
            Assert.NotNull(actual);
            Assert.AreEqual(expected.SmtpServer, actual.SmtpServer);
            Assert.AreEqual(expected.Status, actual.Status);
        }

        [Test]
        public async Task StatusSmtpConnectAsyncExceptionNegativeTest()
        {
            //given
            EntitySettings.LoginEmailGmail = "dogsitterclub2022@gmail.com";
            EntitySettings.PasswordEmailGmail = "gcpummvb";
            var item = new SmtpClientGoogleAsync(_runner);
            var expected = new OutputStatusSmtp { SmtpServer = "smtp.gmail.com", Status = false };

            ////when
            var actual = await item.StatusSmtpConnectAsync();

            ////then 
            Assert.NotNull(actual);
            Assert.AreEqual(expected.SmtpServer, actual.SmtpServer);
            Assert.AreEqual(expected.Status, actual.Status);
        }

        [Test]
        public async Task SendAsyncErrorAuthenticationNegativeTest()
        {
            //given
            EntitySettings.LoginEmailGmail = "dogsitterclub2022@gmail.com";
            EntitySettings.PasswordEmailGmail = "asdasd11s!";
            var message = CollectMessageTest();
            var item = new SmtpClientGoogleAsync(_runner);

            ////when
            var result = await item.SendAsync(message);

            ////then 
            Assert.IsFalse(result);
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
