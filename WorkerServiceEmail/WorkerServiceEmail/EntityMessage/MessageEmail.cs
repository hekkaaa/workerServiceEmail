using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerServiceEmail.EntityMessage
{
    public class MessageEmail
    {
        public string? NameFrom { get; set; }
        public string? EmailFrom { get; set; }
        public string? NameTo { get; set; }
        public string? EmailTo { get; set; }
        public string? Subject { get; set; }
        public string? MessageText { get; set; }

    }
}
