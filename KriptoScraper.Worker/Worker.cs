using KriptoScraper.Application.Services;
using KriptoScraper.Interfaces.DataStorage;

namespace KriptoScraper.Worker;

public class Worker(
    TradeLoggerService loggerService,
    ITradeAggregatorService tradeAggregatorService,
    IConfiguration config) : BackgroundService
{
    private readonly string _symbol = config.GetValue<string>("TradeSettings:Symbol")!;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await tradeAggregatorService.StartAsync(stoppingToken);
        await loggerService.StartLoggingAsync(_symbol);
    }
}


