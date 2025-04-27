using CsvHelper;
using CsvHelper.Configuration;
using KriptoScraper.Application.Interfaces;
using KriptoScraper.Domain.Interfaces;
using System.Globalization;

namespace KriptoScraper.Infrastructure.Writers;

public class CsvSummaryWriter<T> : ISummaryWriter<T> where T : ISummary
{
    private readonly ILogFilePathProvider _pathProvider;
    private readonly string _symbol;
    private readonly TimeSpan _interval;
    private readonly CsvConfiguration _csvConfig;

    public CsvSummaryWriter(ILogFilePathProvider pathProvider, string symbol, TimeSpan interval)
    {
        _pathProvider = pathProvider;
        _symbol = symbol;
        _interval = interval;

        _csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            ShouldQuote = _ => true
        };
    }

    public async Task WriteBatchAsync(IEnumerable<T> summaries, CancellationToken cancellationToken = default)
    {
        var filePath = _pathProvider.GetPath(_symbol, _interval, "trades");
        EnsureDirectoryExists(filePath);

        var summaryList = summaries.ToList();

        if (!summaryList.Any())
            return;

        var fileExist = File.Exists(filePath);

        using var stream = File.Open(filePath, FileMode.Append, FileAccess.Write, FileShare.Read);
        using var writer = new StreamWriter(stream);
        using var csv = new CsvWriter(writer, _csvConfig);

        if (!fileExist)
        {
            csv.WriteHeader<T>();
            await csv.NextRecordAsync();
        }

        foreach (var summary in summaryList)
        {
            cancellationToken.ThrowIfCancellationRequested();
            csv.WriteRecord(summary);
            await csv.NextRecordAsync();
        }

        await writer.FlushAsync();
        stream.Flush(true);

    }

    private static void EnsureDirectoryExists(string filePath)
    {
        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }
}






