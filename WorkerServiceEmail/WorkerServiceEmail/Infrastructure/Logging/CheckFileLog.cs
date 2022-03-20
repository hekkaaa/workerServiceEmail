using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerServiceEmail.Email;
using WorkerServiceEmail.EntityMessage;
using WorkerServiceEmail.Infrastructure.Logging;

namespace WorkerServiceEmail.Infrastructure
{
    public class CheckFileLog
    {
        static FileStream? _createfile;
        public static async Task<Task> CheckFileForSystem(string? userDirectory, IEmailService emailService)
        {
            // Check files.
            try
            {
                string[] dirs = Directory.GetFiles($@"{userDirectory}", "EmailServiceLog.log");

                if (dirs.Length > 0)
                {
                    return Task.CompletedTask;
                }
            }
            catch (Exception ex)
            {
                MessageEmail message = new MessageEmail
                {
                    EmailFrom = "dogsitterclub2022@gmail.com",
                    NameFrom = "Daemon Start Service",
                    EmailTo = "silencemyalise@gmail.com",
                    NameTo = "Administrator Service",
                    Subject = "Service Email Alert!",
                    MessageText = "<b>Log file existence check error!</b><br>" +
                         $"<b>Check Folder path:</b> {userDirectory}<br>" +
                         $"<b>Check File name:</b> тут будет имя файла<br>" +
                         $"<b>Exception text:</b> {ex.Message}"
                };

                await emailService.SendEmailAsync(message);

                return Task.CompletedTask;
            }

            // Create log file.
            try
            {
                _createfile = File.Create("EmailServiceLog.log");
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                MessageEmail message = new MessageEmail
                {
                    EmailFrom = "dogsitterclub2022@gmail.com",
                    NameFrom = "Daemon Start Service",
                    EmailTo = "silencemyalise@gmail.com",
                    NameTo = "Administrator Service",
                    Subject = "Service Email Alert!",
                    MessageText = "<b>Error creating log file</b><br>" +
                    $"<b>Check Folder path:</b> {userDirectory}<br>" +
                    $"<b>The name of the file to be created:</b> {_createfile.Name}<br>" +
                    $"<b>Exception text:</b> {ex.Message}"
                };

                await emailService.SendEmailAsync(message);
                return Task.CompletedTask;
            }
        }
    }
}
