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

    public void RemovePage(Type pageType)
    {
        var pageToRemove = Navigation.NavigationStack.FirstOrDefault(p => p.GetType() == pageType);
        if (pageToRemove != null)
        {
            Navigation.RemovePage(pageToRemove);
        }
    }
    private async void OnViewTaskClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddTask());
    }

    private async void OnAddTaskClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CreateTask(BindingContext as AddTaskViewModel));
    }
    private async void OnCompletedTasksClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CompletedTasksPage());
    }
    private async void OnMotBtnClicked(object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new MotivationPage());
    }

    private async void OnAboutBtnClicked(object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new AboutPage());
    }

    private async void OnLogOutClicked(object sender, EventArgs e)
    {
        //await Navigation.PushAsync(new CreateTask());
        await Navigation.PushAsync(new LoginPage());
    }
}