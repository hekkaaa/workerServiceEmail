namespace WorkerServiceEmail.Infrastructure.Logging
{
    public interface IRunner
    {
        void CriticalAction(string name);
        void DoAction(string name);
        void InfoAction(string name);
        void WarningAction(string name);
    }
}