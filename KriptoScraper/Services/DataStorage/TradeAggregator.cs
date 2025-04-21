using KriptoScraper.Interfaces.DataStorage;
using KriptoScraper.Models;

namespace KriptoScraper.Services.DataStorage;
public class TradeAggregator : ITradeAggregator
{
    public List<MinuteSummary> AggregateToMinuteSummary(List<TradeEvent> trades)
    {
        return trades
            .GroupBy(t => new DateTime(t.EventTimeUtc.Year, t.EventTimeUtc.Month, t.EventTimeUtc.Day,
                                        t.EventTimeUtc.Hour, t.EventTimeUtc.Minute, 0))
            .Select(g =>
            {
                var ordered = g.OrderBy(t => t.EventTimeUtc).ToList();
                return new MinuteSummary
                {
                    Minute = g.Key,
                    Open = ordered.First().Price,
                    High = g.Max(t => t.Price),
                    Low = g.Min(t => t.Price),
                    Close = ordered.Last().Price,
                    Volume = g.Sum(t => t.Quantity),
                    TradeCount = g.Count(),
                    BuyerMakerRatio = g.Count(t => t.IsBuyerMaker) / (double)g.Count()
                };
            })
            .OrderBy(m => m.Minute)
            .ToList();
    }
}

