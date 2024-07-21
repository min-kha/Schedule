namespace ScheduleHost.DTOs
{
    public class StudentRankDto
    {
        public string StudentCode { get; set; } = null!;
        /// <summary>
        /// Xét theo điểm thi đại học hoặc học bạ sẽ ra được rank (Hệ thống khác xử lý)
        /// </summary>
        public int? Rank { get; set; }
    }
}
