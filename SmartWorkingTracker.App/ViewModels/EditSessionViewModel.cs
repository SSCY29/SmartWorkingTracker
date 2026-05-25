using SmartWorkingTracker.Core.Enums;
using SmartWorkingTracker.Core.Helpers;
using SmartWorkingTracker.Core.Models;
using SmartWorkingTracker.Data.Services;

namespace SmartWorkingTracker.App.ViewModels
{
    public class EditSessionViewModel : BaseViewModel
    {
        private WorkSession _currentSession;
        private readonly DatabaseService _service;

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public int TypeIndex { get; set; }

        public string Notes { get; set; }

        public DateTime Date { get; private set; }
        public int? Id { get; private set; }

        public EditSessionViewModel(DatabaseService service)
        {
            _service = service;

        }

        public void SetDate(DateTime? date) // ✅ Inizializza i valori di default per una nuova sessione
        {
            if (date == null)
                return;

            Date = date.Value;
            StartTime = new TimeSpan(9, 0, 0);
            EndTime = new TimeSpan(18, 0, 0);
            TypeIndex = 0;
            Notes = string.Empty;

            OnPropertyChanged(nameof(Date));
            OnPropertyChanged(nameof(StartTime));
            OnPropertyChanged(nameof(EndTime));
            OnPropertyChanged(nameof(TypeIndex));
            OnPropertyChanged(nameof(Notes));
        }

        public void SetId(int? id)
        {
            this.Id = id;
        }

        public async Task LoadSession() // ✅ Carica i dati di una sessione esistente
        {
            IsLoading = true;
            try
            {
                if(Id != null)
                {
                    var sessions = await _service.GetSessionById(Id.Value);
                    var session = sessions.FirstOrDefault();

                    if (session == null)
                        return;
                    else
                    {
                        _currentSession = session;
                        InitializeData();
                    }
                }
                
            }
            catch (Exception)
            {

            }
            finally
            {
                await Task.Delay(500);
                IsLoading = false;
            }

        }

        private async void InitializeData()
        {

            Date = _currentSession.StartDate.Date;
            StartTime = _currentSession.StartDate.TimeOfDay;
            EndTime = _currentSession.EndDate.TimeOfDay;
            TypeIndex = (int)_currentSession.Type;
            Notes = _currentSession.Notes;


            OnPropertyChanged(nameof(Date));
            OnPropertyChanged(nameof(StartTime));
            OnPropertyChanged(nameof(EndTime));
            OnPropertyChanged(nameof(TypeIndex));
            OnPropertyChanged(nameof(Notes));
        }

        public async Task<bool> Save()
        {
            IsLoading = true;
            bool result = true;
            try
            {
                WorkSession session;

                if (_currentSession != null)
                {
                    // ✅ UPDATE
                    session = _currentSession;

                    session.StartDate = Date.Date + StartTime;
                    session.EndDate = Date.Date + EndTime;
                    session.Type = (SessionType)TypeIndex;
                    session.Notes = Notes;
                }
                else
                {
                    // ✅ INSERT
                    session = new WorkSession
                    {
                        StartDate = Date.Date + StartTime,
                        EndDate = Date.Date + EndTime,
                        Type = (SessionType)TypeIndex,
                        Notes = Notes
                    };
                }

                var existing = await _service.GetSessionsByDate(Date.Date.Year, Date.Date.Month, Date.Date.Day);

                if (!WorkSessionValidator.IsValid(session, existing))
                    result = false;
                else
                    await _service.SaveSession(session);

                
            }
            catch (Exception)
            {
                result = false;
            }
            finally
            {
                IsLoading = false;
            }

            return result;
        }
    }
}
