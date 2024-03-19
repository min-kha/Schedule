using System.Reflection.Metadata.Ecma335;

namespace ScheduleService.Models
{
    public class TimetableDto
    {
        public string? Classroom { get; set; }
        public string? Subject { get; set; }
        public int? Room { get; set; }
        public string? Teacher { get; set; }
        public string? TimeSlotDouble { get; set; }
        public DateTime StartDate { get; set; }
    }
}
