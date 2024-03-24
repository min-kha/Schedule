using ScheduleCore.Entities;
using ScheduleService.Models;

namespace ScheduleService.Logic.Interfaces
{
    public interface ITimetableService
    {
        public List<TimetableExtend> GenerateScheduleAll(IEnumerable<TimetableDto> timetableDtos);
    }
}
