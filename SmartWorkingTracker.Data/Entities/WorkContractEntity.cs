using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartWorkingTracker.Data.Entities
{
    [Table("WorkContracts")]
    public class WorkContractEntity
    {
        [PrimaryKey, Indexed]
        public int Year { get; set; }
        public double WeeklyHoursLimit { get; set; }
        public double MonthlyHoursLimit { get; set; }
        public double YearlyHoursLimit { get; set; }
        public int ContractType { get; set; } = 0;

    }
}
