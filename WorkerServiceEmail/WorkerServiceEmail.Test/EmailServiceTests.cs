using Microsoft.Extensions.Logging;
using MimeKit;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkerServiceEmail.Email;
using WorkerServiceEmail.EntityMessage;
using WorkerServiceEmail.Infrastructure.Logging;

namespace WorkerServiceEmail.Test
{
    public class EmailServiceTests
    {

        private IRunner _runner;
        private IEmailService _emailService;
        private MessageEmail _message;
        private List<OutputStatusSmtp> _listOutputSmtp;

        [SetUp]
        public void Setup()
        {
            var mock = new Mock<ILogger<Runner>>();
            _runner = new Runner(mock.Object);

            _emailService = new EmailService(_runner);

           _message =  new MessageEmail()
            {
                EmailFrom = "kek@mamam.ru",
                EmailTo = "Tes@tesmmai.ru",
                MessageText = "Test",
                NameFrom = "Test",
                NameTo = "Test",
                Subject = "lol"
            };

            _listOutputSmtp = new List<OutputStatusSmtp>() { new OutputStatusSmtp() { SmtpServer = "smtp.google",
                ErrorMessage = "TestError", Status = true },
                new OutputStatusSmtp(){  SmtpServer = "smtp.yandex",
                ErrorMessage = "TestError", Status = false } };

        }

        [Test]
        public async Task SendAsyncArgumentExceptionNegativeTest()
        {
            //given
            EntitySettings.LoginEmailYandex = "Laiserk24SKA@yandex.ru";
            EntitySettings.PasswordEmailYandex = "asd";
            EntitySettings.LoginEmailGmail = "dogsitterclub2022@gmail.com";
            EntitySettings.PasswordEmailGmail = "asd!";

            ////when

            ////then 
            Assert.ThrowsAsync<ArgumentException>(()=> _emailService.SendEmailAsync(_message));
        }

        [Test]
        public async Task SendEmailStatusSubServiceAsyncArgumentExceptionNegativeTest()
        {
            //given
            EntitySettings.LoginEmailYandex = "Laiserk24SKA@yandex.ru";
            EntitySettings.PasswordEmailYandex = "asd";
            EntitySettings.LoginEmailGmail = "dogsitterclub2022@gmail.com";
            EntitySettings.PasswordEmailGmail = "asd!";

            ////when
            ///
            ////then 
            Assert.ThrowsAsync<ArgumentException>(() => _emailService.SendEmailStatusSubServiceAsync(_message, _listOutputSmtp));
        }

        [Test]
        public async Task SendEmailStatusSubServiceAsyncTest()
        {
            //given
            EntitySettings.LoginEmailYandex = "Laiserk24SKA@yandex.ru";
            EntitySettings.PasswordEmailYandex = "asd";
            EntitySettings.LoginEmailGmail = "dogsitterclub2022@gmail.com";
            EntitySettings.PasswordEmailGmail = "devedu2022!";

            ////when
            var actual = await _emailService.SendEmailStatusSubServiceAsync(_message, _listOutputSmtp);

            ////then 
            Assert.NotNull(actual);
            Assert.AreEqual(true, actual);
        }
    }
}
