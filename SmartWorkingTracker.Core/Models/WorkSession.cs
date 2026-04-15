using SmartWorkingTracker.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartWorkingTracker.Core.Models
{
    public class WorkSession
    {
        public WorkSession()
        {
                
        }

        public int Id { get; set; } = 0;
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string? Notes { get; set; }
        public SessionType SessionType { get; set; }
        public DateOnly Date => DateOnly.FromDateTime(From);
        public int Week => System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(From, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
    }
}
