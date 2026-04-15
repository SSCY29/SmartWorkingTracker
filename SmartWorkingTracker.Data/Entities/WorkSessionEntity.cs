using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartWorkingTracker.Data.Entities
{
    [Table("WorkSessions")]
    public class WorkSessionEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public DateTime From { get; set; }
        [Indexed]
        public DateTime To { get; set; }
        public string Notes { get; set; }
        public int SessionType { get; set; } = 0;

    }
}
