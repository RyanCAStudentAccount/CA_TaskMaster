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
using CA_TaskMaster.Views;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using Microsoft.Maui.Controls;
using System.Runtime.CompilerServices;

namespace CA_TaskMaster.ViewModels
{
    public class AddTaskViewModel : INotifyPropertyChanged
    {
        // List to view Tasks
        //public IList<MyTask> Tasks { get; set; }


        private IList<MyTask> _tasks;
        public IList<MyTask> Tasks
        {
            get { return _tasks; }
            set
            {
                _tasks = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

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

        private readonly INavigation _navigation;

        public AddTaskViewModel() : this(null)
        {
        }

        public AddTaskViewModel(INavigation navigation)
        {
            _navigation = navigation;

            _dbContext = new TaskDbContext();
            _dbContext.Database.EnsureCreated();

            Tasks = (IList<MyTask>)_dbContext.Tasks.AsNoTracking().ToList();
            NewTask = new MyTask();

            MinimumDate = DateTime.Now;
            MaximumDate = DateTime.Now.AddYears(10);

            AddTaskCommand = new Command(async () =>
            {
                try
                {
                    _dbContext.Tasks.Add(NewTask);
                    _dbContext.SaveChanges();
                    NewTask = new MyTask(); // Reset the new task form
                    Tasks = (IList<MyTask>)_dbContext.Tasks.AsNoTracking().ToList(); // Refresh the tasks list

                    
                    // Navigate to the Add Task page
                    await _navigation.PushAsync(new AddTask());
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine("DbUpdateException: " + ex.InnerException?.Message);
                }
            }); // To Add Tasks

            DeleteTaskCommand = new Command<MyTask>(async (task) =>
            {
                bool answer = await Application.Current.MainPage.DisplayAlert("Delete Task", "Are you sure you want to delete this task?", "Yes", "No");
                if (answer)
                {
                    // Find the task in the database
                    MyTask taskToDelete = await _dbContext.Tasks.AsNoTracking().FirstOrDefaultAsync(t => t.TaskId == task.TaskId);

                    if (taskToDelete != null)
                    {
                        _dbContext.Tasks.Remove(taskToDelete);
                        _dbContext.SaveChanges();
                        Tasks = (IList<MyTask>)_dbContext.Tasks.AsNoTracking().ToList();
                    }
                }
            }); // To Delete Tasks

            //ViewTaskCommand = new Command<MyTask>((task) =>
            //{
            //    // Navigate to a new page to view the selected task
            //});

            //EditTaskCommand = new Command<MyTask>((task) =>
            //{
            //    // Navigate to a new page to edit the selected task
            //});
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public DateTime MinimumDate { get; }
        public DateTime MaximumDate { get; }
    }
}