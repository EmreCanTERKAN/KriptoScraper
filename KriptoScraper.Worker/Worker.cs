using KriptoScraper.Application.Interfaces;

namespace KriptoScraper.Worker;

public class Worker(
    IKlineLoggerService tradeLoggerService,
    ILogger<Worker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Worker ba�lat�ld�.");

        try
        {
            await tradeLoggerService.StartLoggingAsync(stoppingToken);
        }
        catch (OperationCanceledException)
        {
            logger.LogWarning(" Worker durduruldu (cancellation requested).");
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Worker beklenmeyen bir hatayla sonland�.");
        }
        finally
        {
            logger.LogInformation(" Worker sonland�.");
        }
    }
}


