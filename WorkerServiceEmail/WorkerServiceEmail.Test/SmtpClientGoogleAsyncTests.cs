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
        private IRunner _runner;
        private IClientSmtp _smtp;

        [SetUp]
        public void Setup()
        {
            var mock = new Mock<ILogger<Runner>>();
            _runner = new Runner(mock.Object);
           
        }

        [Test]
        public void AuthResponseTokenTest()
        {
            //given
            MimeMessage message = CollectMessageTest();

            var mock = new Mock<IClientSmtp>();
            mock.Setup(a => a.SendAsync(message)).ReturnsAsync(true);
            IClientSmtp controller = mock.Object;

            //when
            Task<bool> result = controller.SendAsync(message);

            //then 
            Assert.IsTrue(result.Result);
        }


        [Test]
        public void AuthResponseTokenAuthenticationNegativeTest()
        {
            //given
            MimeMessage message = CollectMessageTest();
            var mock = new Mock<IClientSmtp>();
            mock.Setup(a => a.SendAsync(message));
            IClientSmtp controller = mock.Object;

            //when
            Task<bool> result = controller.SendAsync(message);

            //then 
            Assert.IsFalse(result.Result);
        }

        [Test]
        public void StatusSmtpConnectAsyncTest()
        {
            //given
            var mock = new Mock<IClientSmtp>();
            mock.Setup(a => a.StatusSmtpConnectAsync()).ReturnsAsync(new OutputStatusSmtp { SmtpServer = "smtp.gmail.com", Status = true });
            IClientSmtp controller = mock.Object;

            //when
            var actual = new OutputStatusSmtp { SmtpServer = "smtp.gmail.com", Status = true };
            var result = controller.StatusSmtpConnectAsync();

            //then 
            Assert.AreEqual(actual.SmtpServer, result.Result.SmtpServer);
            Assert.AreEqual(actual.Status, result.Result.Status);
        }

        [Test]
        public void StatusSmtpConnectAsyncNegativeTest()
        {
            //given
            var mock = new Mock<IClientSmtp>();
            mock.Setup(a => a.StatusSmtpConnectAsync());
            IClientSmtp controller = mock.Object;

            //when
            var actual = new OutputStatusSmtp { SmtpServer = "smtp.gmail.com", Status = true };
            var result = new SmtpClientGoogleAsync(_runner).StatusSmtpConnectAsync();


            //then 
            Assert.NotNull(result.Result);
            //Assert.AreEqual(actual.SmtpServer, result.Result.SmtpServer);
            //Assert.AreEqual(actual.Status, result.Result.Status);
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
