using FileService;
using FileService.Interface;
using Microsoft.AspNetCore.Http;
using ScheduleService.Models;

namespace ScheduleService.Logic.Interfaces;

public interface IInputService
{
    public Task<IEnumerable<TimetableDto>> ReadFromFileAsync(string filePath);
    /// <summary>
    /// Copy file to host with filename {originalFileName}_{timestamp}{fileExtension}
    /// </summary>
    /// <param name="folder"></param>
    /// <param name="file"></param>
    /// <returns></returns>
    public Task<string> CopyFileToHost(string folder, IFormFile file);
}
