using ScheduleCore.Entities;
using ScheduleService.Logic.Interfaces;
using ScheduleService.Models;

namespace ScheduleService.Service
{
    public interface ITimetableImporter
    {
        public Task<ImportResult<TimetableExtend>> ImportNewScheduleFromFileAsync(string filePath);

    }
}
