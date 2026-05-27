using SmartWorkingTracker.Core.Enums;
using SQLite;

namespace SmartWorkingTracker.Core.Models
{

    public class WorkSession
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Serial { get; set; } = Guid.NewGuid().ToString();

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string? Notes { get; set; }

        public SessionType Type { get; set; }
        [Ignore]
        public string TypeString => Type == SessionType.Presenza ? "Presenza" : Type == SessionType.SmartWorking ? "Smart working" : "Non presente";

        [Ignore]
        public string Range => $"{StartDate.ToString("HH:mm")} - {EndDate.ToString("HH:mm")}";
    }

}
