using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace botTask
{
    public class ChatClass
    {
        public long chatID { get; set; }
        public string callbackString { get; set; }

        public ChatClass() { }

        public ChatClass(long chatID, string callbackString)
        {
            this.chatID = chatID;
            this.callbackString = callbackString;
        }
    }
}
