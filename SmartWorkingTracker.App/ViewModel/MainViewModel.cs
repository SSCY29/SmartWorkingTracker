using CommunityToolkit.Mvvm.ComponentModel;
using SmartWorkingTracker.Core.Enums;
using SmartWorkingTracker.Core.Models;
using SmartWorkingTracker.Core.Services;
using SmartWorkingTracker.Data.Database;
using SmartWorkingTracker.Data.Entities;
using System.Windows.Input;

namespace SmartWorkingTracker.App.ViewModel
{

    public partial class MainViewModel : ObservableObject
    {
        private readonly WorkSessionsService _workSessionsService;
        private readonly WorkContractsService _workContractsService;
        private readonly AppDatabase _database;

        public MainViewModel(
            AppDatabase database,
            WorkSessionsService workSessionsService,
            WorkContractsService workContractsService)
        {
            _workSessionsService = workSessionsService;
            _workContractsService = workContractsService;
            _database = database;

            SelectedDate = new DateTime(
                DateTime.Today.Year,
                DateTime.Today.Month,
                1);

            LoadCommand = new Command(async () => await LoadAsync());
        }

        // ======================================
        // SELEZIONE MESE
        // ======================================

        [ObservableProperty]
        private DateTime _selectedDate;

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private bool _isWeeklySmartWorkingLimitExceeded;

        [ObservableProperty]
        private bool _isMonthlySmartWorkingLimitExceeded;

        [ObservableProperty]
        private List<WorkSession> _workSessions;

        [ObservableProperty]
        private WorkContract _contract;

        [ObservableProperty]
        private double _currentWeekTotalWorkingHours;

        [ObservableProperty]
        private double _currentMonthTotalWorkingHours;

        [ObservableProperty]
        private double _currentWeekPresenceWorkingHours;

        [ObservableProperty]
        private double _currentMonthPresenceWorkingHours;

        [ObservableProperty]
        private double _currentWeekSmartWorkingHours;

        [ObservableProperty]
        private double _currentMonthSmartWorkingHours;

        

        public string SelectedDateDisplay => SelectedDate.ToString("dd MMMM yyyy");


        // ======================================
        // COMANDI
        // ======================================

        public ICommand LoadCommand { get; }

        // ======================================
        // LOGICA
        // ======================================

        private async Task LoadAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                
                await _database.InitializeAsync();

                // 1️ Caricamento Sessioni e contratto
                foreach (WorkSessionEntity ent in await _workSessionsService.GetSessionByMonthAsync(SelectedDate.Year, SelectedDate.Month))
                {
                    WorkSession session = new WorkSession
                    {
                        Id = ent.Id,
                        From = ent.From,
                        To = ent.To,
                        Notes = ent.Notes,
                        SessionType = (SessionType)ent.SessionType
                    };
                    WorkSessions.Add(session);
                }
                

                WorkContractEntity workContractEntity = await _workContractsService.GetContractByYearAsync(SelectedDate.Year);

                if (workContractEntity != null)
                {
                    Contract = new WorkContract
                    {
                        Year = workContractEntity.Year,
                        WeeklyHoursLimit = workContractEntity.WeeklyHoursLimit,
                        MonthlyHoursLimit = workContractEntity.MonthlyHoursLimit,
                        YearlyHoursLimit = workContractEntity.YearlyHoursLimit,
                        ContractType = (ContractType)workContractEntity.ContractType
                    };
                }
                else
                {
                    Contract = new WorkContract
                    {
                        Year = DateTime.Today.Year,
                        WeeklyHoursLimit = 16,
                        MonthlyHoursLimit = 64,
                        YearlyHoursLimit = 120,
                        ContractType = ContractType.FullTime
                    };
                }

                // 2️ Calcolo riepilogo
                double currentWeekNumber = System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(SelectedDate, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);

                if (WorkSessions != null)
                {
                    CurrentWeekTotalWorkingHours = WorkSessions.Where(x => x.Week == currentWeekNumber).Sum(x => (x.To - x.From).TotalHours);
                    CurrentMonthTotalWorkingHours = WorkSessions.Where(x => x.From.Month == SelectedDate.Month).Sum(x => (x.To - x.From).TotalHours);

                    CurrentWeekPresenceWorkingHours = WorkSessions.Where(x => x.Week == currentWeekNumber && x.SessionType == SessionType.Presence).Sum(x => (x.To - x.From).TotalHours);
                    CurrentMonthPresenceWorkingHours = WorkSessions.Where(x => x.From.Month == SelectedDate.Month && x.SessionType == SessionType.Presence).Sum(x => (x.To - x.From).TotalHours);

                    CurrentWeekSmartWorkingHours = WorkSessions.Where(x => x.Week == currentWeekNumber && x.SessionType == SessionType.SmartWorking).Sum(x => (x.To - x.From).TotalHours);
                    CurrentMonthSmartWorkingHours = WorkSessions.Where(x => x.From.Month == SelectedDate.Month && x.SessionType == SessionType.SmartWorking).Sum(x => (x.To - x.From).TotalHours);

                    IsWeeklySmartWorkingLimitExceeded = CurrentWeekSmartWorkingHours > Contract.WeeklyHoursLimit;
                    IsMonthlySmartWorkingLimitExceeded = CurrentMonthSmartWorkingHours > Contract.MonthlyHoursLimit;
                }
            }
            finally
            {
                IsBusy = false;
            }
        }
    }


}
