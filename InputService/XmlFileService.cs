using FileService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FileService;

public class XmlFileService : IFileService
{
    public async Task<IEnumerable<T>> ReadAsync<T>(string filePath)
    {
        XmlSerializer serializer = new(typeof(List<T>));
        using FileStream fileStream = new(filePath, FileMode.Open);
        using var reader = new StreamReader(fileStream);
        var content = await reader.ReadToEndAsync();
        using var stringReader = new StringReader(content);
        return (IEnumerable<T>)(serializer.Deserialize(stringReader) ?? new List<T>());
    }

    public async Task WriteAsync<T>(string filePath, IEnumerable<T> records)
    {
        XmlSerializer serializer = new(typeof(List<T>));
        using FileStream fileStream = new(filePath, FileMode.Create);
        using var writer = new StringWriter();
        serializer.Serialize(writer, records);
        var content = writer.ToString();
        await File.WriteAllTextAsync(filePath, content);
    }
}
