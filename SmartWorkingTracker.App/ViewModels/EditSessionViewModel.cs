using SmartWorkingTracker.Core.Enums;
using SmartWorkingTracker.Core.Helpers;
using SmartWorkingTracker.Core.Models;
using SmartWorkingTracker.Data.Services;

namespace SmartWorkingTracker.App.ViewModels
{
    public class SessionTypeItem
    {
        public SessionType Type { get; set; }
        public string Name { get; set; }
    }


    public class EditSessionViewModel : BaseViewModel
    {
        private WorkSession _currentSession;
        private readonly DatabaseService _service;

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Notes { get; set; }
        public DateTime? Date { get; private set; }
        public int? Id { get; private set; }
        public SessionTypeItem SelectedTypeItem { get; set; }

        public List<SessionTypeItem> Types { get; } = new()
        {
            new() { Type = SessionType.Presenza, Name = "Presenza" },
            new() { Type = SessionType.SmartWorking, Name = "Smart working" },
            new() { Type = SessionType.NonPresente, Name = "Non presente" }
        };


        public EditSessionViewModel(DatabaseService service)
        {
            _service = service;

        }

        public void SetDate(DateTime? date) // ✅ Inizializza i valori di default per una nuova sessione
        {            
            Date = date;            
        }

        public void SetId(int? id)
        {
            this.Id = id;
        }

        public async Task LoadSession() // ✅ Carica i dati di una sessione esistente
        {
            IsLoading = true;

            _currentSession = null;
            try
            {
                if (Id != null)
                {
                    var sessions = await _service.GetSessionById(Id.Value);
                    var session = sessions.FirstOrDefault();

                    _currentSession = session;
                    
                }
                else if(Date != null)
                {
                    _currentSession = new WorkSession()
                    {
                        StartDate = Date.Value + new TimeSpan(9,0,0),
                        EndDate = Date.Value + new TimeSpan(18,0,0),
                        Serial = Guid.NewGuid().ToString(),
                        Type = SessionType.SmartWorking,
                        Notes = string.Empty
                    };
                }

                InitializeData();

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
            SelectedTypeItem = Types.FirstOrDefault(x => x.Type == _currentSession.Type);
            Notes = _currentSession.Notes;


            OnPropertyChanged(nameof(Date));
            OnPropertyChanged(nameof(StartTime));
            OnPropertyChanged(nameof(EndTime));
            OnPropertyChanged(nameof(SelectedTypeItem));
            OnPropertyChanged(nameof(Notes));
        }

        public async Task<bool> Save()
        {
            IsLoading = true;
            bool result = true;
            try
            {
                _currentSession.StartDate = Date.Value + StartTime;
                _currentSession.EndDate = Date.Value + EndTime;
                _currentSession.Notes = Notes;
                _currentSession.Type = SelectedTypeItem.Type;

                WorkSession session = _currentSession;

                if (session == null)
                {
                    return false;
                }

                var existing = await _service.GetSessionsByDate(Date.Value.Year, Date.Value.Month, Date.Value.Day);

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
