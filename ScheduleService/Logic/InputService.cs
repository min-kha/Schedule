using FileService;
using FileService.Interface;
using Microsoft.AspNetCore.Http;
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

    public async Task<string> CopyFileToHost(string folder, IFormFile file)
    {
        // Get the original file name
        string originalFileName = Path.GetFileNameWithoutExtension(file.FileName);
        string fileExtension = Path.GetExtension(file.FileName);

        // Create a timestamp string
        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");

        // Combine the original file name, timestamp, and extension
        string newFileName = $"{originalFileName}_{timestamp}{fileExtension}";

        // Combine the uploads folder path and the new file name
        string filePath = Path.Combine(folder, newFileName);
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        return filePath;
    }
    public async Task<IEnumerable<T>> ReadFromFileAsync<T>(IFormFile file) where T : class, new()
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("File không hợp lệ hoặc trống.");
        }

        string tempFilePath = Path.GetRandomFileName();

        try
        {
            using (var stream = new FileStream(tempFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            string fileExtension = Path.GetExtension(file.FileName).ToLower();
            IEnumerable<T> items = fileExtension switch
            {
                ".json" => await _jsonFileService.ReadAsync<T>(tempFilePath),
                ".xml" => await _xmlFileService.ReadAsync<T>(tempFilePath),
                ".csv" => await _csvFileService.ReadAsync<T>(tempFilePath),
                _ => throw new NotSupportedException($"Định dạng tệp '{fileExtension}' không được hỗ trợ."),
            };

            // Kiểm tra xem T có thuộc tính Id không
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty != null && idProperty.PropertyType == typeof(string))
            {
                int index = 1;
                foreach (var item in items)
                {
                    idProperty.SetValue(item, index.ToString());
                    index++;
                }
            }

            return items;
        }
        finally
        {
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }
    }


    public async Task<IEnumerable<T>> ReadFromFileAsync<T>(string filePath) where T : class, new()
    {
        string tempFilePath = Path.GetFileNameWithoutExtension(filePath) + "-temp" + Path.GetExtension(filePath);

        try
        {
            File.Copy(filePath, tempFilePath);
        }
        catch (Exception ex)
        {
            // Xử lý các lỗi tiềm ẩn trong quá trình sao chép (ví dụ: không đủ quyền)
            throw new Exception($"Không thể sao chép tệp: {ex.Message}");
        }

        try
        {
            string fileExtension = Path.GetExtension(tempFilePath);
            IEnumerable<T> items = fileExtension.ToLower() switch
            {
                ".json" => await _jsonFileService.ReadAsync<T>(tempFilePath),
                ".xml" => await _xmlFileService.ReadAsync<T>(tempFilePath),
                ".csv" => await _csvFileService.ReadAsync<T>(tempFilePath),
                _ => throw new NotSupportedException($"Định dạng tệp '{fileExtension}' không được hỗ trợ."),
            };

            // Kiểm tra xem T có thuộc tính Id không
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty != null && idProperty.PropertyType == typeof(string))
            {
                int index = 1;
                foreach (var item in items)
                {
                    idProperty.SetValue(item, index.ToString());
                    index++;
                }
            }

            return items;
        }
        finally
        {
            File.Delete(tempFilePath);
        }
    }
}
