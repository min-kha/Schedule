using Moq;
using NUnit.Framework;
using ScheduleCore.Entities;
using ScheduleService.Logic;
using ScheduleService.Models;
using ScheduleService.Utils;

namespace Schedule.Test
{
    [TestFixture]
    public class TimetableMapperTests
    {
        private Mock<ScheduleContext> _mockContext;
        private TimetableService _timetableService;

        [SetUp]
        public void Setup()
        {
            _mockContext = new Mock<ScheduleContext>();
            _timetableService = new TimetableService(_mockContext.Object);
        }

        [Test]
        public void TestFindNextDayOfWeek()
        {
            // Arrange
            DateTime fromDate = new DateTime(2024, 3, 20); // Wednesday
            DayOfWeek dayOfWeek = DayOfWeek.Sunday;

            // Act
            DateTime result = Util.FindNextDayOfWeek(fromDate, dayOfWeek);

            // Assert
            Assert.That(new DateTime(2024, 3, 24), Is.EqualTo(result));

            // Time 2

            // Arrange
            fromDate = new DateTime(2024, 3, 20); // Wednesday
            dayOfWeek = DayOfWeek.Wednesday;

            // Act
            result = Util.FindNextDayOfWeek(fromDate, dayOfWeek);

            // Assert
            Assert.That(new DateTime(2024, 3, 20), Is.EqualTo(result));
        }

        [Test]
        public void GenerateSemesterSchedule_InvalidTimeSlotDouble_ThrowsInvalidDataException()
        {
            // Arrange
            var timetableDto = new TimetableDto
            {
                StartDate = DateTime.Now.Date.ToShortDateString(),
                Classroom = "A101",
                Subject = "MATH101",
                Room = "102",
                Teacher = "JohnDoe",
                TimeSlot = "XYZ"
            };

            // Assert
            Assert.Throws<InvalidDataException>(() => _timetableService.GenerateSemesterSchedule(timetableDto));
        }

        /// <summary>
        /// Test bằng database thật. Với SUB004 có creditSlot = 6 thì tạo ra 12 timetable
        /// </summary>
        [Test]
        public void GenerateSemesterSchedule_ValidData_GeneratesTimetables()
        {
            var context = new ScheduleContext(); // Sử dụng ScheduleContext thực tế
            var timetableService = new TimetableService(context);

            var timetableDto = new TimetableDto
            {
                StartDate = DateTime.Now.Date.ToShortDateString(),
                TimeSlot = "A24",
                //Classroom = "C211", // Maybe null
                //Room = 294, // Maybe null
                //Teacher = "TC00003", // Maybe null
                Subject = "SUB004" // creditSlot = 6
            };

            var generatedTimetables = timetableService.GenerateSemesterSchedule(timetableDto);

            Assert.That(generatedTimetables, Has.Count.EqualTo(6));
        }
    }
}
