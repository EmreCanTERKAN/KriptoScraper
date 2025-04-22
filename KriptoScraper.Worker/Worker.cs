using KriptoScraper.Application.Services;

namespace KriptoScraper.Worker;

public class Worker : BackgroundService
{
    private readonly TradeLoggerService _loggerService;
    private readonly string _symbol;
    public Worker(TradeLoggerService loggerService, IConfiguration config)
    {
        _loggerService = loggerService;
        _symbol = config.GetValue<string>("TradeSettings:Symbol")!;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _loggerService.StartLoggingAsync(_symbol);
    }
}


