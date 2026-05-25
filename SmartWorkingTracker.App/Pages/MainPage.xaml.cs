using SmartWorkingTracker.App.ViewModels;
using System.Diagnostics;

namespace SmartWorkingTracker.App.Pages;

public partial class MainPage : ContentPage
{
    private readonly HomeViewModel _vm;

    public MainPage(HomeViewModel viewModel)
    {
        InitializeComponent();
        _vm = viewModel;

        BindingContext = _vm; // ✅ UNA VOLTA SOLA

    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        _ = LoadData();
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
        try
        {
            await _vm.LoadDays();

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Errore caricamento dati: {ex.Message}");
        }
    }

    private async void OnReloadClicked(object sender, EventArgs e)
    {
        await LoadData();
    }
}