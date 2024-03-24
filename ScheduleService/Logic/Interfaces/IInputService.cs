using FileService;
using FileService.Interface;
using ScheduleService.Models;

namespace ScheduleService.Logic.Interfaces;

public interface IInputService
{
    public Task<IEnumerable<TimetableDto>> ReadFromFileAsync(string filePath);
}
