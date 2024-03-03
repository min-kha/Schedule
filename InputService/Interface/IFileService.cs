using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Interface
{
    public interface IFileService
    {
        Task<IEnumerable<T>> ReadAsync<T>(string filePath);
        Task WriteAsync<T>(string filePath, IEnumerable<T> records);
    }
}
