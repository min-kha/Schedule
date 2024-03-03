using FileService.Interface;
using Newtonsoft.Json;
namespace FileService;

public class JsonFileService : IFileService
{
    public async Task<IEnumerable<T>> ReadAsync<T>(string filePath)
    {
        using StreamReader file = File.OpenText(filePath);
        var content = await file.ReadToEndAsync();
        return JsonConvert.DeserializeObject<IEnumerable<T>>(content) ?? new List<T>();
    }

    public async Task WriteAsync<T>(string filePath, IEnumerable<T> records)
    {
        var content = JsonConvert.SerializeObject(records);
        await File.WriteAllTextAsync(filePath, content);
    }
}
