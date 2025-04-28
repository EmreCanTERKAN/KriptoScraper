using KriptoScraper.Application.Entities;
using KriptoScraper.Application.Helpers;
using KriptoScraper.Application.Interfaces;

namespace KriptoScraper.Application.Services;
public class TradeAggregatorService<TSummary>(
    ITradeBuffer buffer,
    ITradeAggregator<TSummary> aggregator,
    ISummaryWriter<TSummary> writer,
    TimeSpan interval) : ITradeAggregatorService, IDisposable
    where TSummary : ISummary
{
    private Timer? _timer;
    public void AddTrade(TradeEvent tradeEvent) => buffer.Add(tradeEvent);

    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var nextMinute = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0, DateTimeKind.Utc)
                         .AddMinutes(1);
        var dueTime = nextMinute - now; // İlk çalışmaya kadar bekleme süresi

        _timer = new Timer(async _ =>
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                await AggregateAndWriteAsync(cancellationToken);
            }
        }, null, dueTime, interval); // dikkat: ilk çalıştırma -> dueTime

        return Task.CompletedTask;
    }

    private async Task AggregateAndWriteAsync(CancellationToken cancellationToken)
    {
        try
        {
            var trades = buffer.Drain();
            if (!trades.Any()) return;

            var summaries = aggregator.Aggregate(trades).ToList();
            if (!summaries.Any()) return;

            await writer.WriteBatchAsync(summaries, cancellationToken);

            var currentUtcMinute = TimeHelper.TruncateToMinute(DateTime.UtcNow);
            var unfinishedTrades = trades
                .Where(t => TimeHelper.TruncateToMinute(t.EventTimeUtc) >= currentUtcMinute)
                .ToList();

            foreach (var trade in unfinishedTrades)
            {
                buffer.Add(trade);
            }

        }
        catch (Exception ex)
        {
            // Loglama servisi varsa burada loglayabilirsin.
            Console.WriteLine($"Hata oluştu: {ex.Message}");
        }
    }

    public void Dispose() => _timer?.Dispose();

}



