using System;
using System.Collections.Generic;

namespace ScheduleCore.Entities
{
    public partial class Slot
    {
        public Slot()
        {
            Timetables = new HashSet<Timetable>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }

        public virtual ICollection<Timetable> Timetables { get; set; }
    }
}
