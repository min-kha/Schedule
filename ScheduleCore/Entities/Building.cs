using System;
using System.Collections.Generic;

namespace ScheduleCore.Entities
{
    public partial class Building
    {
        public Building()
        {
            Rooms = new HashSet<Room>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}
