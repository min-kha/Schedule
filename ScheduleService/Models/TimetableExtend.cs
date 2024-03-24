using ScheduleCore.Entities;

namespace ScheduleService.Models
{
    public class TimetableExtend
    {
        public Timetable? Timetable { get; set; }
        public int TimetableDtoId { get; set; }
        public string? Message { get; set; }
    }
}
