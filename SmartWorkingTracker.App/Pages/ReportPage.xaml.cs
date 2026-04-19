using SmartWorkingTracker.App.ViewModel;

namespace SmartWorkingTracker.App.Pages;

public partial class ReportPage : ContentPage
{
    public ReportPage(ReportViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

}