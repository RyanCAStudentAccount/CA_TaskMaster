using CA_TaskMaster.Data;
using CA_TaskMaster.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CA_TaskMaster.ViewModels
{
    public class ExpiredTasksViewModel : INotifyPropertyChanged
    {
        private IList<MyTask> _expiredTasks;
        public IList<MyTask> ExpiredTasks
        {
            get { return _expiredTasks; }
            set
            {
                _expiredTasks = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly TaskDbContext _dbContext;

        public ExpiredTasksViewModel()
        {
            _dbContext = new TaskDbContext();
            LoadExpiredTasks();
        }

        public void LoadExpiredTasks()
        {
            ExpiredTasks = _dbContext.Tasks.AsNoTracking().Where(t => t.TaskDueDate < DateTime.Now).ToList();
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
