using NLog;
using WorkerServiceEmail.Email;
using WorkerServiceEmail.EntityMessage;
using WorkerServiceEmail.Infrastructure.Logging;

namespace WorkerServiceEmail.Infrastructure
{
    public class CheckingPreparationLogToWork
    {
        static IEmailService? _emailService;
        static string? _userDirectory = Environment.GetEnvironmentVariable("LOG_DIRECTORY");
        static string? _mailTo = Environment.GetEnvironmentVariable("ADMIN_MAIL");

        static IRunner _runner;

        public static async Task<Task> CheckLogFileForSystem(IEmailService emailService,IRunner runner)
        {
            _emailService = emailService;
            _runner = runner;

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
                if (res)
                {
                    _runner.InfoAction("Log save paths have been successfully verified.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogManager.DisableLogging();

                MessageEmail messageReabase = new MessageEmail
                {
                    EmailFrom = _mailTo,
                    NameFrom = "Daemon Start Service",
                    EmailTo = _mailTo,
                    NameTo = "Administrator Service",
                    Subject = "Service Email Alert!",
                    MessageText = "<b>Logs are written on the backup path!</b><br>" +
                        $"<b>New folder path:</b> C:\\Temp<br>" +
                        $"<b>Server:</b> {IpAddressHelper.GetIpThisHost()} <br>" +
                        $"<b>Exception text:</b> {ex.Message}"
                };
                _runner.WarningAction($"Проблема пути сохранения логов по пути {_userDirectory}. Запись логов отключена. Ошибка {ex.Message}");
                await _emailService.SendEmailAsync(messageReabase);

                return false;
            }
            return false;
        }

        private async static Task<bool> CreateNewFileLog()
        {
            // Create log file.
            try
            {
                File.Create($"{_userDirectory}/EmailServiceLog-{DateTime.Now.ToString("dd-MM-yyyy")}.txt").Dispose();
                return true;
            }
            catch (Exception ex)
            {
                MessageEmail message = new MessageEmail
                {
                    EmailFrom = _mailTo,
                    NameFrom = "Daemon Start Service",
                    EmailTo = _mailTo,
                    NameTo = "Administrator Service",
                    Subject = "Service Email Alert!",
                    MessageText = "<b>Error creating log file</b><br>" +
                    $"<b>Check Folder path:</b> {_userDirectory}<br>" +
                    $"<b>The name of the file to be created:</b> EmailServiceLog-{DateTime.Now.ToString("dd-MM-yyyy")}.log<br>" +
                    $"<b>Server:</b> {IpAddressHelper.GetIpThisHost()} <br>" +
                    $"<b>Exception text:</b> {ex.Message}"
                };

                //LogManager.DisableLogging();
                await _emailService.SendEmailAsync(message);

                return true;
            }
        }

        private static bool CheckFileLogFromDirectory()
        {
            try
            {
                var currentDate = DateTime.Now.ToString("dd-MM-yyyy");
                string[] dirs = Directory.GetFiles($@"{_userDirectory}", $"EmailServiceLog-{currentDate}.txt");

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
