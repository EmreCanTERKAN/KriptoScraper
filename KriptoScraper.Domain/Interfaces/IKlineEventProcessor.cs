using KriptoScraper.Domain.Entities;

namespace KriptoScraper.Application.Interfaces;
public interface IKlineEventProcessor
{
    Task ProcessAsync(KlineEvent klineEvent);
}
