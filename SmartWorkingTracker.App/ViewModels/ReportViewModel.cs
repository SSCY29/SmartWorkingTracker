using SmartWorkingTracker.Core.Enums;
using SmartWorkingTracker.Data.Services;

namespace SmartWorkingTracker.App.ViewModels
{
    public class ReportViewModel : BaseViewModel
    {
        private readonly DatabaseService _service;
        
        public int SelectedYear { get; set; } = DateTime.Now.Year;


        public List<int> Years { get; set; } =
            Enumerable.Range(2020, DateTime.Now.Year - 2019).ToList();


        public double YearlyLimit { get; set; }
        public double AvgYearly { get; set; }
        public double ForecastYearly { get; set; }
        public string StatusYearly => ForecastYearly > YearlyLimit ? "Previsto sforamento annuale ❌" : "Annuale OK ✅";
        public bool ContractMissing { get; set; }

        public double JanuaryHours { get; private set; } = 0;
        public double FebruaryHours { get; private set; } = 0;
        public double MarchHours { get; private set; } = 0;
        public double AprilHours { get; private set; } = 0;
        public double MayHours { get; private set; } = 0;
        public double JuneHours { get; private set; } = 0;
        public double JulyHours { get; private set; } = 0;
        public double AugustHours { get; private set; } = 0;
        public double SeptemberHours { get; private set; } = 0;
        public double OctoberHours { get; private set; } = 0;
        public double NovemberHours { get; private set; } = 0;
        public double DecemberHours { get; private set; } = 0;

        public ReportViewModel(DatabaseService service)
        {
            _service = service;
        }

        public async Task Load()
        {
            IsLoading = true;

            try
            {
                JanuaryHours = 0;
                FebruaryHours = 0;
                MarchHours = 0;
                AprilHours = 0;
                MayHours = 0;
                JuneHours = 0;
                JulyHours = 0;
                AugustHours = 0;
                SeptemberHours = 0;
                OctoberHours = 0;
                NovemberHours = 0;
                DecemberHours = 0;
                AvgYearly = 0;
                ForecastYearly = 0;

                var contract = await _service.GetContractByYear(SelectedYear);

                if (contract == null)
                {
                    ContractMissing = true;                    
                }
                else
                {
                    ContractMissing = false;
                    YearlyLimit = contract.YearlyLimitHours;

                    var sessions = await _service.GetSessionsByYear(SelectedYear);

                    if (sessions != null && sessions.Count > 0)                     
                    {


                        var smartWorkingHoursByMonth = sessions
                            .Where(s => s.Type == SessionType.SmartWorking)
                            .GroupBy(s => s.StartDate.Month)
                            .ToDictionary(x => x.Key, x => x.Sum(s => (s.EndDate - s.StartDate).TotalHours));

                        JanuaryHours = smartWorkingHoursByMonth.ContainsKey(1) ? smartWorkingHoursByMonth[1] : 0;
                        FebruaryHours = smartWorkingHoursByMonth.ContainsKey(2) ? smartWorkingHoursByMonth[2] : 0;
                        MarchHours = smartWorkingHoursByMonth.ContainsKey(3) ? smartWorkingHoursByMonth[3] : 0;
                        AprilHours = smartWorkingHoursByMonth.ContainsKey(4) ? smartWorkingHoursByMonth[4] : 0;
                        MayHours = smartWorkingHoursByMonth.ContainsKey(5) ? smartWorkingHoursByMonth[5] : 0;
                        JuneHours = smartWorkingHoursByMonth.ContainsKey(6) ? smartWorkingHoursByMonth[6] : 0;
                        JulyHours = smartWorkingHoursByMonth.ContainsKey(7) ? smartWorkingHoursByMonth[7] : 0;
                        AugustHours = smartWorkingHoursByMonth.ContainsKey(8) ? smartWorkingHoursByMonth[8] : 0;
                        SeptemberHours = smartWorkingHoursByMonth.ContainsKey(9) ? smartWorkingHoursByMonth[9] : 0;
                        OctoberHours = smartWorkingHoursByMonth.ContainsKey(10) ? smartWorkingHoursByMonth[10] : 0;
                        NovemberHours = smartWorkingHoursByMonth.ContainsKey(11) ? smartWorkingHoursByMonth[11] : 0;
                        DecemberHours = smartWorkingHoursByMonth.ContainsKey(12) ? smartWorkingHoursByMonth[12] : 0;

                        // ✅ calcolo settimane nel mese
                        //int weeks = GetWeeksInMonth(SelectedYear, SelectedMonth + 1);

                        //double weeklyExpected = WeeklyLimit * weeks;            


                        // ✅ previsione annuale
                        double monthsCompleted = sessions.Max(x => x.StartDate.Month);
                        double SmartHoursYearly = sessions.Where(s => s.Type == SessionType.SmartWorking).Sum(s => (s.EndDate - s.StartDate).TotalHours);

                        AvgYearly = monthsCompleted > 0 ? (SmartHoursYearly / monthsCompleted) : 0;
                        ForecastYearly = AvgYearly * 12;
                    }
                }

                OnPropertyChanged(nameof(ContractMissing));
                OnPropertyChanged(nameof(JanuaryHours));
                OnPropertyChanged(nameof(FebruaryHours));
                OnPropertyChanged(nameof(MarchHours));
                OnPropertyChanged(nameof(AprilHours));
                OnPropertyChanged(nameof(MayHours));
                OnPropertyChanged(nameof(JuneHours));
                OnPropertyChanged(nameof(JulyHours));
                OnPropertyChanged(nameof(AugustHours));
                OnPropertyChanged(nameof(SeptemberHours));
                OnPropertyChanged(nameof(OctoberHours));
                OnPropertyChanged(nameof(NovemberHours));
                OnPropertyChanged(nameof(DecemberHours));
                OnPropertyChanged(nameof(ForecastYearly));
                OnPropertyChanged(nameof(StatusYearly));
                OnPropertyChanged(nameof(AvgYearly));
            }
            catch (Exception)
            {

            }
            finally
            {
                IsLoading = false;
            }
        }

        /*
        private int GetWeeksInMonth(int year, int month)
        {
            var first = new DateTime(year, month, 1);
            var last = first.AddMonths(1).AddDays(-1);

            return (int)Math.Ceiling((last.Day + (int)first.DayOfWeek) / 7.0);
        }
        */
    }
}
