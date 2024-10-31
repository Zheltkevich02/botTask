using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace botTask.DataBase.Tables
{
    public class Project
    {
        [Key]
        public int IDProject { get; set; }

        private string NameProject, Discription;
        private DateTime DateCreate;
        private int Status;

        public string nameProject { get { return NameProject; } set { NameProject = value; } }
        public string discription { get { return Discription; } set { Discription = value; } }
        public DateTime dateCreate { get { return DateCreate; } set { DateCreate = value; } }
        public int status { get { return Status; } set { Status = value; } }

        public Project() { }

        public Project(string NameProject, string Discription, DateTime DateCreate, int Status)
        {
            this.NameProject = NameProject;
            this.Discription = Discription;
            this.DateCreate = DateCreate;
            this.Status = Status;
        }
    }

}
