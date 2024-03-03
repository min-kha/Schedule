using FileService.Interface;
using Newtonsoft.Json;
namespace FileService;

public class JsonFileService : IFileService
{
    public async Task<IEnumerable<T>> ReadAsync<T>(string filePath)
    {
        using StreamReader file = File.OpenText(filePath);
        var serializer = new JsonSerializer();
        return (IEnumerable<T>)serializer.Deserialize(file, typeof(IEnumerable<T>));
    }

    public async Task WriteAsync<T>(string filePath, IEnumerable<T> records)
    {
        using StreamWriter file = File.CreateText(filePath);
        JsonSerializer serializer = new JsonSerializer();
        serializer.Serialize(file, records);
    }
}
