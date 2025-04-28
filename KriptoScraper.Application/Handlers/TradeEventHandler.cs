using KriptoScraper.Application.Entities;
using KriptoScraper.Application.Interfaces;
using KriptoScraper.Domain.Interfaces;

namespace KriptoScraper.Application.Handlers;
public class TradeEventHandler(
    ITradeAggregatorService aggregatorService,
    ITradeEventWriter writer) : ITradeEventHandler
{
    public async Task HandleAsync(TradeEvent tradeEvent)
    {
        aggregatorService.AddTrade(tradeEvent);
        await writer.WriteAsync(tradeEvent.Symbol, tradeEvent);
    }
}
