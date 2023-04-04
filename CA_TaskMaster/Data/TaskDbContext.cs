using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CA_TaskMaster.Models;

namespace CA_TaskMaster.Data
{
    public class TaskDbContext : DbContext
    {
        public DbSet<MyTask> Tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlite("Data Source=tasks.db");
            optionsBuilder.UseSqlite($"Data Source={Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "tasks.db")}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MyTask>().HasKey(t => t.TaskId);
        }

        public void InitializeDatabase()
        {
            Database.EnsureCreated();
        }

        public void Migrate()
        {
            Database.Migrate();
        }

    }
}
