using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace CA_TaskMaster.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        private async void OnMenuClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}