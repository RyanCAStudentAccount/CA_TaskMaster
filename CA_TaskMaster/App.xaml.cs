using CA_TaskMaster.Data;
using CA_TaskMaster.ViewModels;

namespace CA_TaskMaster;

public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;
    public App()
	{
		InitializeComponent();

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddDbContext<TaskDbContext>();
        serviceCollection.AddTransient<AddTaskViewModel>();
        _serviceProvider = serviceCollection.BuildServiceProvider();

        MainPage = new AppShell();
	}
}
