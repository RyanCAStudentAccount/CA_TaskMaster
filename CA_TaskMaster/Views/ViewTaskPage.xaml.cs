using CA_TaskMaster.Data;
using CA_TaskMaster.Models;
using CA_TaskMaster.ViewModels;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace CA_TaskMaster.Views
{
    public partial class ViewTaskPage : ContentPage
    {
        public ViewTaskPage(MyTask task)
        {
            InitializeComponent();
            BindingContext = task;
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            if (BindingContext is MyTask task)
            {
                await Navigation.PopAsync();
            }
        }
    }
}