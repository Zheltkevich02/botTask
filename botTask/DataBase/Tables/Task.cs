using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace botTask.DataBase.Tables
{
    public class Task
    {
        public int IDTask { get; set; }

        private string NameTask, Discription;
        private DateTime DateCreate, DeadLine;
        private int Priority, Status, IDProject;

        public string nameTask { get { return NameTask; } set { NameTask = value; } }
        public string discription { get { return Discription; } set { Discription = value; } }
        public DateTime dateCreate { get { return DateCreate; } set { DateCreate = value; } }
        public DateTime deadLine { get { return DeadLine; } set { DeadLine = value; } }
        public int priority { get { return Priority; } set { Priority = value; } }
        public int status { get { return Status; } set { Status = value; } }
        public int idProject { get { return IDProject; } set { IDProject = value; } }

        public Task() { }

        public Task(string NameTask, string Discription, DateTime DateCreate, DateTime DeadLine, int Priority, int Status, int IDProject)
        {
            this.NameTask = NameTask;
            this.Discription = Discription;
            this.DateCreate = DateCreate;
            this.DeadLine = DeadLine;
            this.Priority = Priority;
            this.Status = Status;
            this.IDProject = IDProject;
        }
    }

}
