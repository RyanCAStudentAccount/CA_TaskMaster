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
        // Define the database set that will hold the tasks.
        public DbSet<MyTask> Tasks { get; set; }


        // This method is called to configure the database to use a SQLite database.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Set the database to use SQLite. The database file is located in the local application data folder.
            optionsBuilder.UseSqlite($"Data Source={Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "tasks.db")}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define the primary key for the MyTask entity.
            modelBuilder.Entity<MyTask>().HasKey(t => t.TaskId);
        }

        // Ensure the database for the context exists
        public void InitializeDatabase()
        {
            Database.EnsureCreated();
        }

        // Will create the database if it does not already exist.
        public void Migrate()
        {
            Database.Migrate();
        }

    }
}
