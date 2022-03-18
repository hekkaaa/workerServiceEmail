using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerServiceEmail.Infrastructure
{
    public class CheckFileLog
    {  
        public static bool CheckFile()
        {
            // проверка на есть ли файл для логирования.

            try
            {
                string userDirectory = Directory.GetCurrentDirectory();
                var trs = Directory.GetFiles($@"{userDirectory}\EmailServiceLog.log");
                return true;
            }
            catch (DirectoryNotFoundException)
            {
                try
                {
                    File.Create("EmailServiceLog.log");
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception(); // заглушка. Удалю позднее.
                    // Тут будет отправка письма админа о том что файл логов не создался и записи не идут.
                }
            }
        }

     
    }
}
