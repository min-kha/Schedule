using Microsoft.Data.SqlClient.Server;
using Microsoft.EntityFrameworkCore;
using ScheduleCore.Entities;
using ScheduleService.Logic.Interfaces;
using ScheduleService.Models;
using ScheduleService.Utils;
using System.Xml;

namespace ScheduleService.Logic
{
    public class TimetableService : ITimetableService
    {
        private readonly StudentManagementContext _context;
        public TimetableService(StudentManagementContext context)
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
                catch (Exception ex)
                {
                    Console.WriteLine("***Error: GenerateScheduleAll - " + ex.Message);
                    timetableExtends.Add(new TimetableExtend { Timetable = null, TimetableDtoId = index, Message = "Lỗi không xác định" });
                }
            }

            return timetableExtends;
        }
        public async Task<string?> CheckTimetableConflictForEditAsync(Timetable timetable)
{
    var conflictingTimetable = await _context.Timetables
        .Include(t => t.Classroom)
        .Include(t => t.Room)
        .Include(t => t.Slot)
        .Include(t => t.Subject)
        .Include(t => t.Teacher)
        .FirstOrDefaultAsync(t =>
        (t.TeacherId == timetable.TeacherId ||
            t.RoomId == timetable.RoomId ||
            t.ClassroomId == timetable.ClassroomId) &&
            t.Date.Date == timetable.Date.Date &&
            t.SlotId == timetable.SlotId &&
            t.Id != timetable.Id); // Exclude the current timetable being edited

    if (conflictingTimetable != null)
    {
        var message = $"Trùng với thời gian biểu hiện tại: (";

        if (conflictingTimetable.TeacherId == timetable.TeacherId)
        {
            message += $"Giáo viên {conflictingTimetable.Teacher?.Name}; ";
        }

        if (conflictingTimetable.RoomId == timetable.RoomId)
        {
            message += $"Phòng {conflictingTimetable.Room?.RoomNumber}; ";
        }

        if (conflictingTimetable.ClassroomId == timetable.ClassroomId)
        {
            message += $"Lớp {conflictingTimetable.Classroom?.Code}; ";
        }

        message += ") vào ngày: " + conflictingTimetable.Date.ToString("d/M/yyyy") + " - " + conflictingTimetable.Slot?.Name;

        return message;
    }
    else
    {
        return null; // Không có xung đột
    }
}

        public async Task<string?> CheckTimetableConflictAsync(Timetable timetable)
        {
            var conflictingTimetable = await _context.Timetables
                .Include(t => t.Classroom)
                .Include(t => t.Room)
                .Include(t => t.Slot)
                .Include(t => t.Subject)
                .Include(t => t.Teacher)
                .FirstOrDefaultAsync(t =>
                (t.TeacherId == timetable.TeacherId ||
                    t.RoomId == timetable.RoomId ||
                    t.ClassroomId == timetable.ClassroomId) &&
                    t.Date.Date == timetable.Date.Date &&
                    t.SlotId == timetable.SlotId);

            if (conflictingTimetable != null)
            {
                var message = $"Trùng với thời gian biểu hiện tại: (";

                if (conflictingTimetable.TeacherId == timetable.TeacherId)
                {
                    message += $"Giáo viên {conflictingTimetable.Teacher?.Name}; ";
                }

                if (conflictingTimetable.RoomId == timetable.RoomId)
                {
                    message += $"Phòng {conflictingTimetable.Room?.RoomNumber}; ";
                }

                if (conflictingTimetable.ClassroomId == timetable.ClassroomId)
                {
                    message += $"Lớp {conflictingTimetable.Classroom?.Code}; ";
                }

                message += ") vào ngày: " + conflictingTimetable.Date.ToString("d/M/yyyy") + " - " + conflictingTimetable.Slot?.Name;

                return message;
            }
            else
            {
                return null; // Không có xung đột
            }
        }

        public List<Timetable> GenerateSemesterSchedule(TimetableDto timetableDto)
        {
            if (timetableDto.TimeSlot == null || !Util.IsTimeSlotDouble(timetableDto.TimeSlot)) throw new InvalidDataException("Không tìm thấy TimeSlot Double phù hợp với " + timetableDto?.TimeSlot);
            var timetables = new List<Timetable>();

            var classroom = GetClassroom(timetableDto.Classroom);
            var subject = GetSubject(timetableDto.Subject);
            var room = GetRoom(timetableDto.Room);
            var teacher = GetTeacher(timetableDto.Teacher);
            var (firstSlot, secondSlot, firstWeekTime, secondWeekTime) = GetTimeSlots(timetableDto.TimeSlot.ToString()!);
            if (!DateTime.TryParseExact(timetableDto.StartDate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime startDate))
            {
                throw new InvalidDataException("Sai format StartDate. Đảm bảo bạn nhập theo d/M/yyyy");
            }
            if (subject != null)
            {
                var firstDate = Util.FindNextDayOfWeek(startDate, firstWeekTime);
                var secondDate = Util.FindNextDayOfWeek(startDate, secondWeekTime);
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
                            SlotId = firstSlot.Id,
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
                            SlotId = secondSlot.Id,
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
            if (string.IsNullOrEmpty(roomNumber))
                return null;
            if (!int.TryParse(roomNumber, out var roomId))
            {
                throw new InvalidDataException("Sai format roomNumber, với roomNumber = " + roomNumber);
            }

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
