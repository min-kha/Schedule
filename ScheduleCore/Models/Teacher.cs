﻿using System;
using System.Collections.Generic;

namespace ScheduleCore.Models
{
    public partial class Teacher
    {
        public Teacher()
        {
            Timetables = new HashSet<Timetable>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string TeacherCode { get; set; } = null!;

        public virtual ICollection<Timetable> Timetables { get; set; }
    }
}
