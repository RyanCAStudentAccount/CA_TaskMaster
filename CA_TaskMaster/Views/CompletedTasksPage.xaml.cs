using Microsoft.Maui.Controls;
using CA_TaskMaster.ViewModels;
using Microsoft.Maui.Controls.Xaml;

namespace CA_TaskMaster.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)] 
    public partial class CompletedTasksPage : ContentPage
    {
        public CompletedTasksPage()
        {
            InitializeComponent();
        }

        private async void OnMenuClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
