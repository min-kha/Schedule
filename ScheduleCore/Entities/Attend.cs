using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ScheduleCore.Entities
{
    public partial class Attend
    {
        public int Id { get; set; }
        public int TimeTableId { get; set; }
        public int StudentId { get; set; }
        public int? Status { get; set; }
        [JsonIgnore]
        public virtual Student Student { get; set; } = null!;
        [JsonIgnore]
        public virtual Timetable TimeTable { get; set; } = null!;
    }
}
