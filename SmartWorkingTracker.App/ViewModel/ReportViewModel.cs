using CommunityToolkit.Mvvm.ComponentModel;
using SmartWorkingTracker.Core.Enums;
using SmartWorkingTracker.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SmartWorkingTracker.App.ViewModel
{
    public partial class ReportViewModel : ObservableObject
    {
        private readonly WorkSessionsService _workSessionsService;

        public ReportViewModel(WorkSessionsService workSessionsService)
        {
            _workSessionsService = workSessionsService;

            SelectedMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            GenerateCsvCommand = new Command(async () => await GenerateCsvAsync());
        }

        [ObservableProperty] private DateTime selectedMonth;
        [ObservableProperty] private bool isBusy;
        [ObservableProperty] private string? statusMessage;

        public bool HasStatus => !string.IsNullOrWhiteSpace(StatusMessage);

        public ICommand GenerateCsvCommand { get; }

        private async Task GenerateCsvAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                StatusMessage = null;

                var sessions = await _workSessionsService
                    .GetSessionByMonthAsync(SelectedMonth.Year, SelectedMonth.Month);

                var sb = new StringBuilder();

                // Header CSV
                sb.AppendLine("Data,Ora Inizio,Ora Fine,Tipo,Ore,Note");

                foreach (var s in sessions)
                {
                    var hours = (s.To - s.From).TotalHours;
                    sb.AppendLine(
                        $"{s.From:yyyy-MM-dd}," +
                        $"{s.From:HH\\:mm}," +
                        $"{s.To:HH\\:mm}," +
                        $"{((SessionType)s.SessionType)}," +
                        $"{hours:F2}," +
                        $"\"{s.Notes}\"");
                }

                var fileName = $"SmartWorkingReport_{SelectedMonth:yyyy_MM}.csv";
                var path = Path.Combine(FileSystem.CacheDirectory, fileName);

                File.WriteAllText(path, sb.ToString(), Encoding.UTF8);

                // Condivisione Android / iOS
                await Share.Default.RequestAsync(new ShareFileRequest
                {
                    Title = "Report Smart Working",
                    File = new ShareFile(path)
                });

                StatusMessage = "Report generato correttamente.";
                OnPropertyChanged(nameof(HasStatus));
            }
            catch (Exception ex)
            {
                StatusMessage = $"Errore: {ex.Message}";
                OnPropertyChanged(nameof(HasStatus));
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
