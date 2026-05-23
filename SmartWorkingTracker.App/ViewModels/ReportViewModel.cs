using SmartWorkingTracker.Core.Enums;
using SmartWorkingTracker.Data.Services;

namespace SmartWorkingTracker.App.ViewModels
{
    public class ReportViewModel : BaseViewModel
    {

        private readonly DatabaseService _service;

        public int SelectedMonth { get; set; } = DateTime.Now.Month;
        public int SelectedYear { get; set; } = DateTime.Now.Year;

        public double SmartHours { get; set; }
        public double MonthlyLimit { get; set; }
        public double WeeklyLimit { get; set; }
        public double YearlyLimit { get; set; }

        public double ForecastYearly { get; set; }

        public string StatusMonthly =>
            SmartHours > MonthlyLimit ? "Mensile SUPERATO ❌" : "Mensile OK ✅";

        public string StatusYearly =>
            ForecastYearly > YearlyLimit ? "Previsto sforamento annuale ❌" : "Annuale OK ✅";

        public bool ContractMissing { get; set; }

        public ReportViewModel(DatabaseService service)
        {
            _service = service;
        }

        public async Task Load()
        {
            var sessions = await _service.GetSessionsByMonth(SelectedYear, SelectedMonth);

            SmartHours = sessions
                .Where(s => s.Type == SessionType.SmartWorking)
                .Sum(s => (s.EndDate - s.StartDate).TotalHours);

            var contract = await _service.GetContractByYear(SelectedYear);

            if (contract == null)
            {
                ContractMissing = true;
                OnPropertyChanged(nameof(ContractMissing));
                return;
            }

            ContractMissing = false;

            MonthlyLimit = contract.MonthlyLimitHours;
            WeeklyLimit = contract.WeeklyLimitHours;
            YearlyLimit = contract.YearlyLimitHours;

            // ✅ calcolo settimane nel mese
            int weeks = GetWeeksInMonth(SelectedYear, SelectedMonth);

            double weeklyExpected = WeeklyLimit * weeks;

            // ✅ previsione annuale
            double monthsCompleted = SelectedMonth;
            ForecastYearly = monthsCompleted > 0
                ? (SmartHours / monthsCompleted) * 12
                : 0;

            OnPropertyChanged(nameof(SmartHours));
            OnPropertyChanged(nameof(MonthlyLimit));
            OnPropertyChanged(nameof(WeeklyLimit));
            OnPropertyChanged(nameof(YearlyLimit));
            OnPropertyChanged(nameof(ForecastYearly));
            OnPropertyChanged(nameof(StatusMonthly));
            OnPropertyChanged(nameof(StatusYearly));
        }

        private int GetWeeksInMonth(int year, int month)
        {
            var first = new DateTime(year, month, 1);
            var last = first.AddMonths(1).AddDays(-1);

            return (int)Math.Ceiling((last.Day + (int)first.DayOfWeek) / 7.0);
        }

    }
}
