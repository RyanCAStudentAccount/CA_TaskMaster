using CA_TaskMaster.ViewModels;

namespace CA_TaskMaster.Views
{
    public partial class EditTaskPage : ContentPage
    {
        public EditTaskPage(AddTaskViewModel parentViewModel)
        {
            InitializeComponent();
            BindingContext = parentViewModel;
        }
    }
}
