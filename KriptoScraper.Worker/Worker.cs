using KriptoScraper.Application.Interfaces;

namespace KriptoScraper.Worker;

public class Worker(
    IKlineLoggerService klineLoggerService,
    ILogger<Worker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Worker baþlatýldý.");

        try
        {
            await klineLoggerService.StartLoggingAsync(stoppingToken);
        }
        catch (OperationCanceledException)
        {
            logger.LogWarning(" Worker durduruldu (cancellation requested).");
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Worker beklenmeyen bir hatayla sonlandý.");
        }
        finally
        {
            logger.LogInformation(" Worker sonlandý.");
        }
    }
}


