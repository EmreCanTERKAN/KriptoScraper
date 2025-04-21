using KriptoScraper.Interfaces.DataStorage;
using KriptoScraper.Models;

public class TradeAggregatorService(
   ITradeBuffer buffer,
   ITradeAggregator aggregator,
   IMinuteSummaryWriter writer,
   TimeSpan interval) : ITradeAggregatorService
{ 
    private Timer? _timer;                   

    public void AddTrade(TradeEvent tradeEvent)
    {
        buffer.Add(tradeEvent);  
    }

    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        _timer = new Timer(async _ => await AggregateAndWriteAsync(), null, interval, interval);
        return Task.CompletedTask;
    }

    private async Task AggregateAndWriteAsync()
    {
        var trades = buffer.Drain();

        if (!trades.Any()) return;

        var summaries = aggregator.AggregateToMinuteSummary(trades);

        foreach (var summary in summaries)
        {
            await writer.WriteAsync(summary);
        }
    }
}
