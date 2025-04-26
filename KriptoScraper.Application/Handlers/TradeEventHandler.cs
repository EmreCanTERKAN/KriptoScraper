using KriptoScraper.Domain.Entities;
using KriptoScraper.Domain.Interfaces;

namespace KriptoScraper.Domain.Handlers;
public class TradeEventHandler(
    ITradeAggregatorService aggregatorService,
    ITradeEventWriter writer) : ITradeEventHandler
{
    public async Task HandleAsync(TradeEvent tradeEvent)
    {
        aggregatorService.AddTrade(tradeEvent);
        await writer.WriteAsync(tradeEvent.Symbol, tradeEvent.TimeFrame.ToString(), tradeEvent);
    }
}
