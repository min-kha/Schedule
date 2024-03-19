using ScheduleCore.Entities;
using ScheduleService.Models;

namespace ScheduleService.Logic
{
    public class TimetableService
    {
        private readonly ScheduleContext _context;
        public TimetableService(ScheduleContext context)
        {
            _context = context;
        }
        public List<Timetable> GenerateSemesterSchedule(TimetableDto timetableDto)
        {
            if (timetableDto.TimeSlotDouble == null || !IsTimeSlotDouble(timetableDto.TimeSlotDouble)) throw new InvalidDataException();
            var timetables = new List<Timetable>();

            var classroom = GetClassroom(timetableDto.Classroom);
            var subject = GetSubject(timetableDto.Subject);
            var room = GetRoom(timetableDto.Room);
            var teacher = GetTeacher(timetableDto.Teacher);
            var (firstSlot, secondSlot, firstWeekTime, secondWeekTime) = GetTimeSlots(timetableDto.TimeSlotDouble.ToString()!);

            if (subject != null)
            {
                var firstDate = FindNextDayOfWeek(timetableDto.StartDate, firstWeekTime);
                var secondDate = FindNextDayOfWeek(timetableDto.StartDate, secondWeekTime);

                for (int i = 0; i < subject.CreditSlot; i++)
                {
                    timetables.Add(new Timetable
                    {
                        Classroom = classroom,
                        Subject = subject,
                        Room = room,
                        Teacher = teacher,
                        Slot = firstSlot,
                        Date = firstDate
                    });
                    timetables.Add(new Timetable
                    {
                        Classroom = classroom,
                        Subject = subject,
                        Room = room,
                        Teacher = teacher,
                        Slot = secondSlot,
                        Date = secondDate
                    });
                    firstDate = firstDate.AddDays(7);
                    secondDate = secondDate.AddDays(7);
                }
            }

            return timetables;
        }

        private Classroom? GetClassroom(string? classroomCode)
        {
            if (string.IsNullOrEmpty(classroomCode))
                return null;
            return _context.Classrooms.FirstOrDefault(c => c.Code.ToUpper() == classroomCode.ToUpper());
        }

        private Subject? GetSubject(string? subjectCode)
        {
            if (string.IsNullOrEmpty(subjectCode))
                return null;
            return _context.Subjects.FirstOrDefault(s => s.Code.ToUpper() == subjectCode.ToUpper());
        }

        private Room? GetRoom(int? roomNumber)
        {
            if (roomNumber == null)
                return null;
            return _context.Rooms.FirstOrDefault(r => r.RoomNumber == roomNumber);
        }

        private Teacher? GetTeacher(string? teacherCode)
        {
            if (string.IsNullOrEmpty(teacherCode))
                return null;
            return _context.Teachers.FirstOrDefault(t => t.TeacherCode.ToUpper() == teacherCode.ToUpper());
        }

        private (Slot firstSlot, Slot secondSlot, DayOfWeek firstWeekTime, DayOfWeek secondWeekTime) GetTimeSlots(string timeSlotDouble)
        {
            if (string.IsNullOrEmpty(timeSlotDouble))
                throw new InvalidDataException();

            char dateSession = timeSlotDouble[0];
            var firstWeekTime = (DayOfWeek)(int.Parse(timeSlotDouble[1].ToString()) - 1);
            var secondWeekTime = (DayOfWeek)(int.Parse(timeSlotDouble[2].ToString()) - 1);
            Slot firstSlot, secondSlot;

            if (dateSession == 'A')
            {
                firstSlot = GetSlotByNumber(1);
                secondSlot = GetSlotByNumber(2);
            }
            else if (dateSession == 'P')
            {
                firstSlot = GetSlotByNumber(3);
                secondSlot = GetSlotByNumber(4);
            }
            else
            {
                throw new InvalidDataException();
            }

            return (firstSlot, secondSlot, firstWeekTime, secondWeekTime);
        }

        /// <summary>
        /// Tìm ngày gần nhất lớn hơn fromDate và có cùng thứ với dayOfWeek
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        public DateTime FindNextDayOfWeek(DateTime fromDate, DayOfWeek dayOfWeek)
        {
            int daysToAdd = ((int)dayOfWeek - (int)fromDate.DayOfWeek + 7) % 7;
            return fromDate.AddDays(daysToAdd);
        }
        private Slot GetSlotByNumber(int slotNum)
        {
            return _context.Slots.First(s => s.Name == $"Slot {slotNum}D");
        }

        private bool IsTimeSlotDouble(string input)
        {
            return Enum.TryParse(input, out TimeSlotDouble timeSlotDouble);
        }
    }
}
