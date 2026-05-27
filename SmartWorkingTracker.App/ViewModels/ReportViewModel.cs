using SmartWorkingTracker.Core.Enums;
using SmartWorkingTracker.Data.Services;
using System.Collections.ObjectModel;

namespace SmartWorkingTracker.App.ViewModels
{
    public class MonthlyItem
    {
        public string MonthName { get; set; }
        public double Hours { get; set; }
    }

    public class ReportViewModel : BaseViewModel
    {
        private readonly DatabaseService _service;
        
        public int SelectedYear { get; set; } = DateTime.Now.Year;


        public List<int> Years { get; set; } =
            Enumerable.Range(2020, DateTime.Now.Year - 2019).ToList();        

        public ObservableCollection<MonthlyItem> MonthlyData { get; set; } = new();

        public double YearlyLimit { get; private set; }
        public double MonthlyLimit { get; private set; }
        public double MonthlyWeightedLimit => YearlyLimit == 0 ? 0 : YearlyLimit / 12;
        public double AvgYearly { get; private set; }
        public double ForecastYearly { get; private set; }
        public string StatusYearly => ForecastYearly > YearlyLimit ? "Previsto sforamento annuale ❌" : "Annuale OK ✅";
        public double UsagePercent { get; private set; }
        public double TotalYearly { get; private set; }


        public bool ContractMissing { get; set; }


        public ReportViewModel(DatabaseService service)
        {
            _service = service;
        }

        public async Task Load()
        {
            IsLoading = true;

            try
            {
                MonthlyData.Clear();
                AvgYearly = 0;
                ForecastYearly = 0;
                UsagePercent = 0;
                TotalYearly = 0;

                YearlyLimit = 0;
                MonthlyLimit = 0;

                var contract = await _service.GetContractByYear(SelectedYear);

                if (contract == null)
                {
                    ContractMissing = true;                    
                }
                else
                {
                    ContractMissing = false;
                    YearlyLimit = contract.YearlyLimitHours;
                    MonthlyLimit = contract.MonthlyLimitHours;

                    var sessions = await _service.GetSessionsByYear(SelectedYear);

                    if (sessions != null && sessions.Count > 0)                     
                    {
                        var smartWorkingHoursByDate = sessions
                            .Where(s => s.Type != SessionType.Presenza)
                            .GroupBy(s => s.StartDate)
                            .ToDictionary(x => x.Key, x => x.Sum(s => (s.EndDate - s.StartDate).TotalHours));

                        MonthlyData.Clear();
                        for (int month = 1; month <= 12; month++)
                        {
                            MonthlyItem item = new MonthlyItem
                            {
                                MonthName = new DateTime(SelectedYear, month, 1).ToString("MMMM"),
                                Hours = 0,
                            };

                            foreach (var hoursByDate in smartWorkingHoursByDate.Where(x => x.Key.Month == month))
                            {
                                item.Hours += hoursByDate.Value > 8 ? 8 : hoursByDate.Value;
                            }

                            MonthlyData.Add(item);

                        }

                        // ✅ previsione annuale
                        double monthsCompleted = sessions.Max(x => x.StartDate.Month);
                        TotalYearly = MonthlyData.Sum(s => s.Hours);

                        UsagePercent = YearlyLimit == 0 ? 0 : TotalYearly / YearlyLimit;
                        AvgYearly = monthsCompleted > 0 ? (TotalYearly / monthsCompleted) : 0;
                        ForecastYearly = AvgYearly * 12;
                    }
                }

                OnPropertyChanged(nameof(UsagePercent));

                OnPropertyChanged(nameof(ContractMissing));

                OnPropertyChanged(nameof(ForecastYearly));
                OnPropertyChanged(nameof(StatusYearly));
                OnPropertyChanged(nameof(AvgYearly));
                OnPropertyChanged(nameof(TotalYearly));                
                
                OnPropertyChanged(nameof(YearlyLimit));
                OnPropertyChanged(nameof(MonthlyLimit));
                OnPropertyChanged(nameof(MonthlyWeightedLimit));
            }
            catch (Exception)
            {

            }
            finally
            {
                IsLoading = false;
            }
        }

    }
}
