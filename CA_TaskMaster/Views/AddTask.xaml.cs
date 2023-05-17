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

        //void OnActionSelected(object sender, EventArgs e)
        //{
        //    var picker = (Picker)sender;
        //    int selectedIndex = picker.SelectedIndex;

        //    if (selectedIndex != -1)
        //    {
        //        MyTask selectedTask = (MyTask)picker.BindingContext;
        //        var action = (string)picker.ItemsSource[selectedIndex];
        //        var viewModel = BindingContext as AddTaskViewModel;

        //        switch (action)
        //        {
        //            case "Done":
        //                viewModel.MarkAsDoneCommand.Execute(selectedTask);
        //                break;
        //            case "View":
        //                viewModel.ViewTaskCommand.Execute(selectedTask);
        //                break;
        //            case "Edit":
        //                OnEditClicked(sender, e);
        //                break;
        //            case "Delete":
        //                viewModel.DeleteTaskCommand.Execute(selectedTask);
        //                break;
        //        }
        //    }
        //}
    }
}
