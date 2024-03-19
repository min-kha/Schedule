using FileService;
using FileService.Interface;
using ScheduleService.Models;

namespace ScheduleService.Logic;

public class InputService
{
    private readonly IFileService _jsonFileService;
    private readonly IFileService _xmlFileService;
    private readonly IFileService _csvFileService;
    public InputService(IEnumerable<IFileService> fileServices)
    {
        _jsonFileService = fileServices.First(s => s.GetType() == typeof(JsonFileService));
        _xmlFileService = fileServices.First(s => s.GetType() == typeof(XmlFileService));
        _csvFileService = fileServices.First(s => s.GetType() == typeof(CsvFileService));
    }


    public async Task<IEnumerable<TimetableDto>> ReadFromFileAsync(string filePath)
    {
        string fileExtension = Path.GetExtension(filePath);

        switch (fileExtension.ToLower())
        {
            case ".json":
                return await _jsonFileService.ReadAsync<TimetableDto>(filePath);
            case ".xml":
                return await _xmlFileService.ReadAsync<TimetableDto>(filePath);
            case ".csv":
                return await _csvFileService.ReadAsync<TimetableDto>(filePath);
            default:
                throw new NotSupportedException($"File format '{fileExtension}' is not supported.");
        }
    }
}
