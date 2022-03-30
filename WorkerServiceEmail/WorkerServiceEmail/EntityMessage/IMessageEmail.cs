namespace WorkerServiceEmail.EntityMessage
{
    public interface IMessageEmail
    {
        string? EmailFrom { get; set; }
        string? EmailTo { get; set; }
        string? MessageText { get; set; }
        string? NameFrom { get; set; }
        string? NameTo { get; set; }
        string? Subject { get; set; }
    }
}