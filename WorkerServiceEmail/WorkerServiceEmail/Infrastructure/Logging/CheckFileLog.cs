using NLog;
using WorkerServiceEmail.Email;
using WorkerServiceEmail.EntityMessage;

namespace WorkerServiceEmail.Infrastructure
{
    public class CheckFileLog
    {
        static IEmailService? _emailService;
        static string? _userDirectory = Environment.GetEnvironmentVariable("LOG_DIRECTORY");

        public static async Task<Task> CheckLogFileForSystem(IEmailService emailService)
        {
            _emailService = emailService;

            bool result = await CheckPresenceLogFileForSystem();

            if(result is true)
            {
                return Task.CompletedTask;
            }

            bool resiltCreate = await CreateNewFileLog();

            if (resiltCreate is true)
            {
                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }

        private async static Task<bool> CheckPresenceLogFileForSystem()
        {
            // Check files.
            try
            {
                var res = CheckFileLogFromDirectory();
                if (res) return true;
            }
            catch (Exception ex)
            {
                try
                {
                    ReBasePathFileNlog();
                    _userDirectory = @"C:\Temp\";
                    CheckFileLogFromDirectory();

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

                    await _emailService.SendEmailAsync(messageReabase);

                    return true;
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

                    await _emailService.SendEmailAsync(message);

                    return true;
                }
            }
            return false;
        }

        private async static Task<bool> CreateNewFileLog()
        {
            // Create log file.
            try
            {
                File.Create($"{_userDirectory}/EmailServiceLog-{DateTime.Now.ToString("dd-MM-yyyy")}.log").Dispose();
                return true;
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
                    $"<b>Check Folder path:</b> {_userDirectory}<br>" +
                    $"<b>The name of the file to be created:</b> EmailServiceLog-{DateTime.Now.ToString("dd-MM-yyyy")}.log<br>" +
                    $"<b>Server:</b> {GetIpAddresHost.GetIpThisHost()} <br>" +
                    $"<b>Exception text:</b> {ex.Message}"
                };

                LogManager.DisableLogging();
                await _emailService.SendEmailAsync(message);

                return true;
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
