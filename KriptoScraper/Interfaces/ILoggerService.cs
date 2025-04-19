namespace KriptoScraper.Interfaces;
public interface ILoggerService
{
    Task LogAsync(string symbol, DateTime time, decimal price);
}
