using KriptoScraper.Application.Interfaces;
using KriptoScraper.Domain.Interfaces;

namespace KriptoScraper.Worker;

public class Worker(
    ITradeLoggerService tradeLoggerService,
    IEnumerable<ITradeAggregatorService> aggregatorServices,
    IEnumerable<ISummaryProcessingService> summaryProcessingServices,
    ILogger<Worker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Worker ba�lat�ld�.");

        try
        {
            var tasks = new List<Task>();

            // TradeLogger ba�lat
            tasks.Add(tradeLoggerService.StartLoggingAsync(stoppingToken));

            // TradeAggregator servislerini ba�lat
            foreach (var aggregator in aggregatorServices)
            {
                tasks.Add(Task.Run(() => aggregator.StartAsync(stoppingToken), stoppingToken));
            }

            // SummaryProcessing servislerini periyodik olarak �al��t�r (�rn: her 1 dakikada bir)
            tasks.Add(Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    foreach (var processor in summaryProcessingServices)
                    {
                        try
                        {
                            await processor.ProcessAsync(stoppingToken);
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "SummaryProcessingService �al��t�r�l�rken hata olu�tu.");
                        }
                    }

                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
            }, stoppingToken));

            await Task.WhenAll(tasks);
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


