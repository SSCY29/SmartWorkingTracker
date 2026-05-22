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

        public int SelectedMonth { get; set; } = DateTime.Now.Month;
        public int SelectedYear { get; set; } = DateTime.Now.Year;

        public HomeViewModel(DatabaseService service, AppDatabase db)
        {
            _service = service;
            _db = db;
        }

        public async Task LoadDays()
        {
            Days.Clear();

            await _db.InitializeAsync();

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

        }
    }

}
