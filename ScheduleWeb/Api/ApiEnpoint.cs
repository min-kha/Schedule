using ScheduleCore.Entities;

namespace ScheduleWeb.Api
{
    public class ApiEnpoint
    {
        public static readonly string DOMAIN = "https://localhost:7167/api/";
        public static readonly string API_TIMETABLE = DOMAIN + "timetables/";
        public static readonly string API_TEACHER = DOMAIN + "teachers/";
        public static readonly string API_GET_TIMETABLE_TEACHER = API_TIMETABLE + "teacher/";

    }
}
