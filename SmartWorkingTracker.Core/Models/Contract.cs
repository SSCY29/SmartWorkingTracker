using SQLite;

namespace SmartWorkingTracker.Core.Models
{
    public class Contract
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        // Unico per anno
        public int Year { get; set; }

        public double WeeklyLimitHours { get; set; }

        public double MonthlyLimitHours { get; set; }

        public double YearlyLimitHours { get; set; }

    }
}
