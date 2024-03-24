using FileService;
using FileService.Interface;
using ScheduleService.Logic.Interfaces;
using ScheduleService.Models;

namespace ScheduleService.Logic;

public class InputService : IInputService
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
        string tempFilePath = Path.GetFileNameWithoutExtension(filePath) + "-temp" + Path.GetExtension(filePath);

        try
        {
            File.Copy(filePath, tempFilePath);
        }
        catch (Exception ex)
        {
            // Handle potential errors during copying (e.g., insufficient permissions)
            throw new Exception($"Failed to copy file: {ex.Message}");
        }
        try
        {
            string fileExtension = Path.GetExtension(tempFilePath);
            IEnumerable<TimetableDto> timetableDtos = fileExtension.ToLower() switch
            {
                ".json" => await _jsonFileService.ReadAsync<TimetableDto>(tempFilePath),
                ".xml" => await _xmlFileService.ReadAsync<TimetableDto>(tempFilePath),
                ".csv" => await _csvFileService.ReadAsync<TimetableDto>(tempFilePath),
                _ => throw new NotSupportedException($"File format '{fileExtension}' is not supported."),
            };
            for (int i = 0; i < timetableDtos.Count(); i++)
            {
                timetableDtos.ElementAt(i).Id = (i + 1).ToString();
            }
            return timetableDtos;
        }
        finally
        {
            // Ensure the temporary file is deleted regardless of success or failure
            File.Delete(tempFilePath);
        }
    }
}
