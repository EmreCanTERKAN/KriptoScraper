using KriptoScraper.Domain.Entities;
using KriptoScraper.Domain.Interfaces;

namespace KriptoScraper.Domain.Services;
public class MinuteTradeAggregator : ITradeAggregator<MinuteSummary>
{
    public List<MinuteSummary> Aggregate(List<TradeEvent> trades)
    {
        if (trades == null || trades.Count == 0)
            return new List<MinuteSummary>();

        return trades
            .GroupBy(t => TruncateToMinute(t.EventTimeUtc))
            .OrderBy(g => g.Key)
            .Select(group =>
            {
                var ordered = group.OrderBy(t => t.EventTimeUtc).ToList();
                var open = ordered.First().Price;
                var close = ordered.Last().Price;
                var high = group.Max(t => t.Price);
                var low = group.Min(t => t.Price);
                var volume = group.Sum(t => t.Quantity);
                var tradeCount = group.Count();
                var buyerMakerCount = group.Count(t => t.IsBuyerMaker);
                var buyerMakerRatio = tradeCount > 0 ? (double)buyerMakerCount / tradeCount : 0.0;

                return new MinuteSummary(
                    period: group.Key,
                    open: open,
                    high: high,
                    low: low,
                    close: close,
                    volume: volume,
                    tradeCount: tradeCount,
                    buyerMakerRatio: buyerMakerRatio
                );
            })
            .ToList();
    }

    private DateTime TruncateToMinute(DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day,
                            dateTime.Hour, dateTime.Minute, 0, DateTimeKind.Utc);
    }
}


