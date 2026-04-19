using SmartWorkingTracker.App.ViewModel;

namespace SmartWorkingTracker.App.Pages;

public partial class MainPage : ContentPage
{

    
    
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is MainViewModel vm)
        {
            vm.LoadCommand.Execute(null);
        }
    }


    private async void OnCalendarSelectionChanged(
        object sender,
        SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is CalendarDayViewModel day)
        {
            await Shell.Current.GoToAsync(
                $"{nameof(DayDetailsPage)}?date={day.Date:yyyy-MM-dd}");

            ((CollectionView)sender).SelectedItem = null;
        }
    }


}