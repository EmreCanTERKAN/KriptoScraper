namespace KriptoScraper.Application.Interfaces;
public interface IKlineLoggerService
{
    Task StartLoggingAsync(CancellationToken cancellationToken = default);
}
