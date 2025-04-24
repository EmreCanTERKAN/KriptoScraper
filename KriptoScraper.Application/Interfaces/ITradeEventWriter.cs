using KriptoScraper.Domain.Entities;

namespace KriptoScraper.Application.Interfaces;
public interface ITradeEventWriter
{
    Task WriteAsync(string symbol, string interval, TradeEvent tradeEvent);
}
