using KriptoScraper.Application.Entities;
using KriptoScraper.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace KriptoScraper.Application.Aggregators;
public class MultiMinuteAggregator(
    ILogger<MultiMinuteAggregator> logger) : ISummaryAggregator<MinuteSummary>
{
    public IEnumerable<MinuteSummary> Aggregate(IEnumerable<MinuteSummary> summaries, TimeSpan interval)
    {
        var summaryList = summaries?.OrderBy(s => s.Period).ToList() ?? new List<MinuteSummary>();

        foreach (var item in summaryList)
        {
            logger.LogDebug("📌 Period: {Period}", item.Period);
        }

        if (!summaryList.Any())
            return Enumerable.Empty<MinuteSummary>();

        logger.LogInformation("✅ {Count} adet mum okundu. İlk: {FirstPeriod}, Son: {LastPeriod}",
            summaryList.Count,
            summaryList.First().Period,
            summaryList.Last().Period);

        return summaryList
            .GroupBy(s => TruncatePeriod(s.Period, interval))
            .Where(group => group.Count() == (int)interval.TotalMinutes)  // sadece tam gruplar
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
                var buyerMakerRatio = group.Average(s => s.BuyerMakerRatio);

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
        return DateTime.SpecifyKind(new DateTime(ticks), time.Kind);
    }
}