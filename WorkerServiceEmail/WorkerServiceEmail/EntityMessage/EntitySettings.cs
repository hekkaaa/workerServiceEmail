using WorkerServiceEmail.Infrastructure;

namespace WorkerServiceEmail.EntityMessage
{
    public static class EntitySettings
    {
        public static string? LoginEmailGmail { get; set; } = RequestSetting.ReturnValueByKey("LOGIN_EMAIL_GMAIL");
        public static string? PasswordEmailGmail { get; set; } = RequestSetting.ReturnValueByKey("PASSWORD_EMAIL_GMAIL");
        public static string? LoginEmailYandex { get; set; } = RequestSetting.ReturnValueByKey("LOGIN_EMAIL_YANDEX");
        public static string? PasswordEmailYandex { get; set; } = RequestSetting.ReturnValueByKey("PASSWORD_EMAIL_YANDEX");
        public static string? AdminMail { get; set; } = RequestSetting.ReturnValueByKey("ADMIN_MAIL");

    }
}
