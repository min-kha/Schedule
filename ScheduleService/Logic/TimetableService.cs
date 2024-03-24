using ScheduleCore.Entities;
using ScheduleService.Logic.Interfaces;
using ScheduleService.Models;
using ScheduleService.Utils;
using System.Xml;

namespace ScheduleService.Logic
{
    public class TimetableService : ITimetableService
    {
        private readonly ScheduleContext _context;
        public TimetableService(ScheduleContext context)
        {
            _context = context;
        }

        public List<TimetableExtend> GenerateScheduleAll(IEnumerable<TimetableDto> timetableDtos)
        {
            var timetableExtends = new List<TimetableExtend>();
            int index = 0;

            foreach (var timetableDto in timetableDtos)
            {
                index++;
                try
                {
                    var generatedTimetables = GenerateSemesterSchedule(timetableDto);
                    foreach (var timetable in generatedTimetables)
                    {
                        timetableExtends.Add(new TimetableExtend { Timetable = timetable, TimetableDtoId = index });
                    }
                }
                catch (InvalidDataException ex)
                {
                    timetableExtends.Add(new TimetableExtend { Timetable = null, TimetableDtoId = index, Message = ex.Message });
                }
                catch (Exception ex )
                {
                    Console.WriteLine("***Error: GenerateScheduleAll - " + ex.Message);
                    timetableExtends.Add(new TimetableExtend { Timetable = null, TimetableDtoId = index, Message = "Lỗi không xác định" });
                }
            }

            return timetableExtends;
        }

        public List<Timetable> GenerateSemesterSchedule(TimetableDto timetableDto)
        {
            if (timetableDto.TimeSlot == null || !Util.IsTimeSlotDouble(timetableDto.TimeSlot)) throw new InvalidDataException();
            var timetables = new List<Timetable>();

            var classroom = GetClassroom(timetableDto.Classroom);
            var subject = GetSubject(timetableDto.Subject);
            var room = GetRoom(timetableDto.Room);
            var teacher = GetTeacher(timetableDto.Teacher);
            var (firstSlot, secondSlot, firstWeekTime, secondWeekTime) = GetTimeSlots(timetableDto.TimeSlot.ToString()!);

            if (subject != null)
            {
                var firstDate = Util.FindNextDayOfWeek(timetableDto.StartDate, firstWeekTime);
                var secondDate = Util.FindNextDayOfWeek(timetableDto.StartDate, secondWeekTime);
                // đổi chỗ
                if (firstDate > secondDate)
                {
                    (secondDate, firstDate) = (firstDate, secondDate);
                    (secondSlot, firstSlot) = (firstSlot, secondSlot);
                }
                for (int i = 0; i < subject.CreditSlot; i++)
                {
                    if (i % 2 == 0)
                    {

                        timetables.Add(new Timetable
                        {
                            Classroom = classroom,
                            ClassroomId = classroom?.Id,
                            Subject = subject,
                            SubjectId = subject.Id,
                            Room = room,
                            RoomId = room?.Id,
                            Teacher = teacher,
                            TeacherId = teacher?.Id,
                            Slot = firstSlot,
                            Date = firstDate
                        });
                        firstDate = firstDate.AddDays(7);

                    }
                    else
                    {
                        timetables.Add(new Timetable
                        {
                            Classroom = classroom,
                            ClassroomId = classroom?.Id,
                            Subject = subject,
                            SubjectId = subject.Id,
                            Room = room,
                            RoomId = room?.Id,
                            Teacher = teacher,
                            TeacherId = teacher?.Id,
                            Slot = secondSlot,
                            Date = secondDate
                        });
                        secondDate = secondDate.AddDays(7);
                    }
                }
            }

            return timetables;
        }

        private Classroom? GetClassroom(string? classroomCode)
        {
            if (string.IsNullOrEmpty(classroomCode))
                return null;
            return _context.Classrooms.FirstOrDefault(c => c.Code.ToUpper() == classroomCode.ToUpper()) ?? throw new InvalidDataException("Không tìm thấy lớp học với classroomCode = " + classroomCode);
        }

        private Subject? GetSubject(string? subjectCode)
        {
            if (string.IsNullOrEmpty(subjectCode))
                return null;
            return _context.Subjects.FirstOrDefault(s => s.Code.ToUpper() == subjectCode.ToUpper()) ?? throw new InvalidDataException("Không tìm thấy môn học với subjectCode = " + subjectCode);
        }

        private Room? GetRoom(string? roomNumber)
        {
            bool isInt = int.TryParse(roomNumber, out var roomId);
            if (string.IsNullOrEmpty(roomNumber) || !isInt)
                return null;
            return _context.Rooms.FirstOrDefault(r => r.RoomNumber == roomId) ?? throw new InvalidDataException("Không tìm thấy phòng học với roomNumber = " + roomNumber);
        }

        private Teacher? GetTeacher(string? teacherCode)
        {
            if (string.IsNullOrEmpty(teacherCode))
                return null;
            return _context.Teachers.FirstOrDefault(t => t.TeacherCode.ToUpper() == teacherCode.ToUpper()) ?? throw new InvalidDataException("Không tìm thấy giáo viên với teacherCode = " + teacherCode);
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
                firstSlot = GetSlotDByNumber(1);
                secondSlot = GetSlotDByNumber(2);
            }
            else if (dateSession == 'P')
            {
                firstSlot = GetSlotDByNumber(3);
                secondSlot = GetSlotDByNumber(4);
            }
            else
            {
                throw new InvalidDataException();
            }

            return (firstSlot, secondSlot, firstWeekTime, secondWeekTime);
        }


        private Slot GetSlotDByNumber(int slotNum)
        {
            return _context.Slots.First(s => s.Name == $"Slot {slotNum}D");
        }

    }
}
