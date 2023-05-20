using CA_TaskMaster.Models;
using CA_TaskMaster.ViewModels;

namespace CA_TaskMaster.Views
{
    public partial class AddTask : ContentPage
    {
        public AddTask()
        {
            InitializeComponent();
            BindingContext = new AddTaskViewModel(Navigation);
        }

        private async void OnCreateClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateTask(BindingContext as AddTaskViewModel));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = BindingContext as AddTaskViewModel;
            if (viewModel != null)
            {
                viewModel.RefreshTasks();
            }
        }

        private async void OnEditClicked(object sender, EventArgs e)
        {
            MyTask selectedTask = (MyTask)((Button)sender).BindingContext;
            await Navigation.PushAsync(new EditTaskPage(selectedTask, BindingContext as AddTaskViewModel));
        }

        private async void OnMenuClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }
    }
}
