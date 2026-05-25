using SmartWorkingTracker.App.ViewModels;

namespace SmartWorkingTracker.App.Pages
{
    [QueryProperty(nameof(DateString), "date")]

    [QueryProperty(nameof(SessionId), "id")]   

    public partial class EditSessionPage : ContentPage
    {

        private readonly EditSessionViewModel _vm;
                
        public string DateString
        {
            set
            {
                _vm.SetDate(DateTime.Parse(value));
                _vm.SetId(null); // ✅ Nuova sessione, reset id
            }
        }
        public string SessionId
        {
            set
            {
                var id = int.Parse(value);
                _vm.SetId(id);
                _vm.SetDate(null); // ✅ Sessione esistente, non modificare la data
            }
        }        

        public EditSessionPage(EditSessionViewModel vm)
        {
            InitializeComponent();

            _vm = vm;

            BindingContext = _vm; // ✅ UNA VOLTA SOLA
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);

            if(_vm.Id.HasValue)
            {
                await _vm.LoadSession();
            }


        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var ok = await _vm.Save();

            if (!ok)
            {
                await DisplayAlertAsync("Errore", "Sessione non valida", "OK");
                return;
            }

            await Shell.Current.GoToAsync($"//home");
        }

    }
}