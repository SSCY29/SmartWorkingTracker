using SmartWorkingTracker.App.ViewModels;

namespace SmartWorkingTracker.App.Pages;


[QueryProperty(nameof(ContractId), "id")]
public partial class EditContractPage : ContentPage
{
    public string ContractId
    {
        set
        {
            var id = int.Parse(value);
            _vm.SetId(id);
        }
    }

    private readonly EditContractViewModel _vm;

    public EditContractPage(EditContractViewModel vm)
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

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        var ok = await _vm.Save();

        if (!ok)
        {
            await DisplayAlertAsync("Errore", "Esiste già un contratto per l'anno selezionato", "OK");
            return;
        }

        await Shell.Current.GoToAsync($"//contracts");
    }
}
