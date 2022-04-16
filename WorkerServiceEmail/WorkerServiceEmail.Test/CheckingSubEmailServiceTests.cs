using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkerServiceEmail.Email;
using WorkerServiceEmail.EntityMessage;
using WorkerServiceEmail.Infrastructure.Logging;
using WorkerServiceEmail.Services;

namespace WorkerServiceEmail.Test
{
    public class CheckingSubEmailServiceTests
    {
        private IRunner _runner;
        private Mock<IEmailService> _emailService = new Mock<IEmailService>();
        private MessageEmail _message;

        [SetUp]
        public void Setup()
        {
            var mock = new Mock<ILogger<Runner>>();
            _runner = new Runner(mock.Object);

            _message = new MessageEmail()
            {
                EmailFrom = "kek@mamam.ru",
                EmailTo = "Tes@tesmmai.ru",
                MessageText = "Test",
                NameFrom = "Test",
                NameTo = "Test",
                Subject = "lol"
            };
        }

        [Test]
        public async Task StartTest()
        {
            //given
            EntitySettings.LoginEmailGmail = "dogsitterclub2022@gmail.com";
            EntitySettings.PasswordEmailGmail = "devedu2022!";
            _emailService.Setup(x => x.SendEmailAsync(_message)).ReturnsAsync(true);
            var actiualItemEmailService = _emailService.Object;

            var preItemExpected = new CheckingSubEmailService(actiualItemEmailService, _runner);

            //when
            var expected = await preItemExpected.Start();

            //then 
            Assert.IsTrue(expected);
        }

        [Test]
        public async Task StartNotStartSMTPServiceNegativeTest()
        {
            //given
            EntitySettings.LoginEmailGmail = "dogsitterclub2022@gmail.com";
            EntitySettings.PasswordEmailGmail = "11221112!";
            _emailService.Setup(x => x.SendEmailAsync(_message)).ReturnsAsync(true);
            var actiualItemEmailService = _emailService.Object;

            var preItemExpected = new CheckingSubEmailService(actiualItemEmailService, _runner);

            //when
            var expected = await preItemExpected.Start();

            //then 
            Assert.IsFalse(expected);
        }

        [Test]
        public async Task StopTest()
        {
            //given
            EntitySettings.LoginEmailGmail = "dogsitterclub2022@gmail.com";
            EntitySettings.PasswordEmailGmail = "devedu2022!";
            _emailService.Setup(x => x.SendEmailAsync(_message)).ReturnsAsync(true);
            var actiualItemEmailService = _emailService.Object;

            var preItemExpected = new CheckingSubEmailService(actiualItemEmailService, _runner);

            //when
            var expected = await preItemExpected.Stop();

            //then 
            Assert.NotNull(expected);
            Assert.IsFalse(expected.IsCanceled);

        }
    }
}
