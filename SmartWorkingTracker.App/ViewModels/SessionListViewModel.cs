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
            Date = date;
            Sessions.Clear();


            var sessions = await _service.GetSessionsByMonth(Date.Year, Date.Month);

            sessions = sessions.Where(s => s.StartDate.Date == Date).OrderBy(s => s.StartDate).ToList();

            foreach (var s in sessions)
                Sessions.Add(s);
        }


        public async Task Delete(WorkSession session)
        {
            await _service.DeleteSession(session);

            // REMOVE DIRETTO (più veloce e sicuro)
            Sessions.Remove(session);

        }


    }
}