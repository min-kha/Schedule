using System;
using System.Collections.Generic;

namespace ScheduleCore.Entities
{
    public partial class Timetable
    {
        public Timetable()
        {
            Attends = new HashSet<Attend>();
        }

        public int Id { get; set; }
        public int? SlotId { get; set; }
        public int? ClassroomId { get; set; }
        public int? TeacherId { get; set; }
        public int? RoomId { get; set; }
        public int? SubjectId { get; set; }
        public DateTime Date { get; set; }
        public virtual Classroom? Classroom { get; set; }
        public virtual Room? Room { get; set; }
        public virtual Slot? Slot { get; set; }
        public virtual Subject? Subject { get; set; }
        public virtual Teacher? Teacher { get; set; }
        public virtual ICollection<Attend> Attends { get; set; }
    }
}
