using KriptoScraper.Application.Entities;
using KriptoScraper.Domain.Interfaces;

namespace KriptoScraper.Application.Aggregators;
public class MultiMinuteAggregator : ISummaryAggregator<MinuteSummary>
{
    public IEnumerable<MinuteSummary> Aggregate(IEnumerable<MinuteSummary> summaries, TimeSpan interval)
    {
        var summaryList = summaries?.OrderBy(s => s.Period).ToList() ?? new List<MinuteSummary>();

        if (!summaryList.Any())
            return Enumerable.Empty<MinuteSummary>();

        return summaryList
            .GroupBy(s => TruncatePeriod(s.Period, interval))
            .OrderBy(g => g.Key)
            .Select(group =>
            {
                var ordered = group.OrderBy(s => s.Period).ToList();
                var open = ordered.First().Open;
                var close = ordered.Last().Close;
                var high = ordered.Max(s => s.High);
                var low = ordered.Min(s => s.Low);
                var volume = group.Sum(s => s.Volume);
                var tradeCount = group.Sum(s => s.TradeCount);
                var buyerMakerRatio = group.Any()
                    ? group.Average(s => s.BuyerMakerRatio)
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

    private static DateTime TruncatePeriod(DateTime time, TimeSpan interval)
    {
        var ticks = (time.Ticks / interval.Ticks) * interval.Ticks;
        return new DateTime(ticks, time.Kind);
    }
}

