namespace KriptoScraper.Interfaces.Logging;
public interface ILoggerService<T>
{
    Task LogAsync(string symbol, DateTime time, T data);
}
