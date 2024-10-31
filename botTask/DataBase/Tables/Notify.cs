using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace botTask.DataBase.Tables
{
    public class Notify
    {
        [Key]
        public int IDNotify { get; set; }
        public int IDUser { get; set; }

        private int TypeNotify;
        private string Text;
        private DateTime DateNotify;
        private bool Send;

        public int typeNotify { get { return TypeNotify; } set { TypeNotify = value; } }
        public string text { get { return Text; } set { Text = value; } }
        public DateTime dateNotify { get { return DateNotify; } set { DateNotify = value; } }
        public bool send { get { return Send; } set { Send = value; } }

        public Notify() { }

        public Notify(int TypeNotify, string Text, DateTime DateNotify, bool Send)
        {
            this.TypeNotify = TypeNotify;
            this.Text = Text;
            this.DateNotify = DateNotify;
            this.Send = Send;
        }
    }

}
