using KriptoScraper.Application.Interfaces;
using KriptoScraper.Domain.Entities;
using KriptoScraper.Domain.Interfaces;

namespace KriptoScraper.Application.Services;
public class TradeWriterProcessor(ITradeEventWriter writer, ISymbolProvider symbolProvider)
    : ITradeEventProcessor
{
    public async Task ProcessAsync(TradeEvent tradeEvent)
    {
        var symbol = symbolProvider.GetSymbol();
        await writer.WriteAsync(symbol, "1m", tradeEvent); // Interval dinamik de olabilir
    }
}

