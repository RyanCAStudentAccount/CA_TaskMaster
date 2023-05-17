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
        private ObservableCollection<MyTask> _completedTasks;
        private readonly TaskDbContext _dbContext;
        public ICommand DeleteTaskCommand { get; }

        public ObservableCollection<MyTask> CompletedTasks
        {
            get => _completedTasks;
            set
            {
                _completedTasks = value;
                OnPropertyChanged(nameof(CompletedTasks));
            }
        }

        public CompletedTasksViewModel()
        {
            _dbContext = new TaskDbContext();
            _dbContext.Database.EnsureCreated();

            LoadCompletedTasks();
            DeleteTaskCommand = new Command<MyTask>(async (task) => await DeleteTask(task));
        }

        private async Task DeleteTask(MyTask task)
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
                    CompletedTasks.Remove(task);
                }
            }
        }

        private void LoadCompletedTasks()
        {
            using (var db = new TaskDbContext())
            {
                var completedTasks = db.Tasks.Where(t => t.TaskCompletionStatus).ToList();
                CompletedTasks = new ObservableCollection<MyTask>(completedTasks);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
