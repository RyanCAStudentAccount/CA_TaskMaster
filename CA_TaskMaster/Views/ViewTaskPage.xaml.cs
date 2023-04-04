using CA_TaskMaster.Data;
using CA_TaskMaster.Models;
using CA_TaskMaster.ViewModels;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace CA_TaskMaster.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewTaskPage : ContentPage
    {
        private readonly TaskDbContext _dbContext;

        public ViewTaskPage(int taskId)
        {
            InitializeComponent();

            //_dbContext = TaskDbContext.Instance;

            MyTask task = _dbContext.Tasks.FirstOrDefault(t => t.TaskId == taskId);

            //TaskNameLabel.Text = task.TaskName;
            //TaskDescriptionLabel.Text = task.TaskDescription;
            //TaskPriorityLabel.Text = task.TaskPriority.ToString();
            //TaskDueDateLabel.Text = task.TaskDueDate.ToString("MM/dd/yyyy");
        }
    }
}