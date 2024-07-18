using System;
using System.Collections.Generic;

namespace ScheduleCore.Entities
{
    public partial class Subject
    {
        public Subject()
        {
            Classrooms = new HashSet<Classroom>();
            Marks = new HashSet<Mark>();
            Timetables = new HashSet<Timetable>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public int? Credit { get; set; }
        public string? Description { get; set; }
        public int CreditSlot { get; set; }

        public virtual ICollection<Classroom> Classrooms { get; set; }
        public virtual ICollection<Mark> Marks { get; set; }
        public virtual ICollection<Timetable> Timetables { get; set; }
    }
}
