using SQLite;

namespace SmartWorkingTracker.Core.Models
{
    public class Settings
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DayOfWeek FirstDayOfWeek { get; set; }

        public string DateFormat { get; set; } = "dd/MM/yyyy";
    }

}
