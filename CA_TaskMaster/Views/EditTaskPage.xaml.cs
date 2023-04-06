using CA_TaskMaster.ViewModels;
using CA_TaskMaster.Models;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace CA_TaskMaster.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditTaskPage : ContentPage
    {
        private readonly AddTaskViewModel _parentViewModel;

        public EditTaskPage()
        {
            InitializeComponent();
        }

        public EditTaskPage(MyTask task, AddTaskViewModel parentViewModel) : this()
        {
            BindingContext = task;
            _parentViewModel = parentViewModel;
        }

        private async void OnUpdateClicked(object sender, EventArgs e)
        {
            if (BindingContext is MyTask task)
            {
                await _parentViewModel.ExecuteUpdateTask(task);
                await Navigation.PopAsync();
            }
        }
    }
}
