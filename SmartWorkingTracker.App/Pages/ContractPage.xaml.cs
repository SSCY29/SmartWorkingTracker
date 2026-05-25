using SmartWorkingTracker.App.ViewModels;
using SmartWorkingTracker.Core.Models;

namespace SmartWorkingTracker.App.Pages;

public partial class ContractPage : ContentPage
{

    private readonly ContractViewModel _vm;

    public ContractPage(ContractViewModel vm)
    {
        InitializeComponent();
        _vm = vm;

        BindingContext = _vm; // ✅ UNA VOLTA SOLA
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        await _vm.Load();
    }

    // ✅ Aggiunta contratto (veloce MVP)
    private async void OnAddClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("editContract");
    }
    private async void OnEditClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is Contract contract)
        {
            await Shell.Current.GoToAsync($"editContract?id={contract.Id}");
        }
    }


    // ✅ Delete
    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is Contract contract)
        {
            bool confirm = await DisplayAlertAsync(
                "Conferma",
                $"Eliminare contratto {contract.Year}?",
                "Si",
                "No");

            if (!confirm)
                return;

            await _vm.Delete(contract);
        }
    }

}