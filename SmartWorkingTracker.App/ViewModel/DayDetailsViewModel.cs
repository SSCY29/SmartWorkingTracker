using CommunityToolkit.Mvvm.ComponentModel;
using SmartWorkingTracker.Core.Enums;
using SmartWorkingTracker.Core.Models;
using SmartWorkingTracker.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SmartWorkingTracker.App.ViewModel
{
    [QueryProperty(nameof(DateString), "date")]
    public partial class DayDetailsViewModel : ObservableObject
    {
        private readonly WorkSessionsService _service;

        public DayDetailsViewModel(WorkSessionsService service)
        {
            _service = service;
            Sessions = new ObservableCollection<WorkSession>();
        }

        public string DateString
        {
            set
            {
                SelectedDate = DateTime.Parse(value);
                LoadSessions();
            }
        }

        [ObservableProperty]
        private DateTime selectedDate;

        [ObservableProperty]
        private ObservableCollection<WorkSession> sessions;

        private async void LoadSessions()
        {
            Sessions.Clear();

            var result = await _service.GetSessionByDateAsync(SelectedDate);

            foreach (var s in result.OrderBy(s => s.From))
                Sessions.Add(new WorkSession
                {
                    Id = s.Id,
                    From = s.From,
                    To = s.To,
                    Notes = s.Notes,
                    SessionType = (SessionType)s.SessionType
                });
        }
    }
}
