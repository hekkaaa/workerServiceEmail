using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerServiceEmail.Infrastructure.Logging
{
    public class Runner : IRunner
    {
        private readonly ILogger<Runner> _logger;

        public Runner(ILogger<Runner> logger)
        {
            _logger = logger;
        }

        public void DoAction(string name)
        {
            _logger.LogDebug(20, "Doing hard work! {Action}", name);
        }

        public void WarningAction(string name)
        {
            _logger.LogWarning(20, "MESSAGE:  {Action}", name);
        }

        public void InfoAction(string name)
        {
            _logger.LogWarning(20, "MESSAGE:  {Action}", name);
        }

        public void CriticalAction(string name)
        {
            _logger.LogWarning(20, "MESSAGE:  {Action}", name);
        }
    }
}
