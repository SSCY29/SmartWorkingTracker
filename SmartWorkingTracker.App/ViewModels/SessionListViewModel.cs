using SmartWorkingTracker.Core.Models;
using SmartWorkingTracker.Data.Services;
using System.Collections.ObjectModel;

namespace SmartWorkingTracker.App.ViewModels
{

    public class SessionListViewModel : BaseViewModel
    {
        private readonly DatabaseService _service;

        public ObservableCollection<WorkSession> Sessions { get; set; } = new();


        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date)); // 🔥 fondamentale
            }
        }


        public SessionListViewModel(DatabaseService db)
        {
            _service = db;
        }

        public async Task Load(DateTime date)
        {
            IsLoading = true;

            try
            {
                Date = date;
                Sessions.Clear();

                var sessions = await _service.GetSessionsByMonth(Date.Year, Date.Month);

                sessions = sessions?.Where(s => s.StartDate.Date == Date)?.ToList() ?? new List<WorkSession>();

                foreach (var s in sessions.OrderBy(x => x.StartDate.Hour).ThenBy(x => x.StartDate.Minute))
                    Sessions.Add(s);

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

        public async Task Delete(WorkSession session)
        {
            IsLoading = true;

            try
            {
                await _service.DeleteSession(session);

            // REMOVE DIRETTO (più veloce e sicuro)
            Sessions.Remove(session);
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