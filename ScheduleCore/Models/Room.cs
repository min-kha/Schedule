using System;
using System.Collections.Generic;

namespace ScheduleCore.Models
{
    public partial class Room
    {
        public Room()
        {
            Timetables = new HashSet<Timetable>();
        }

        public int Id { get; set; }
        public int Capacity { get; set; }
        public int RoomNumber { get; set; }
        public int BuildingId { get; set; }
        public string? Note { get; set; }

        public virtual Building Building { get; set; } = null!;
        public virtual ICollection<Timetable> Timetables { get; set; }
    }
}
