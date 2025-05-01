using KriptoScraper.Domain.Entities;

namespace KriptoScraper.Domain.Interfaces;
public interface IKlineEventWriter
{
    Task WriteAsync(string symbol, KlineEvent klineEvent);
}
