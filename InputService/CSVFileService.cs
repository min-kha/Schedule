using CsvHelper;
using FileService.Interface;
using System.Globalization;

namespace FileService;

public class CsvFileService : IFileService
{
    public async Task<IEnumerable<T>> ReadAsync<T>(string filePath)
    {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = new List<T>();
        await foreach (var record in csv.GetRecordsAsync<T>())
        {
            records.Add(record);
        }
        return records;
    }

    public async Task WriteAsync<T>(string filePath, IEnumerable<T> records)
    {
        using var writer = new StreamWriter(filePath);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        await csv.WriteRecordsAsync(records);
    }
}