using SmartWorkingTracker.App.ViewModel;

namespace SmartWorkingTracker.App.Pages;

public partial class StatisticsPage : ContentPage
{
	public StatisticsPage(MainViewModel mainVm)
	{
		InitializeComponent();
        BindingContext = new StatisticsViewModel(mainVm);
    }
}