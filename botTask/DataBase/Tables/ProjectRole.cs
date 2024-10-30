using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace botTask.DataBase.Tables
{
    public class ProjectRole
    {
        public int IDProjectRole { get; set; }
        public int IDUser { get; set; }
        public int IDProject { get; set; }

        private string Role;

        public string role { get { return Role; } set { Role = value; } }

        public ProjectRole() { }

        public ProjectRole(string Role)
        {
            this.Role = Role;
        }
    }

}
