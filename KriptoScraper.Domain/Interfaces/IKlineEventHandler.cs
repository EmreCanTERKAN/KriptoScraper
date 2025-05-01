using KriptoScraper.Domain.Entities;

namespace KriptoScraper.Domain.Interfaces;
public interface IKlineEventHandler
{
    Task HandleAsync(KlineEvent klineEvent);
}
