using KriptoScraper.Application.Entities;

namespace KriptoScraper.Application.Interfaces;
public interface ITradeEventWriter
{
    Task WriteAsync(string symbol, TimeSpan interval, TradeEvent tradeEvent);
}
