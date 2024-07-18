using FileService;
using FileService.Interface;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using ScheduleCore.Entities;
using ScheduleService.Logic;
using ScheduleService.Logic.Interfaces;
using ScheduleService.Service;
using System;
using System.Threading.Tasks;

namespace Schedule.Test.Service
{
    [TestFixture]
    public class TimetableImporterTests
    {
        [Test]
        public async Task ImportNewScheduleFromFileAsync_StateUnderTest_ExpectedBehavior()
        {
            IEnumerable<IFileService> fileServices = new List<IFileService>() {
                new JsonFileService(), new CsvFileService(), new XmlFileService()
            };
            IInputService inputService = new InputService(fileServices);
            var context = new StudentManagementContext(); // Sử dụng ScheduleContext thực tế
            var timetableService = new TimetableService(context);
            // Arrange
            var timetableImporter = new TimetableImporter(inputService, timetableService, context);
            string filePath = @"C:\Users\Kha\Downloads\scheduleTest.csv";

            // Act
            var result = await timetableImporter.ImportNewScheduleFromFileAsync(
                filePath);

            // Assert
            result.ErrorImporteds.ForEach(e => TestContext.WriteLine(e.Message));
            Assert.Multiple(() =>
            {
                Assert.That(result.SuccessfullyImporteds, Has.Count.EqualTo(80));
                Assert.That(result.ErrorImporteds, Has.Count.EqualTo(0));
            });
        }
    }
}
