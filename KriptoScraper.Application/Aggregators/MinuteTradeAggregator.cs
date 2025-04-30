using KriptoScraper.Application.Entities;
using KriptoScraper.Application.Helpers;
using KriptoScraper.Application.Interfaces;
using KriptoScraper.Domain.Entities;

namespace KriptoScraper.Application.Aggregators;
public class MinuteTradeAggregator : ITradeAggregator<MinuteSummary>
{
    public IEnumerable<MinuteSummary> Aggregate(IEnumerable<TradeEvent> trades)
    {
        var tradeList = trades?.ToList() ?? new List<TradeEvent>();

        if (!tradeList.Any())
            return Enumerable.Empty<MinuteSummary>();

        var currentUtcMinute = TimeHelper.TruncateToMinute(DateTime.UtcNow);

        return tradeList
            .Where(t => TimeHelper.TruncateToMinute(t.EventTimeUtc) < currentUtcMinute)
            .GroupBy(t => TimeHelper.TruncateToMinute(t.EventTimeUtc))
            .OrderBy(g => g.Key)
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
}


