using CsvHelper;
using CsvHelper.Configuration;
using KriptoScraper.Domain.Entities;
using KriptoScraper.Infrastructure.Mappings;
using KriptoScraper.Interfaces.DataStorage;
using System.Globalization;
using System.Text;

namespace KriptoScraper.Infrastructure.Services;
public class CsvTradeEventWriter : ITradeEventWriter
{
    private readonly string _filePath;
    private readonly SemaphoreSlim _lock = new(1, 1); // aynı anda sadece bir iş parçacığına izin verir.
    private bool _headerWritten = false;

    public CsvTradeEventWriter(string filePath)
    {
        _filePath = filePath;

        var directory = Path.GetDirectoryName(_filePath);
        if (directory != null && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    public async Task WriteAsync(TradeEvent tradeEvent)
    {
        await _lock.WaitAsync();
        try
        {
            var fileExists = File.Exists(_filePath);

            using var stream = File.Open(_filePath, FileMode.Append, FileAccess.Write, FileShare.Read);
            using var writer = new StreamWriter(stream, Encoding.UTF8);
            using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = !_headerWritten && !fileExists,
            });

            csv.Context.RegisterClassMap<TradeEventMap>();

            if (!_headerWritten && !fileExists)
            {
                csv.WriteHeader<TradeEvent>();
                await csv.NextRecordAsync();
                _headerWritten = true;
            }

            csv.WriteRecord(tradeEvent);
            await csv.NextRecordAsync();
        }
        finally
        {
            _lock.Release();
        }
    }
}
