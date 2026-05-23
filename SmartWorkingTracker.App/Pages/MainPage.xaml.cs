using SmartWorkingTracker.App.ViewModels;

namespace SmartWorkingTracker.App.Pages;

public partial class MainPage : ContentPage
{
    private readonly HomeViewModel _vm;

    public MainPage(HomeViewModel viewModel)
    {
        InitializeComponent();
        _vm = viewModel;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        await LoadData();
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

            await LoadData();
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

            await LoadData();
        }
    }

    public async Task LoadData()
    {
        await _vm.LoadDays();
        BindingContext = null; // 🔥 forza refresh
        BindingContext = _vm; // 🔥 ricollega
    }
}