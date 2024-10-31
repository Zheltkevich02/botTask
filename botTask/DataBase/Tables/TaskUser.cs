using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace botTask.DataBase.Tables
{
    public class TaskUser
    {
        [Key]
        public int IDTasksUsers { get; set; }
        public int IDTask { get; set; }
        public int IDUser { get; set; }

        public TaskUser() { }

        public TaskUser(int IDTask, int IDUser)
        {
            this.IDTask = IDTask;
            this.IDUser = IDUser;
        }
    }

}
