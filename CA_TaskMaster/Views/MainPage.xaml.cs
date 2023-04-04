using CA_TaskMaster.ViewModels;
using CA_TaskMaster.Views;

namespace CA_TaskMaster;

public partial class MainPage : ContentPage
{
    int count = 0;

    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnViewTaskClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddTask());
    }

    private async void OnAddTaskClicked(object sender, EventArgs e)
    {
        //await Navigation.PushAsync(new CreateTask());
        await Navigation.PushAsync(new CreateTask(BindingContext as AddTaskViewModel));
    }
}