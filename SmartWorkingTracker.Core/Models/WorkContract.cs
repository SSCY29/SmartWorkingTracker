using SmartWorkingTracker.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartWorkingTracker.Core.Models
{
    public class WorkContract
    {
        public int Year { get; set; }
        public double WeeklyHoursLimit { get; set; }
        public double MonthlyHoursLimit { get; set; }
        public double YearlyHoursLimit { get; set; }
        public ContractType ContractType { get; set; } = 0;

        // ore di lavoro giornaliere
        // giorni di lavoro da contratto
    }
}
