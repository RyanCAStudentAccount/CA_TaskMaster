using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CA_TaskMaster.Data;
using CA_TaskMaster.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CA_TaskMaster.ViewModels;

namespace CA_TaskMaster.ViewModels
{
    public class AddTaskViewModel : ObservableObject
    {
        // List to view Tasks
        public IList<MyTask> Tasks { get; set; }

        // Selectable Task
        public MyTask SelectedTask { get; set; }

        // Add Task Command
        public ICommand AddTaskCommand { get; }

        // Update Command
        public ICommand SaveChangesCommand { get; }


        // Delete Command
        public ICommand DeleteTaskCommand { get; }

        public ICommand ViewTaskCommand { get; }
        public ICommand EditTaskCommand { get; }


        public MyTask NewTask { get; set; } = new MyTask();

        private readonly TaskDbContext _dbContext;

        public AddTaskViewModel()
        {
            _dbContext = new TaskDbContext();

            //Tasks = (IList<MyTask>)_dbContext.Tasks.ToList();

            AddTaskCommand = new Command(() =>
            {
                _dbContext.Tasks.Add(NewTask);
                _dbContext.SaveChanges();
            }); // To Add Tasks

            //SaveChangesCommand = new Command(() =>
            //{
            //    _dbContext.Tasks.Update(SelectedTask);
            //    _dbContext.SaveChanges();

            //    Tasks = _dbContext.Tasks.ToList();
            //}); // To Edit Tasks

            //DeleteTaskCommand = new Command(() =>
            //{
            //    _dbContext.Tasks.Remove(SelectedTask);
            //    _dbContext.SaveChanges();

            //    Tasks = _dbContext.Tasks.ToList();
            //}); // To Delete Tasks

            //ViewTaskCommand = new Command<MyTask>((task) =>
            //{
            //    // Navigate to a new page to view the selected task
            //});

            //EditTaskCommand = new Command<MyTask>((task) =>
            //{
            //    // Navigate to a new page to edit the selected task
            //});


        }
    }
}