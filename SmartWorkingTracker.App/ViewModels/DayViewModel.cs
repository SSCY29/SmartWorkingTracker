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

        public string Color
        {
            get
            {
                if (IsEmpty)
                    return "Transparent";

                if (IsWeekend)
                    return "DarkGray";

                if (!Sessions.Any())
                    return "Transparent";

                var types = Sessions.Select(s => s.Type).Distinct().ToList();

                if (types.Count > 1)
                    return "Purple";

                return types.First() switch
                {
                    SessionType.Presenza => "Blue",
                    SessionType.SmartWorking => "Green",
                    SessionType.NonPresente => "Red",
                    _ => "White"
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
