using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace botTask.DataBase.Tables
{
    public class Status
    {
        public int IDStatus { get; set; }

        private string StatusName;

        public string statusName { get { return StatusName; } set { StatusName = value; } }

        public Status() { }

        public Status(string StatusName)
        {
            this.StatusName = StatusName;
        }
    }

}
