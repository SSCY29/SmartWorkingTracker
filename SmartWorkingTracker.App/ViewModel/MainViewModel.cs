using CommunityToolkit.Mvvm.ComponentModel;
using SmartWorkingTracker.Core.Enums;
using SmartWorkingTracker.Core.Models;
using SmartWorkingTracker.Core.Services;
using SmartWorkingTracker.Data.Database;
using SmartWorkingTracker.Data.Entities;
using System.Collections.ObjectModel;
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
                DateTime.Today.Day);

            

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

        [ObservableProperty]
        private bool _hasSessions;
        
        public ObservableCollection<CalendarDayViewModel> CalendarDays { get; set; } = new ObservableCollection<CalendarDayViewModel>();

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

                WorkSessions = new List<WorkSession>();

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
                        YearlyHoursLimit = 120 * 8,
                        ContractType = ContractType.FullTime
                    };
                }

                // 2️ Calcolo riepilogo
                double currentWeekNumber = System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(SelectedDate, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);

                HasSessions = false;

                CurrentWeekTotalWorkingHours = CurrentMonthTotalWorkingHours =
                    CurrentWeekPresenceWorkingHours = CurrentMonthPresenceWorkingHours =
                    CurrentWeekSmartWorkingHours = CurrentMonthSmartWorkingHours = 0;

                IsWeeklySmartWorkingLimitExceeded = IsMonthlySmartWorkingLimitExceeded = false;

                if (WorkSessions != null && WorkSessions.Count > 0)
                {
                    CurrentWeekTotalWorkingHours = WorkSessions.Where(x => x.Week == currentWeekNumber).Sum(x => (x.To - x.From).TotalHours);
                    CurrentMonthTotalWorkingHours = WorkSessions.Where(x => x.From.Month == SelectedDate.Month).Sum(x => (x.To - x.From).TotalHours);

                    CurrentWeekPresenceWorkingHours = WorkSessions.Where(x => x.Week == currentWeekNumber && x.SessionType == SessionType.Presence).Sum(x => (x.To - x.From).TotalHours);
                    CurrentMonthPresenceWorkingHours = WorkSessions.Where(x => x.From.Month == SelectedDate.Month && x.SessionType == SessionType.Presence).Sum(x => (x.To - x.From).TotalHours);

                    CurrentWeekSmartWorkingHours = WorkSessions.Where(x => x.Week == currentWeekNumber && x.SessionType == SessionType.SmartWorking).Sum(x => (x.To - x.From).TotalHours);
                    CurrentMonthSmartWorkingHours = WorkSessions.Where(x => x.From.Month == SelectedDate.Month && x.SessionType == SessionType.SmartWorking).Sum(x => (x.To - x.From).TotalHours);

                    IsWeeklySmartWorkingLimitExceeded = CurrentWeekSmartWorkingHours > Contract.WeeklyHoursLimit;
                    IsMonthlySmartWorkingLimitExceeded = CurrentMonthSmartWorkingHours > Contract.MonthlyHoursLimit;

                    HasSessions = true;
                }

                BuildCalendar();
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                IsBusy = false;
            }
        }

        partial void OnSelectedDateChanged(DateTime oldValue, DateTime newValue)
        {
            if (LoadCommand != null)
                LoadCommand.Execute(null);

        }

        private void BuildCalendar()
        {
            CalendarDays.Clear();

            // 1️⃣ Primo giorno del mese selezionato
            var firstDayOfMonth =
                new DateTime(SelectedDate.Year, SelectedDate.Month, 1);

            // 2️⃣ Troviamo il LUNEDÌ della settimana che contiene il 1° del mese
            int diff = firstDayOfMonth.DayOfWeek == DayOfWeek.Sunday
                ? 6
                : (int)firstDayOfMonth.DayOfWeek - 1;

            var calendarStart = firstDayOfMonth.AddDays(-diff);

            // 3️⃣ Costruiamo 42 giorni (6 settimane)
            for (int i = 0; i < 42; i++)
            {
                var date = calendarStart.AddDays(i);

                var sessionsForDay = WorkSessions
                    .Where(s => s.From.Date == date.Date)
                    .ToList();

                CalendarDayViewModel cvm = new CalendarDayViewModel();

                cvm.Index = i;
                cvm.Date = date;
                cvm.HasPresence = sessionsForDay.Any(
                    s => s.SessionType == SessionType.Presence);
                cvm.HasSmartWorking = sessionsForDay.Any(
                    s => s.SessionType == SessionType.SmartWorking);
                cvm.IsCurrentMonth = SelectedDate.Date.Month == date.Month;

                CalendarDays.Add(cvm);
            }
            
        }


    }
}
