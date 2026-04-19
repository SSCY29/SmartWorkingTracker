using SmartWorkingTracker.App.ViewModel;

namespace SmartWorkingTracker.App.Pages;

public partial class EditSessionPage : ContentPage
{
	public EditSessionPage(EditSessionViewModel vm)
	{
		InitializeComponent();

        BindingContext = vm;
    }
}