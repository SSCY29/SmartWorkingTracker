using SmartWorkingTracker.App.ViewModels;

namespace SmartWorkingTracker.App.Pages;

public partial class ReportPage : ContentPage
{
    private readonly ReportViewModel _vm;

    public ReportPage(ReportViewModel vm)
    {
        InitializeComponent();

        _vm = vm;        
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        await LoadData();
    }

    private async void OnReloadClicked(object sender, EventArgs e)
    {
        await LoadData();
    }

    public async Task LoadData()
    {
        await _vm.Load();
        BindingContext = null; // 🔥 forza refresh
        BindingContext = _vm; // 🔥 ricollega
    }
}