using CsvHelper.Configuration.Attributes;

namespace ScheduleService.Models
{
    public class TimetableDto
    {
        [Ignore]
        public string? Id { get; set; }
        public string? Classroom { get; set; }
        public string? Subject { get; set; }
        public string? Room { get; set; }
        public string? Teacher { get; set; }
        public string? TimeSlot { get; set; } // slot 150ph
        public string StartDate { get; set; }
    }
}
