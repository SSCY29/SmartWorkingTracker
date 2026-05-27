using SmartWorkingTracker.Core.Enums;
using SmartWorkingTracker.Core.Models;

namespace SmartWorkingTracker.App.ViewModels
{

    public class DayViewModel
    {
        public DateTime Date { get; set; }

        public List<WorkSession> Sessions { get; set; } = new();

        public bool IsEmpty => Date == DateTime.MinValue;

        public string Display => IsEmpty ? "" : Date.ToString("dd");

        public bool IsWeekend =>
            Date.DayOfWeek == DayOfWeek.Saturday ||
            Date.DayOfWeek == DayOfWeek.Sunday;

        public Microsoft.Maui.Graphics.Color Color
        {
            get
            {
                var isDark = Application.Current.RequestedTheme == AppTheme.Dark;

                if (IsEmpty)
                    return Colors.Transparent;

                if (IsWeekend)
                    return (Color)Application.Current.Resources["WeekendColor" + (isDark ? "Dark" : "Light")];

                if (!Sessions.Any())
                    return Colors.Transparent;

                var types = Sessions.Select(s => s.Type).Distinct().ToList();

                if (types.Count > 1)
                    return (Color)Application.Current.Resources["MixedColor" + (isDark ? "Dark" : "Light")];

                return types.First() switch
                {
                    SessionType.Presenza => (Color)Application.Current.Resources["PresenceColor" + (isDark ? "Dark" : "Light")],
                    SessionType.SmartWorking => (Color)Application.Current.Resources["SwColor" + (isDark ? "Dark" : "Light")],
                    SessionType.NonPresente => (Color)Application.Current.Resources["AbsentColor" + (isDark ? "Dark" : "Light")],
                    _ => Colors.Transparent

                };
            }
        }

        public string Summary
        {
            get
            {
                if (!Sessions.Any())
                    return "";

                return $"{Sessions.Count}";
            }
        }

    }

}
