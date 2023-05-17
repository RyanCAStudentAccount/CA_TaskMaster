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
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Diagnostics;
using System.Xml.Linq;
using Microsoft.Maui.Controls;
using INavigation = Microsoft.Maui.Controls.INavigation;
using System.Timers;

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
        public CompletedTasksViewModel CompletedTasksViewModel { get; set; }
        public ExpiredTasksViewModel ExpiredTasksViewModel { get; set; }

        // Selectable Task
        public MyTask SelectedTask { get; set; }

        // Add Task Command
        public ICommand AddTaskCommand { get; }

        // Update Command
        public ICommand SaveChangesCommand { get; }


        public ICommand MarkAsDoneCommand { get; private set; }
        // Delete Command
        public ICommand DeleteTaskCommand { get; }

        public ICommand ViewTaskCommand { get; }
        public ICommand EditTaskCommand { get; }

        public ICommand UpdateTaskCommand { get; }
        public MyTask NewTask { get; set; } = new MyTask();

        private readonly TaskDbContext _dbContext;

        public INavigation _navigation;

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

            MinimumDate = DateTime.Now.AddDays(1); // Set the minimum date to the next day
            MaximumDate = DateTime.Now.AddYears(10);

            CompletedTasksViewModel = new CompletedTasksViewModel();
            ExpiredTasksViewModel = new ExpiredTasksViewModel();

            // Start a timer to check for expired tasks every minute 
            //Timer timer = new Timer(60 * 1000);
            //timer.Elapsed += CheckForExpiredTasks;
            //timer.Start();

            AddTaskCommand = new Command(async () =>
            {
                if (!AreFieldsValid())
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Please fill in all fields before saving.", "OK");
                    return;
                }

                try
                {
                    _dbContext.Tasks.Add(NewTask);
                    _dbContext.SaveChanges();
                    NewTask = new MyTask(); // Reset the new task form
                    Tasks = (IList<MyTask>)_dbContext.Tasks.AsNoTracking().ToList(); // Refresh the tasks list


                    await _navigation.PopAsync(); // Pop the AddTaskPage
                    var taskListPage = new AddTask();
                    await _navigation.PushAsync(taskListPage); // Push the TaskListPage back onto the stack

                    // Remove AddTaskPage from the navigation stack
                    var mainMenuPage = _navigation.NavigationStack.FirstOrDefault(p => p.GetType() == typeof(MainPage)) as MainPage;
                    if (mainMenuPage != null)
                    {
                        mainMenuPage.RemovePage(typeof(AddTask));
                    }

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
                    // Detach any existing tracked entity with the same TaskId
                    var existingEntity = _dbContext.Tasks.Local.FirstOrDefault(t => t.TaskId == task.TaskId);
                    if (existingEntity != null)
                    {
                        _dbContext.Entry(existingEntity).State = EntityState.Detached;
                    }

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

            ViewTaskCommand = new Command<MyTask>(async (task) => await ExecuteViewTaskCommand(task));

            EditTaskCommand = new Command<MyTask>(ExecuteEditTaskCommand);

            UpdateTaskCommand = new Command<MyTask>(async (task) => await ExecuteUpdateTask(task));

            MarkAsDoneCommand = new Command<MyTask>(MarkTaskAsDone);
        }

        private async Task ExecuteViewTaskCommand(MyTask task)
        {
            await _navigation.PushAsync(new ViewTaskPage(task));
        }

        //public async Task ExecuteUpdateTask(MyTask task)
        //{
        //    _dbContext.Update(NewTask);
        //    _dbContext.SaveChanges();
        //    Tasks = (IList<MyTask>)_dbContext.Tasks.AsNoTracking().ToList(); // Refresh the tasks list

        //    await _navigation.PopAsync(); // Navigate back to the TaskListPage
        //}

        public async Task ExecuteUpdateTask(MyTask task)
        {
            try
            {
                //if (!AreFieldsValid())
                //{
                //    await Application.Current.MainPage.DisplayAlert("Error", "Please fill in all fields before saving.", "OK");
                //    return;
                //}

                // Ensure TaskPriority has a value before updating
                if (task.TaskPriority == null)
                {
                    // Set a default value or show an error message to the user
                    task.TaskPriority = "Default";
                }

                // Detach any existing tracked entity with the same TaskId
                var existingEntity = _dbContext.Tasks.Local.FirstOrDefault(t => t.TaskId == task.TaskId);
                if (existingEntity != null)
                {
                    _dbContext.Entry(existingEntity).State = EntityState.Detached;
                }

                _dbContext.Update(task); // Update `task`
                _dbContext.SaveChanges();
                Tasks = (IList<MyTask>)_dbContext.Tasks.AsNoTracking().ToList(); // Refresh the tasks list

                await _navigation.PopAsync(); // Navigate back to the TaskListPage
            }
            catch (DbUpdateException ex)
            {
                // Log the exception details
                Debug.WriteLine($"Error: {ex.Message}");
                Debug.WriteLine($"Inner Exception: {ex.InnerException?.Message}");

            }
        }

        private async void ExecuteEditTaskCommand(MyTask task)
        {
            await _navigation.PushAsync(new EditTaskPage(task, this));
        }

        public void UpdateTask(MyTask updatedTask)
        {
            var index = Tasks.IndexOf(Tasks.FirstOrDefault(t => t.TaskName == updatedTask.TaskName));
            if (index != -1)
            {
                Tasks[index] = updatedTask;
            }
        }

        public void RefreshTasks()
        {
            Tasks = _dbContext.Tasks.AsNoTracking().Where(t => !t.TaskCompletionStatus).ToList();
        }

        private async void CheckForExpiredTasks(object sender, ElapsedEventArgs e)
        {
            var expiredTasks = Tasks.Where(t => t.TaskDueDate < DateTime.Now).ToList();

            if (expiredTasks.Any())
            {
                foreach (var task in expiredTasks)
                {
                    // Update the task in the database (if needed)
                    // Remove the task from the current task list
                    Tasks.Remove(task);

                    // Add the task to the expired tasks list
                    ExpiredTasksViewModel.ExpiredTasks.Add(task);
                }

                // Refresh the task lists
                RefreshTasks();
                ExpiredTasksViewModel.LoadExpiredTasks();
            }
        }

        private async void MarkTaskAsDone(MyTask task)
        {
            if (task != null)
            {
                // Show a confirmation popup
                bool answer = await Application.Current.MainPage.DisplayAlert("Mark as Done", "Are you sure you want to mark this task as completed?", "Yes", "No");

                // If the user confirms, update the task's completion status
                if (answer)
                {
                    task.TaskCompletionStatus = true;

                    using (var db = new TaskDbContext())
                    {
                        // Save the changes to the database
                        db.Tasks.Update(task);
                        db.SaveChanges();
                    }

                    // Remove the task from the active tasks list
                    Tasks.Remove(task);

                    // Add the task to the completed tasks list in CompletedTasksViewModel
                    CompletedTasksViewModel.CompletedTasks.Add(task);

                    // Refresh the task list
                    RefreshTasks();
                }
            }
        }

        public bool AreFieldsValid()
        {
            if (string.IsNullOrWhiteSpace(NewTask.TaskName) ||
                string.IsNullOrWhiteSpace(NewTask.TaskDescription) ||
                string.IsNullOrWhiteSpace(NewTask.TaskPriority) ||
                NewTask.TaskDueDate == DateTime.MinValue)
            {
                return false;
            }

            return true;
        }


        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            (AddTaskCommand as Command)?.ChangeCanExecute();
        }

        public DateTime MinimumDate { get; }
        public DateTime MaximumDate { get; }
    }
}