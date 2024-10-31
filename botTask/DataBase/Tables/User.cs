using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace botTask.DataBase.Tables
{
    public class User
    {
        [Key]
        public int IDUser { get; set; }

        private string NickName, Email, TGChatID;
        private DateTime DateReg;

        public string nickName { get { return NickName; } set { NickName = value; } }
        public string email { get { return Email; } set { Email = value; } }
        public DateTime dateReg { get { return DateReg; } set { DateReg = value; } }
        public string tgChatID { get { return TGChatID; } set { TGChatID = value; } }

        public User() { }

        public User(string NickName, string Email, DateTime DateReg, string TGChatID)
        {
            this.NickName = NickName;
            this.Email = Email;
            this.DateReg = DateReg;
            this.TGChatID = TGChatID;
        }
    }

}
