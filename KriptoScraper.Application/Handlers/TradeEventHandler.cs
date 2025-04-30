using KriptoScraper.Application.Interfaces;
using KriptoScraper.Domain.Entities;
using KriptoScraper.Domain.Interfaces;

namespace KriptoScraper.Application.Handlers;
public class TradeEventHandler(
    IEnumerable<ITradeAggregatorService> aggregatorServices,
    ITradeEventWriter writer) : ITradeEventHandler
{
    public async Task HandleAsync(TradeEvent tradeEvent)
    {
        foreach (var service in aggregatorServices)
        {
            service.AddTrade(tradeEvent);
        }
        await writer.WriteAsync(tradeEvent.Symbol, tradeEvent);
    }
}
