using System;
using System.Collections.Generic;

namespace ScheduleCore.Entities
{
    public partial class Classroom
    {
        public Classroom()
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
