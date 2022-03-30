namespace WorkerServiceEmail.EntityMessage
{
    public interface IMessageErrorEmail
    {
        public string ServiceName { get; set; }
        public string Message { get; set; }
    }
}
