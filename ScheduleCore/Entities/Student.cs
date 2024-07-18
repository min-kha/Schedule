using System;
using System.Collections.Generic;

namespace ScheduleCore.Entities
{
    public partial class Student
    {
        public Student()
        {
            Attends = new HashSet<Attend>();
            Marks = new HashSet<Mark>();
            StudentClassrooms = new HashSet<StudentClassroom>();
        }

        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }

        public virtual ICollection<Attend> Attends { get; set; }
        public virtual ICollection<Mark> Marks { get; set; }
        public virtual ICollection<StudentClassroom> StudentClassrooms { get; set; }
    }
}
