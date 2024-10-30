using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace botTask.DataBase.Tables
{
    public class Setting
    {
        public int Ver { get; set; }

        private string TelegramKey;
        private DateTime DateUpdate;

        public string telegramKey { get { return TelegramKey; } set { TelegramKey = value; } }
        public DateTime dateUpdate { get { return DateUpdate; } set { DateUpdate = value; } }

        public Setting() { }

        public Setting(string TelegramKey, DateTime DateUpdate)
        {
            this.TelegramKey = TelegramKey;
            this.DateUpdate = DateUpdate;
        }
    }

}
