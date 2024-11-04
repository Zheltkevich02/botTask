using botTask.DataBase.Tables;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = botTask.DataBase.Tables.Task;

namespace botTask.DataBase
{
    public class ApplicationConnection : DbContext
    {
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Notify> Notifies { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectRole> ProjectRoles { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<SettingsUser> SettingsUsers { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<TaskUser> TaskUsers { get; set; }
        public DbSet<User> Users { get; set; }
        public ApplicationConnection() : base("DefaultConnection") { }
    }
}
