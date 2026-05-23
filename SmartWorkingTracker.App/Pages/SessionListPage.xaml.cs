using SmartWorkingTracker.App.ViewModels;
using SmartWorkingTracker.Core.Models;

namespace SmartWorkingTracker.App.Pages;

[QueryProperty(nameof(DateString), "date")]
public partial class SessionListPage : ContentPage
{
    private readonly SessionListViewModel _vm;

    public SessionListPage(SessionListViewModel vm)
    {
        InitializeComponent();

        _vm = vm;
    }

    // Riceve la data dalla Shell
    private DateTime? _pendingDate;    
    public string DateString
    {
        set
        {
            _pendingDate = DateTime.Parse(value);
        }
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (_pendingDate.HasValue)
        {
            await _vm.Load(_pendingDate.Value);

            BindingContext = null; // 🔥 forza refresh
            BindingContext = _vm; // 🔥 ricollega
        }
    }


    // ✅ NUOVA SESSIONE
    private async void OnAddClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"editSession?date={_vm.Date:yyyy-MM-dd}");
    }

    // ✅ ELIMINAZIONE
    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is WorkSession session)
        {
            bool confirm = await DisplayAlertAsync(
                "Conferma",
                "Vuoi eliminare la sessione?",
                "Si",
                "No");

            if (!confirm)
                return;

            await _vm.Delete(session);
        }
    }

    // ✅ MODIFICA
    private async void OnEditClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is WorkSession session)
        {

            await Shell.Current.GoToAsync($"editSession?id={session.Id}");

        }
    }

}