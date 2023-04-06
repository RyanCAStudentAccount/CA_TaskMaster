using CA_TaskMaster.Data;
using CA_TaskMaster.ViewModels;
using CA_TaskMaster.Views;

namespace CA_TaskMaster;

public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;
    public App()
	{
		InitializeComponent();

        //MainPage = new NavigationPage(new MainPage());

        MainPage = new NavigationPage(new LoginPage());

        //var serviceCollection = new ServiceCollection();
        //serviceCollection.AddDbContext<TaskDbContext>();
        //serviceCollection.AddTransient<AddTaskViewModel>();
        //_serviceProvider = serviceCollection.BuildServiceProvider();

        //MainPage = new AppShell();
    }
}
