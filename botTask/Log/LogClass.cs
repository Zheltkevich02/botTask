using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace botTask
{
    internal class LogClass
    {
        public string Text { get; set; }
        public string DateLog { get; set; }

        public LogClass() { }

        public LogClass(string text, string DateLog)
        {
            this.Text = text;
            this.DateLog = DateLog;
        }
    }
}
