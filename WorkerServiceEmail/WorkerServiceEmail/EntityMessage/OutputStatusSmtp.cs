using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerServiceEmail.EntityMessage
{
    public class OutputStatusSmtp
    {
        public string? SmtpServer { get; set; }
        public bool Status { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
