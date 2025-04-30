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
        logger.LogInformation("Worker baþlatýldý.");

        try
        {
            var tasks = new List<Task>();

            // TradeLogger baþlat
            tasks.Add(tradeLoggerService.StartLoggingAsync(stoppingToken));

            // TradeAggregator servislerini baþlat
            foreach (var aggregator in aggregatorServices)
            {
                tasks.Add(Task.Run(() => aggregator.StartAsync(stoppingToken), stoppingToken));
            }

            // SummaryProcessing servislerini periyodik olarak çalýþtýr (örn: her 1 dakikada bir)
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
                            logger.LogError(ex, "SummaryProcessingService çalýþtýrýlýrken hata oluþtu.");
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
            logger.LogCritical(ex, "Worker beklenmeyen bir hatayla sonlandý.");
        }
        finally
        {
            logger.LogInformation(" Worker sonlandý.");
        }
    }
}


