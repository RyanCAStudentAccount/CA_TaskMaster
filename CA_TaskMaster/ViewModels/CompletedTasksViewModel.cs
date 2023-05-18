using CA_TaskMaster.Data;
using CA_TaskMaster.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;


namespace CA_TaskMaster.ViewModels
{
    public class CompletedTasksViewModel : INotifyPropertyChanged
    {
        // A collection of completed tasks
        private ObservableCollection<MyTask> _completedTasks;

        // A reference to the database context
        private readonly TaskDbContext _dbContext;

        // A command to delete a task
        public ICommand DeleteTaskCommand { get; }

        // Property to get or set the collection of completed tasks
        public ObservableCollection<MyTask> CompletedTasks
        {
            get => _completedTasks;
            set
            {
                _completedTasks = value;

                // Notify any subscribers that the completed tasks have changed
                OnPropertyChanged(nameof(CompletedTasks));
            }
        }

        // Constructor
        public CompletedTasksViewModel()
        {
            // Instantiate the database context and ensure the database is created
            _dbContext = new TaskDbContext();
            _dbContext.Database.EnsureCreated();

            // Load the completed tasks
            LoadCompletedTasks();

            // Assign the DeleteTask method to the DeleteTaskCommand
            DeleteTaskCommand = new Command<MyTask>(async (task) => await DeleteTask(task));
        }

        // Method to delete a task
        private async Task DeleteTask(MyTask task)
        {
            // Ask the user to confirm deletion
            bool answer = await Application.Current.MainPage.DisplayAlert("Delete Task", "Are you sure you want to delete this task?", "Yes", "No");

            if (answer)
            {
                // Find the task in the database
                MyTask taskToDelete = await _dbContext.Tasks.AsNoTracking().FirstOrDefaultAsync(t => t.TaskId == task.TaskId);

                if (taskToDelete != null)
                {
                    // Remove the task from the database
                    _dbContext.Tasks.Remove(taskToDelete);
                    _dbContext.SaveChanges();

                    // Remove the task from the collection of completed tasks
                    CompletedTasks.Remove(task);
                }
            }
        }

        // Method to load completed tasks from the database
        private void LoadCompletedTasks()
        {
            using (var db = new TaskDbContext())
            {
                // Get all tasks that are marked as completed
                var completedTasks = db.Tasks.Where(t => t.TaskCompletionStatus).ToList();

                // Assign the completed tasks to the CompletedTasks collection
                CompletedTasks = new ObservableCollection<MyTask>(completedTasks);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // Method to invoke the PropertyChanged event
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
