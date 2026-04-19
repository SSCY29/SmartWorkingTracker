using CommunityToolkit.Mvvm.ComponentModel;
using SmartWorkingTracker.Core.Enums;
using SmartWorkingTracker.Core.Services;
using SmartWorkingTracker.Data.Entities;
using System.Diagnostics;
using System.Windows.Input;

namespace SmartWorkingTracker.App.ViewModel
{
    [QueryProperty(nameof(SessionId), "sessionId")]
    public partial class EditSessionViewModel : ObservableObject
    {
        private readonly WorkSessionsService _service;

        public EditSessionViewModel(WorkSessionsService service)
        {
            _service = service;
            SaveCommand = new Command(async () => await SaveAsync());
            DeleteCommand = new Command(async () => await DeleteAsync());

            SessionTypes = Enum.GetValues<SessionType>().ToList();
        }


        [ObservableProperty]
        private int sessionId;
        partial void OnSessionIdChanged(int value)
        {
            Debug.WriteLine($"SessionId ricevuto: {value}");
            Load(value);
        }


        public List<SessionType> SessionTypes { get; }

        [ObservableProperty] private DateTime day;
        [ObservableProperty] private TimeSpan startTime;
        [ObservableProperty] private TimeSpan endTime;
        [ObservableProperty] private string notes = string.Empty;
        [ObservableProperty] private SessionType sessionType;
        [ObservableProperty] private bool isBusy;

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        private async void Load(int id)
        {
            try
            {
                IsBusy = true;

                var session = await _service.GetByIdAsync(id);

                Day = session.From.Date;
                StartTime = session.From.TimeOfDay;
                EndTime = session.To.TimeOfDay;
                Notes = session.Notes;
                SessionType = (SessionType)session.SessionType;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task SaveAsync()
        {
            if (IsBusy || EndTime <= StartTime) return;

            try
            {
                IsBusy = true;

                await _service.UpdateAsync(new WorkSessionEntity
                {
                    Id = SessionId,
                    From = new DateTime(day.Year, day.Month, day.Day, startTime.Hours, startTime.Minutes, startTime.Seconds),
                    To = new DateTime(day.Year, day.Month, day.Day, endTime.Hours, endTime.Minutes, endTime.Seconds),
                    Notes = Notes,
                    SessionType = (int)SessionType
                });

                await Shell.Current.GoToAsync("..");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task DeleteAsync()
        {
            if (IsBusy) return;

            bool confirm = await Shell.Current.DisplayAlertAsync(
                "Elimina sessione",
                "Sei sicuro di voler eliminare questa sessione?\nL’operazione non può essere annullata.",
                "Elimina",
                "Annulla");

            if (!confirm) return;

            try
            {
                IsBusy = true;
                await _service.DeleteAsync(SessionId);
                await Shell.Current.GoToAsync("..");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
