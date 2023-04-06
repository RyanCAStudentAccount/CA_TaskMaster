using CA_TaskMaster.Data;
using CA_TaskMaster.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;


namespace CA_TaskMaster.ViewModels
{
    public class CompletedTasksViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<MyTask> _completedTasks;

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
            LoadCompletedTasks();
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
