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
        aggregatorService.AddTrade(tradeEvent); // Domain servisine gönder
        await writer.WriteAsync(tradeEvent);    // Infrastructure’a yaz
    }
}
