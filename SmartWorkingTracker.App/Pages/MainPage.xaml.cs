using SmartWorkingTracker.App.ViewModels;

namespace SmartWorkingTracker.App.Pages;

public partial class MainPage : ContentPage
{    
    
    public MainPage(HomeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is HomeViewModel vm)
        {
            await vm.LoadDays();
        }
    }

    private async void OnDayTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is DateTime date)
        {
            if (date == DateTime.MinValue)
                return;

            await Shell.Current.GoToAsync($"sessionList?date={date:yyyy-MM-dd}");
        }
    }

    private async void OnPrevMonth(object sender, EventArgs e)
    {
        if (BindingContext is HomeViewModel vm)
        {
            if (vm.SelectedMonth == 1)
            {
                vm.SelectedMonth = 12;
                vm.SelectedYear--;
            }
            else
            {
                vm.SelectedMonth--;
            }

            await vm.LoadDays();
        }
    }

    private async void OnNextMonth(object sender, EventArgs e)
    {
        if (BindingContext is HomeViewModel vm)
        {
            if (vm.SelectedMonth == 12)
            {
                vm.SelectedMonth = 1;
                vm.SelectedYear++;
            }
            else
            {
                vm.SelectedMonth++;
            }

            await vm.LoadDays();
        }
    }

}