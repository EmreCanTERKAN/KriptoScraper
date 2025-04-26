using KriptoScraper.Domain.Interfaces;

namespace KriptoScraper.Worker;

public class Worker(
    ITradeLoggerService tradeloggerService,
    ITradeAggregatorService tradeAggregatorService) : BackgroundService
{

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await tradeAggregatorService.StartAsync(stoppingToken);
        await tradeloggerService.StartLoggingAsync();
    }
}


