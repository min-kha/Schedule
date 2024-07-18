using System;
using System.Collections.Generic;

namespace ScheduleCore.Entities
{
    public partial class Mark
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public int ClassroomId { get; set; }
        public int StudentId { get; set; }
        public double? Mark1 { get; set; }
        public double? Percent { get; set; }
        public string? Name { get; set; }

        public virtual Classroom Classroom { get; set; } = null!;
        public virtual Student Student { get; set; } = null!;
        public virtual Subject Subject { get; set; } = null!;
    }
}
