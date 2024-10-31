using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace botTask.DataBase.Tables
{
    public class SettingsUser
    {
        [Key]
        public int IDSettingsUser { get; set; }
        public int IDUser { get; set; }

        private bool GetNotifyByComment, GetNotifyByPriority;

        public bool getNotifyByComment { get { return GetNotifyByComment; } set { GetNotifyByComment = value; } }
        public bool getNotifyByPriority { get { return GetNotifyByPriority; } set { GetNotifyByPriority = value; } }

        public SettingsUser() { }

        public SettingsUser(bool GetNotifyByComment, bool GetNotifyByPriority)
        {
            this.GetNotifyByComment = GetNotifyByComment;
            this.GetNotifyByPriority = GetNotifyByPriority;
        }
    }

}
