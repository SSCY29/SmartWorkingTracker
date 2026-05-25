using SmartWorkingTracker.Data.Database;
using SmartWorkingTracker.Data.Services;
using System.Collections.ObjectModel;

namespace SmartWorkingTracker.App.ViewModels
{

    public class HomeViewModel : BaseViewModel
    {
        private readonly DatabaseService _service;
        private readonly AppDatabase _db;

        public ObservableCollection<DayViewModel> Days { get; set; } = new();
        public bool ContractMissing { get; set; }

        public int SelectedMonth { get; set; } = DateTime.Now.Month;
        public int SelectedYear { get; set; } = DateTime.Now.Year;

        public List<int> Years { get; set; } =
            Enumerable.Range(2020, DateTime.Now.Year - 2019).ToList();

        public string CurrentPeriod => $"{SelectedYear} - {GetMonth()}";



        public List<string> WeekDays { get; } = new()
        {
            "L", "M", "M", "G", "V", "S", "D"
        };



        public HomeViewModel(DatabaseService service, AppDatabase db)
        {
            _service = service;
            _db = db;
        }

        public async Task LoadDays()
        {
            IsLoading = true;

            try
            {


                if (SelectedYear != DateTime.Now.Year)
                {
                    SelectedMonth = 1;
                }

                Days.Clear();

                await _db.InitializeAsync();

#if DEBUG
                await _service.PopulateDatabase();
#endif

                var contract = await _service.GetContractByYear(SelectedYear);

                ContractMissing = contract == null;

                var sessions = await _service.GetSessionsByMonth(SelectedYear, SelectedMonth);

                var firstDayOfMonth = new DateTime(SelectedYear, SelectedMonth, 1);

                int startOffset = (int)firstDayOfMonth.DayOfWeek;

                // rendiamo Monday = 0
                startOffset = startOffset == 0 ? 6 : startOffset - 1;

                // slot vuoti iniziali
                for (int i = 0; i < startOffset; i++)
                {
                    Days.Add(new DayViewModel
                    {
                        Date = DateTime.MinValue // placeholder
                    });
                }

                int daysInMonth = DateTime.DaysInMonth(SelectedYear, SelectedMonth);

                for (int i = 1; i <= daysInMonth; i++)
                {
                    var date = new DateTime(SelectedYear, SelectedMonth, i);

                    var daySessions = sessions
                        .Where(s => s.StartDate.Date == date.Date)
                        .ToList();

                    Days.Add(new DayViewModel
                    {
                        Date = date,
                        Sessions = daySessions
                    });
                }

                await Task.Delay(1000);
            }
            catch (Exception)
            {

            }
            finally
            {

                IsLoading = false;



                OnPropertyChanged(nameof(ContractMissing));
                OnPropertyChanged(nameof(SelectedMonth));
                OnPropertyChanged(nameof(SelectedYear));
                OnPropertyChanged(nameof(CurrentPeriod));
            }

        }

        private string GetMonth()
        {
            switch (SelectedMonth)
            {
                case 1: return "Gennaio";
                case 2: return "Febbraio";
                case 3: return "Marzo";
                case 4: return "Aprile";
                case 5: return "Maggio";
                case 6: return "Giugno";
                case 7: return "Luglio";
                case 8: return "Agosto";
                case 9: return "Settembre";
                case 10: return "Ottobre";
                case 11: return "Novembre";
                case 12: return "Dicembre";
                default: return "";
            }
        }
    }

}
