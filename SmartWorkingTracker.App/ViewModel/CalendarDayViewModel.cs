using System;
using System.Collections.Generic;
using System.Text;

namespace SmartWorkingTracker.App.ViewModel
{
    public class CalendarDayViewModel
    {
        public int Index { get; set; }
        public DateTime Date { get; set; }
        
        public bool IsCurrentMonth { get; set; }

        public bool HasPresence { get; set; }
        public bool HasSmartWorking { get; set; }

        public DayType DayType =>
            !HasPresence && !HasSmartWorking
                ? DayType.None
                : HasPresence && HasSmartWorking
                    ? DayType.Mixed
                    : HasSmartWorking
                        ? DayType.SmartWorking
                        : DayType.Presence;

        public int Day => Date.Day;
    }

    public enum DayType
    {
        None,
        Presence,
        SmartWorking,
        Mixed
    }

}
