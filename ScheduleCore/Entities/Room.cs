﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ScheduleCore.Entities
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
        [JsonIgnore]
        public virtual Building Building { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<Timetable> Timetables { get; set; }
    }
}
