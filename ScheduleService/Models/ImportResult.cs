namespace ScheduleService.Models
{
    public class ImportError<T>
    {
        public T Item { get; set; }
        public string Message { get; set; }
        public Exception? Exception { get; set; }

        public ImportError(T item, string message, Exception? exception = null)
        {
            Item = item;
            Message = message;
            Exception = exception;
        }
    }

    public class ImportResult<T>
    {
        public List<T> SuccessfullyImporteds { get; private set; } = new List<T>();
        public List<T> ErrorImporteds { get; private set; } = new();
    }
}
