using CsvHelper;
using CsvHelper.Configuration;
using KriptoScraper.Application.Entities;
using KriptoScraper.Application.Interfaces;
using KriptoScraper.Application.Mappings;
using KriptoScraper.Domain.Interfaces;
using System.Globalization;
using System.Text;

namespace KriptoScraper.Infrastructure.Writers;
public class CsvTradeEventWriter : ITradeEventWriter
{
    private readonly ILogFilePathProvider _logFilePathProvider;
    private readonly SemaphoreSlim _lock = new(1, 1);

    public CsvTradeEventWriter(ILogFilePathProvider logFilePathProvider)
    {
        _logFilePathProvider = logFilePathProvider;
    }

    public async Task WriteAsync(string symbol, TimeSpan interval, TradeEvent tradeEvent)
    {
        var filePath = _logFilePathProvider.GetPath(symbol, interval, "trades");
        EnsureDirectoryExists(filePath);

        await _lock.WaitAsync();
        try
        {
            var fileExists = File.Exists(filePath);

            await using var stream = File.Open(filePath, FileMode.Append, FileAccess.Write, FileShare.Read);
            await using var writer = new StreamWriter(stream, Encoding.UTF8);
            using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = !fileExists,
                ShouldQuote = _ => true
            });

            csv.Context.RegisterClassMap<TradeEventMap>();

            if (!fileExists)
            {
                csv.WriteHeader<TradeEvent>();
                await csv.NextRecordAsync();
            }

            csv.WriteRecord(tradeEvent);
            await csv.NextRecordAsync();
        }
        finally
        {
            _lock.Release();
        }
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



