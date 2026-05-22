using SmartWorkingTracker.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartWorkingTracker.Core.Helpers
{
    public static class WorkSessionValidator
    {
        public static bool IsValid(WorkSession session, List<WorkSession> existingSessions)
        {
            // Check base
            if (session.EndDate <= session.StartDate)
                return false;

            // Check overlap
            return !existingSessions.Any(e =>
                e.Id != session.Id &&
                session.StartDate < e.EndDate &&
                session.EndDate > e.StartDate
            );
        }
    }

}
