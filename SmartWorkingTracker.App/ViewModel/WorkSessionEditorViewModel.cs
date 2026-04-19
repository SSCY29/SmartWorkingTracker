using CommunityToolkit.Mvvm.ComponentModel;
using SmartWorkingTracker.Core.Enums;
using SmartWorkingTracker.Core.Services;
using SmartWorkingTracker.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SmartWorkingTracker.App.ViewModel
{

    public partial class WorkSessionEditorViewModel : ObservableObject
    {
        private readonly WorkSessionsService _workSessionsService;

        public WorkSessionEditorViewModel(WorkSessionsService workSessionsService)
        {
            _workSessionsService = workSessionsService;

            SessionDate = DateTime.Today;
            StartTime = new TimeSpan(9, 0, 0);
            EndTime = new TimeSpan(18, 0, 0);

            SessionTypes = Enum.GetValues(typeof(SessionType)).Cast<SessionType>().ToList();
            SelectedSessionType = SessionType.SmartWorking;

            SaveCommand = new Command(async () => await SaveAsync());
            IsBusy = false;
        }

        // =============================
        // PROPRIETÀ
        // =============================

        [ObservableProperty] private DateTime sessionDate;
        [ObservableProperty] private TimeSpan startTime;
        [ObservableProperty] private TimeSpan endTime;
        [ObservableProperty] private SessionType selectedSessionType;
        [ObservableProperty] private string? notes;

        [ObservableProperty] private bool isBusy;
        [ObservableProperty] private string? errorMessage;

        public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

        public List<SessionType> SessionTypes { get; }

        public ICommand SaveCommand { get; }

        // =============================
        // LOGICA
        // =============================

        private async Task SaveAsync()
        {
            if (IsBusy)
                return;

            ErrorMessage = null;

            if (EndTime <= StartTime)
            {
                ErrorMessage = "L'orario di fine deve essere successivo all'inizio.";
                OnPropertyChanged(nameof(HasError));
                return;
            }

            try
            {
                IsBusy = true;

                var entity = new WorkSessionEntity
                {
                    Id = -1,
                    From = SessionDate.Date + StartTime,
                    To = SessionDate.Date + EndTime,
                    Notes = Notes ?? string.Empty,
                    SessionType = (int)SelectedSessionType
                };

                await _workSessionsService.InsertAsync(entity);

                // Torna alla dashboard
                await Shell.Current.GoToAsync("//dashboard");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                OnPropertyChanged(nameof(HasError));
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}
