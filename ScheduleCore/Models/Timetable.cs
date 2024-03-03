using System;
using System.Collections.Generic;

namespace ScheduleCore.Models
{
    public partial class Timetable
    {
        public int Id { get; set; }
        public int? SlotId { get; set; }
        public int? ClassId { get; set; }
        public int? TeacherId { get; set; }
        public int? RoomId { get; set; }
        public int? SubjectId { get; set; }
        public DateTime Date { get; set; }

        public virtual Class? Class { get; set; }
        public virtual Room? Room { get; set; }
        public virtual Slot? Slot { get; set; }
        public virtual Subject? Subject { get; set; }
        public virtual Teacher? Teacher { get; set; }
    }
}
