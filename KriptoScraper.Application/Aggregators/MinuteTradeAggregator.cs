using KriptoScraper.Application.Entities;
using KriptoScraper.Application.Interfaces;

namespace KriptoScraper.Application.Aggregators;
public class MinuteTradeAggregator : ITradeAggregator<MinuteSummary>
{
    public IEnumerable<MinuteSummary> Aggregate(IEnumerable<TradeEvent> trades)
    {
        var tradeList = trades?.ToList() ?? new List<TradeEvent>();

        if (!tradeList.Any())
            return Enumerable.Empty<MinuteSummary>();

        return tradeList.GroupBy(t => TruncateToMinute(t.EventTimeUtc)).OrderBy(g => g.Key)
            .Select(group =>
                {
                    var ordered = group.OrderBy(t => t.EventTimeUtc).ToList();
                    var open = ordered.First().Price;
                    var close = ordered.Last().Price;
                    var high = ordered.Max(t => t.Price);
                    var low = ordered.Min(t => t.Price);
                    var volume = group.Sum(t => t.Quantity);
                    var tradeCount = group.Count();
                    var buyerMakerCount = group.Count(t => t.IsBuyerMaker);
                    var buyerMakerRatio = tradeCount > 0
                        ? (double)buyerMakerCount / tradeCount
                        : 0.0;

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
                });
    }

    private DateTime TruncateToMinute(DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day,
                            dateTime.Hour, dateTime.Minute, 0, DateTimeKind.Utc);
    }
}


