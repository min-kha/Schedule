using System;
using System.Collections.Generic;

namespace ScheduleCore.Entities
{
    public partial class StudentClassroom
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int ClassroomId { get; set; }

        public virtual Classroom Classroom { get; set; } = null!;
        public virtual Student Student { get; set; } = null!;
    }
}
