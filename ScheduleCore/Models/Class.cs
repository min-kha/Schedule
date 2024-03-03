using System;
using System.Collections.Generic;

namespace ScheduleCore.Models
{
    public partial class Class
    {
        public Class()
        {
            Timetables = new HashSet<Timetable>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public int Semesters { get; set; }
        public int Year { get; set; }

        public virtual ICollection<Timetable> Timetables { get; set; }
    }
}
