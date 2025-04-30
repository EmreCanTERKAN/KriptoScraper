namespace KriptoScraper.Application.Interfaces;
public interface ITradeLoggerService
{
    Task StartLoggingAsync(CancellationToken cancellationToken = default);
}
