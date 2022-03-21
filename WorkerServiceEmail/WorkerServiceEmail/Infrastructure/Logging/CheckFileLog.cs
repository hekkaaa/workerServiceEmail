using NLog;
using WorkerServiceEmail.Email;
using WorkerServiceEmail.EntityMessage;

namespace WorkerServiceEmail.Infrastructure
{
    public class CheckFileLog
    {
        static FileStream? _createfile;
        static string _userDirectory = String.Empty;
        public static async Task<Task> CheckFileForSystem(string? userDirectory, IEmailService emailService)
        {
            _userDirectory = userDirectory;
       
            // Check files.
            try
            {
                var res = CheckFileLogFromDirectory();
                if (res) return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                try
                {
                    ReBasePathFileNlog();
                    _userDirectory = @"C:\Temp\";
                    var res = CheckFileLogFromDirectory();

                    if (res)
                    {
                        MessageEmail messageReabase = new MessageEmail
                        {
                            EmailFrom = "dogsitterclub2022@gmail.com",
                            NameFrom = "Daemon Start Service",
                            EmailTo = "silencemyalise@gmail.com",
                            NameTo = "Administrator Service",
                            Subject = "Service Email Alert!",
                            MessageText = "<b>Logs are written on the backup path!</b><br>" +
                             $"<b>New folder path:</b> C:\\Temp<br>" +
                             $"<b>Server:</b> {GetIpAddresHost.GetIpThisHost()} <br>" +
                             $"<b>Exception text:</b> {ex.Message}"
                        };

                        await emailService.SendEmailAsync(messageReabase);

                        return Task.CompletedTask;
                    }
                }
                catch
                {
                    LogManager.DisableLogging();

                    MessageEmail message = new MessageEmail
                    {
                        EmailFrom = "dogsitterclub2022@gmail.com",
                        NameFrom = "Daemon Start Service",
                        EmailTo = "silencemyalise@gmail.com",
                        NameTo = "Administrator Service",
                        Subject = "Service Email Alert!",
                        MessageText = "<h2><b>Log file existence check error!</b></h2><br>" +
                        "!!!!Logging is completely disabled!!!<br>" +
                        $"<b>Server:</b> {GetIpAddresHost.GetIpThisHost()} <br>" +
                         $"<b>Exception text:</b> {ex.Message}"
                    };

                    await emailService.SendEmailAsync(message);

                    return Task.CompletedTask;
                }
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
                    $"<b>Server:</b> {GetIpAddresHost.GetIpThisHost()} <br>" +
                    $"<b>Exception text:</b> {ex.Message}"
                };

                await emailService.SendEmailAsync(message);
                return Task.CompletedTask;
            }
        }

        private static void ReBasePathFileNlog()
        {   
            LogManager.LoadConfiguration("nlog_reserve.config");
        }

        private static bool CheckFileLogFromDirectory()
        {
            try
            {
                var currentDate = DateTime.Now.ToString("dd-MM-yyyy");
                string[] dirs = Directory.GetFiles($@"{_userDirectory}", $"EmailServiceLog-{currentDate}.log");

                if (dirs.Length > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
