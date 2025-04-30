using KriptoScraper.Application.Entities;
using KriptoScraper.Application.Helpers;
using KriptoScraper.Application.Interfaces;
using KriptoScraper.Domain.Enums;
using KriptoScraper.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace KriptoScraper.Application.Services;
public sealed class SummaryProcessingService(
    ISummaryReader<MinuteSummary> summaryReader,
    ISummaryAggregator<MinuteSummary> multiMinuteAggregator,
    ISummaryWriter<MinuteSummary> summaryWriter,
    ILogFilePathProvider pathProvider,
    string symbol,
    Timeframe targetTimeframe,
    ILogger<SummaryProcessingService> logger) : ISummaryProcessingService
{
    private readonly string _filePath = pathProvider.GetSummaryFilePath(symbol, TimeSpan.FromMinutes(1), DateTime.UtcNow);
    private readonly TimeSpan _targetTimeSpan = targetTimeframe.ToTimeSpan();

    public async Task ProcessAsync(CancellationToken cancellationToken)
    {
        try
        {
            var requiredLastLines = (int)(_targetTimeSpan.TotalMinutes);

            var summaries = await summaryReader.ReadLastNSummariesAsync(_filePath, requiredLastLines, cancellationToken);

            if (!summaries.Any())
            {
                logger.LogWarning("⚠️ No summaries found in file {FilePath}.", _filePath);
                return;
            }

            var aggregatedSummaries = multiMinuteAggregator.Aggregate(summaries, _targetTimeSpan);

            await summaryWriter.WriteBatchAsync(aggregatedSummaries, cancellationToken);

            logger.LogInformation("✅ Summary processing completed for {FilePath}.", _filePath);
        }
        catch (OperationCanceledException)
        {
            logger.LogWarning("🛑 Summary processing cancelled for {FilePath}.", _filePath);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "🔥 Error processing summary for {FilePath}.", _filePath);
        }
    }
}

