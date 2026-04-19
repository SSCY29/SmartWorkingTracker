using SmartWorkingTracker.App.ViewModel;

namespace SmartWorkingTracker.App.Pages;

public partial class WorkSessionEditorPage : ContentPage
{

    public WorkSessionEditorPage(WorkSessionEditorViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

}