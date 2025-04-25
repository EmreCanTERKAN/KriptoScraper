using KriptoScraper.Application.Interfaces;
using KriptoScraper.Domain.Entities;
using KriptoScraper.Domain.Interfaces;
using KriptoScraper.Interfaces.DataStorage;

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
        _timer = new Timer(async _ =>
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                await AggregateAndWriteAsync(cancellationToken);
            }
        }, null, interval, interval);

        return Task.CompletedTask;
    }

    private async Task AggregateAndWriteAsync(CancellationToken cancellationToken)
    {
        try
        {
            var trades = buffer.Drain();
            if (!trades.Any()) return;

            var summaries = aggregator.Aggregate(trades);

            await writer.WriteBatchAsync(summaries, cancellationToken);

        }
        catch (Exception ex)
        {
            // Loglama servisi varsa burada loglayabilirsin.
            Console.WriteLine($"Hata oluştu: {ex.Message}");
        }
    }

    public void Dispose() =>_timer?.Dispose();

}



