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
        // Represents the list of tasks
        private IList<MyTask> _tasks;
        public IList<MyTask> Tasks
        {
            get { return _tasks; }
            set
            {
                _tasks = value;
                // Notify UI of property change
                OnPropertyChanged();
            }
        }

        // Event handler for property changes
        public event PropertyChangedEventHandler PropertyChanged;

        // ViewModel for completed tasks
        public CompletedTasksViewModel CompletedTasksViewModel { get; set; }

        // ViewModel for expired tasks
        public ExpiredTasksViewModel ExpiredTasksViewModel { get; set; }

        // Properties to represent the minimum and maximum dates for tasks
        public DateTime MinimumDate { get; }
        public DateTime MaximumDate { get; }

        // The currently selected task
        public MyTask SelectedTask { get; set; }

        // Command for adding a task
        public ICommand AddTaskCommand { get; }

        // Command for saving changes
        public ICommand SaveChangesCommand { get; }

        // Command for marking a task as done
        public ICommand MarkAsDoneCommand { get; private set; }

        // Command for deleting a task
        public ICommand DeleteTaskCommand { get; }

        // Command for viewing a task
        public ICommand ViewTaskCommand { get; }

        // Command for editing a task
        public ICommand EditTaskCommand { get; }

        // Command for updating a task
        public ICommand UpdateTaskCommand { get; }

        // The new task to be added
        public MyTask NewTask { get; set; } = new MyTask();

        // Instance of the DbContext for database operations
        private readonly TaskDbContext _dbContext;

        // Navigation object for page navigation
        public INavigation _navigation;

        // Default constructor
        public AddTaskViewModel() : this(null)
        {
        }

        public AddTaskViewModel(INavigation navigation)
        {
            _navigation = navigation;

            // Create a new instance of the TaskDbContext
            _dbContext = new TaskDbContext();
            // Ensure that the database for this context exists
            _dbContext.Database.EnsureCreated();

            // Retrieve all tasks from the database
            Tasks = (IList<MyTask>)_dbContext.Tasks.AsNoTracking().ToList();
            // Initialize a new task
            NewTask = new MyTask();

            // Set the minimum date to the next day
            MinimumDate = DateTime.Now.AddDays(1);
            // Set the maximum date to 10 years from now
            MaximumDate = DateTime.Now.AddYears(10);

            // Initialize the CompletedTasksViewModel
            CompletedTasksViewModel = new CompletedTasksViewModel();

            // Initialize the ExpiredTasksViewModel
            ExpiredTasksViewModel = new ExpiredTasksViewModel();

            // Initialize the ViewTaskCommand with a new Command
            ViewTaskCommand = new Command<MyTask>(async (task) => await ExecuteViewTaskCommand(task));

            // Initialize the EditTaskCommand with a new Command
            EditTaskCommand = new Command<MyTask>(ExecuteEditTaskCommand);

            // Initialize the UpdateTaskCommand with a new Command
            UpdateTaskCommand = new Command<MyTask>(async (task) => await ExecuteUpdateTask(task));

            // Initialize the MarkAsDoneCommand with a new Command
            MarkAsDoneCommand = new Command<MyTask>(MarkTaskAsDone);

            // Initialize the AddTaskCommand with a new Command
            AddTaskCommand = new Command(async () =>
            {
                // Check if all fields are valid
                if (!AreFieldsValid())
                {
                    // Show an alert if fields are not valid
                    await Application.Current.MainPage.DisplayAlert("Error", "Please fill in all fields before saving.", "OK");
                    return;
                }

                try
                {
                    // Add the new task to the database
                    _dbContext.Tasks.Add(NewTask);
                    // Save changes in the database
                    _dbContext.SaveChanges();
                    // Reset the new task form
                    NewTask = new MyTask();
                    // Refresh the tasks list
                    Tasks = (IList<MyTask>)_dbContext.Tasks.AsNoTracking().ToList();

                    // Navigate back to the previous page
                    await _navigation.PopAsync();
                    // Create a new instance of the AddTask page and push it onto the navigation stack
                    var taskListPage = new AddTask();
                    await _navigation.PushAsync(taskListPage);

                    // Remove the AddTaskPage from the navigation stack
                    var mainMenuPage = _navigation.NavigationStack.FirstOrDefault(p => p.GetType() == typeof(MainPage)) as MainPage;
                    if (mainMenuPage != null)
                    {
                        mainMenuPage.RemovePage(typeof(AddTask));
                    }
                }
                catch (DbUpdateException ex)
                {
                    // Log any DbUpdateException
                    Console.WriteLine("DbUpdateException: " + ex.InnerException?.Message);
                }
            }); // To Add Tasks

            // Initialize the DeleteTaskCommand with a new Command
            DeleteTaskCommand = new Command<MyTask>(async (task) =>
            {
                // Ask the user if they really want to delete the task
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
                        // Remove the task from the database
                        _dbContext.Tasks.Remove(taskToDelete);
                        // Save changes in the database
                        _dbContext.SaveChanges();
                        // Refresh the tasks list
                        Tasks = (IList<MyTask>)_dbContext.Tasks.AsNoTracking().ToList();
                    }
                }
            });
        }

        // Define the method to execute the ViewTaskCommand
        private async Task ExecuteViewTaskCommand(MyTask task)
        {
            await _navigation.PushAsync(new ViewTaskPage(task));
        }


        public async Task ExecuteUpdateTask(MyTask task)
        {
            try
            {
                // Ensure that TaskPriority is not null before updating
                if (task.TaskPriority == null)
                {
                    // Either set a default value or show an error message to the user
                    task.TaskPriority = "Default";
                }

                // Detach any existing tracked entity with the same TaskId
                var existingEntity = _dbContext.Tasks.Local.FirstOrDefault(t => t.TaskId == task.TaskId);
                if (existingEntity != null)
                {
                    _dbContext.Entry(existingEntity).State = EntityState.Detached;
                }

                _dbContext.Update(task); // Update the task in the database
                _dbContext.SaveChanges(); // Save the changes
                Tasks = (IList<MyTask>)_dbContext.Tasks.AsNoTracking().ToList(); // Refresh the list of tasks

                await _navigation.PopAsync(); // Navigate back to the list of tasks
            }
            catch (DbUpdateException ex)
            {
                // Log the details of the exception
                Debug.WriteLine($"Error: {ex.Message}");
                Debug.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
            }
        }

        // Method to execute when EditTaskCommand is called
        private async void ExecuteEditTaskCommand(MyTask task)
        {
            // Navigate to the EditTaskPage with the selected task
            await _navigation.PushAsync(new EditTaskPage(task, this));
        }

        // Method to update a task in the tasks list
        public void UpdateTask(MyTask updatedTask)
        {
            // Find the index of the task to be updated in the tasks list
            var index = Tasks.IndexOf(Tasks.FirstOrDefault(t => t.TaskName == updatedTask.TaskName));
            // If the task is found, update it
            if (index != -1)
            {
                Tasks[index] = updatedTask;
            }
        }

        // Method to refresh the tasks list from the database
        public void RefreshTasks()
        {
            // Get all tasks from the database where TaskCompletionStatus is false (i.e., tasks not yet completed)
            Tasks = _dbContext.Tasks.AsNoTracking().Where(t => !t.TaskCompletionStatus).ToList();
        }

        // Method to check for tasks that have expired
        private async void CheckForExpiredTasks(object sender, ElapsedEventArgs e)
        {
            // Get tasks where the due date is earlier than now
            var expiredTasks = Tasks.Where(t => t.TaskDueDate < DateTime.Now).ToList();

            // If there are expired tasks
            if (expiredTasks.Any())
            {
                foreach (var task in expiredTasks)
                {
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

        // Method to mark a task as done
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

        // Method to check if the fields of a new task are valid
        public bool AreFieldsValid()
        {
            // Return false if any of the fields are null, empty, or whitespace, or if the due date is the minimum value
            if (string.IsNullOrWhiteSpace(NewTask.TaskName) ||
                string.IsNullOrWhiteSpace(NewTask.TaskDescription) ||
                string.IsNullOrWhiteSpace(NewTask.TaskPriority) ||
                NewTask.TaskDueDate == DateTime.MinValue)
            {
                return false;
            }

            // Otherwise, return true
            return true;
        }

        // Method to raise the PropertyChanged event
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            // Notify the AddTaskCommand that it should check if it can execute
            (AddTaskCommand as Command)?.ChangeCanExecute();
        }
    }
}