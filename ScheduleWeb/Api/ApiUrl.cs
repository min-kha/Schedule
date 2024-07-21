using ScheduleCore.Entities;

namespace ScheduleWeb.Api
{
    public static class ApiUrl
    {
        internal static readonly string DOMAIN = "https://localhost:7167/api/";
        internal static readonly string API_TIMETABLE = DOMAIN + "timetables/";
        internal static readonly string API_STUDENT=DOMAIN + "students/";
        internal static readonly string API_TEACHER = DOMAIN + "teachers/";
        internal static readonly string API_CLASSROOM = DOMAIN + "classrooms/";
        internal static readonly string API_CLASSIFICATION = DOMAIN + "studentclassrooms/";
        internal static readonly string API_SUBJECT = DOMAIN + "subjects/";


        internal static readonly string API_GET_TIMETABLE_TEACHER = API_TIMETABLE + "teacher/";
        internal static readonly string API_GET_TIMETABLE_CLASSROOM = API_TIMETABLE + "classroom/"; 
        internal static readonly string API_GET_STUDENT_CLASSROOM = API_STUDENT + "classroom/"; 
        internal static readonly string API_GET_CLASSROOM_SUBJECT = API_STUDENT + "subject/"; 
        internal static readonly string API_POST_SCHEDULE_FROM_FILE = API_TIMETABLE + "import/";
        internal static readonly string API_POST_STUDENT_FROM_FILE = API_STUDENT + "import/";
        internal static readonly string API_POST_CLASSIFICATION_RANK_FROM_FILE = API_CLASSIFICATION + "import/rank/";
    }
}
