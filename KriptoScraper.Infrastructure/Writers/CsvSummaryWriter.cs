using CsvHelper;
using CsvHelper.Configuration;
using KriptoScraper.Application.Helpers;
using KriptoScraper.Application.Interfaces;
using KriptoScraper.Domain.Interfaces;
using System.Globalization;

namespace KriptoScraper.Infrastructure.Writers;

public class CsvSummaryWriter<T> : ISummaryWriter<T> where T : ISummary
{
    private readonly ILogFilePathProvider _pathProvider;
    public string Symbol { get; }   
    public TimeSpan Interval { get; }  
    private readonly CsvConfiguration _csvConfig;

    public CsvSummaryWriter(ILogFilePathProvider pathProvider, string symbol, TimeSpan interval)
    {
        _pathProvider = pathProvider;
        Symbol = symbol;
        Interval = interval;

        _csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            ShouldQuote = _ => true,
            PrepareHeaderForMatch = args => args.Header.ToLower()
        };
    }

    public async Task WriteBatchAsync(IEnumerable<T> summaries, CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.Date;
        var filePath = _pathProvider.GetSummaryFilePath(Symbol, Interval, today);
        FileHelper.EnsureDirectoryExists(filePath);

        var summaryList = summaries.ToList();

        if (!summaryList.Any())
            return;

        var existingPeriods = new HashSet<DateTime>();
        if (File.Exists(filePath))
        {
            using var reader = new StreamReader(filePath);
            using var csvReader = new CsvReader(reader, _csvConfig);
            var records = csvReader.GetRecords<T>();
            foreach (var record in records)
            {
                existingPeriods.Add(record.Period); // ISummary interface’ine Period ekli zaten
            }
        }

        var newSummaries = summaryList
            .Where(s => !existingPeriods.Contains(s.Period))
            .OrderBy(s => s.Period)
            .ToList();

        if (!newSummaries.Any())
            return;

        using var stream = File.Open(filePath, FileMode.Append, FileAccess.Write, FileShare.Read);
        using var writer = new StreamWriter(stream);
        using var csv = new CsvWriter(writer, _csvConfig);

        if (stream.Length == 0)
        {
            csv.WriteHeader<T>();
            await csv.NextRecordAsync();
        }

        foreach (var summary in newSummaries)
        {
            cancellationToken.ThrowIfCancellationRequested();
            csv.WriteRecord(summary);
            await csv.NextRecordAsync();
        }

        await writer.FlushAsync();
        stream.Flush(true);
    }
}






