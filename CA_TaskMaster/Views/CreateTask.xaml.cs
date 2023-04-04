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
    }
}