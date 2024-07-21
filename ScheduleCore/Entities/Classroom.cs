using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ScheduleCore.Entities
{
    public partial class Classroom
    {
        public Classroom()
        {
            Marks = new HashSet<Mark>();
            StudentClassrooms = new HashSet<StudentClassroom>();
            Timetables = new HashSet<Timetable>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public int Semesters { get; set; }
        public int Year { get; set; }
        public int? SubjectId { get; set; }
        [JsonIgnore]
        public virtual Subject? Subject { get; set; }
        [JsonIgnore]
        public virtual ICollection<Mark> Marks { get; set; }
        [JsonIgnore]
        public virtual ICollection<StudentClassroom> StudentClassrooms { get; set; }
        [JsonIgnore]
        public virtual ICollection<Timetable> Timetables { get; set; }
    }
}
