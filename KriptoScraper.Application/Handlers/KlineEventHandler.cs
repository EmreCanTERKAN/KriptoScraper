using KriptoScraper.Application.Interfaces;
using KriptoScraper.Domain.Entities;
using KriptoScraper.Domain.Interfaces;

namespace KriptoScraper.Application.Handlers;
public class KlineEventHandler(
    IKlineEventWriter writer) : IKlineEventHandler
{
    public async Task HandleAsync(KlineEvent klineEvent)
    {
        await writer.WriteAsync(klineEvent.Symbol, klineEvent);
    }
}
