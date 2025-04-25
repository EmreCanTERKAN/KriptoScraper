using KriptoScraper.Application.Interfaces;
using KriptoScraper.Domain.Entities;
using KriptoScraper.Interfaces.DataStorage;

namespace KriptoScraper.Application.Services;
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
