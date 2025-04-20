namespace KriptoScraper.Interfaces;
public interface ILoggerService<T>
{
    Task LogAsync(string symbol, DateTime time, T data);
}
