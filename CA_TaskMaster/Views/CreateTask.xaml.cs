using CA_TaskMaster.ViewModels;
using Microsoft.Maui.Controls;

namespace CA_TaskMaster.Views
{
    public partial class CreateTask : ContentPage
    {
        public CreateTask(AddTaskViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        private async void OnMenuClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }
    }
}