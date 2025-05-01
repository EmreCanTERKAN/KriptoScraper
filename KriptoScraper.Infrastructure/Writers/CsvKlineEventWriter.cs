using CsvHelper;
using CsvHelper.Configuration;
using KriptoScraper.Application.Helpers;
using KriptoScraper.Application.Mappings;
using KriptoScraper.Domain.Entities;
using KriptoScraper.Domain.Interfaces;
using System.Globalization;
using System.Text;

namespace KriptoScraper.Infrastructure.Writers;
public class CsvKlineEventWriter : IKlineEventWriter
{
    private readonly ILogFilePathProvider _logFilePathProvider;
    private readonly SemaphoreSlim _lock = new(1, 1);

    public CsvKlineEventWriter(ILogFilePathProvider logFilePathProvider)
    {
        _logFilePathProvider = logFilePathProvider;
    }

    public async Task WriteAsync(string symbol, KlineEvent klineEvent)
    {
        var filePath = _logFilePathProvider.GetRawKlinesPath(symbol);
        FileHelper.EnsureDirectoryExists(filePath);

        await _lock.WaitAsync();
        try
        {
            var fileExists = File.Exists(filePath);

            await using var stream = File.Open(filePath, FileMode.Append, FileAccess.Write, FileShare.Read);
            await using var writer = new StreamWriter(stream, Encoding.UTF8);
            using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = !fileExists,
                ShouldQuote = _ => true,
                PrepareHeaderForMatch = args => args.Header.ToLower()
            });

            csv.Context.RegisterClassMap<KlineEventMap >();

            if (stream.Length == 0)
            {
                csv.WriteHeader<KlineEvent>();
                await csv.NextRecordAsync();
            }

            csv.WriteRecord(klineEvent);
            await csv.NextRecordAsync();
        }
        finally
        {
            _lock.Release();
        }
    }
}



