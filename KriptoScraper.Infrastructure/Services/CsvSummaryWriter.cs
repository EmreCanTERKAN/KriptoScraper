using CsvHelper;
using CsvHelper.Configuration;
using KriptoScraper.Application.Interfaces;
using KriptoScraper.Domain.Interfaces;
using KriptoScraper.Infrastructure.Interfaces;
using System.Globalization;

namespace KriptoScraper.Infrastructure.Services;

public class CsvSummaryWriter<T> : ISummaryWriter<T> where T : ISummary
{
    private readonly ILogFilePathProvider _pathProvider;
    private readonly string _symbol;
    private readonly string _interval;
    private readonly CsvConfiguration _csvConfig;

    public CsvSummaryWriter(ILogFilePathProvider pathProvider, string symbol, string interval)
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

    public async Task WriteAsync(T summary)
    {
        var filePath = _pathProvider.GetPath(_symbol, _interval, "summary");
        var fileExists = File.Exists(filePath);

        using var stream = File.Open(filePath, FileMode.Append, FileAccess.Write, FileShare.Read);
        using var writer = new StreamWriter(stream);
        using var csv = new CsvWriter(writer, _csvConfig);

        if (!fileExists)
        {
            csv.WriteHeader<T>();
            await csv.NextRecordAsync();
        }

        csv.WriteRecord(summary);
        await csv.NextRecordAsync();
    }
}






