using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerServiceEmail.Infrastructure.Logging;

namespace WorkerServiceEmail.Infrastructure
{
    public class CheckFileLog
    {
        public static Task CheckFileForSystem(string userDirectory)
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
                // отправить письмо
                return Task.CompletedTask;
            }

            // Create log file.
            try
            {
                File.Create("EmailServiceLog.log");
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {   
                throw new Exception();
                // заглушка. Удалю позднее.
                // Тут будет отправка письма админа о том что файл логов не создался и записи не идут.
            }
        }
    }
}
