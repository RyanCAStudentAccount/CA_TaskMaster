using CA_TaskMaster.ViewModels;

namespace CA_TaskMaster.Views;

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

}