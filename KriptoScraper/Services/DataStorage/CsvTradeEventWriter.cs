using CsvHelper;
using CsvHelper.Configuration;
using KriptoScraper.Dtos;
using KriptoScraper.Interfaces.DataStorage;
using System.Globalization;
using System.Text;

namespace KriptoScraper.Services.DataStorage;
public class CsvTradeEventWriter : ITradeEventWriter
{
    private readonly string _filePath;
    private readonly SemaphoreSlim _lock = new(1, 1);
    private bool _headerWritten = false;

    public CsvTradeEventWriter(string filePath)
    {
        _filePath = filePath;
    }

    public async Task WriteAsync(TradeEventDto tradeEvent)
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

            if (!_headerWritten && !fileExists)
            {
                csv.WriteHeader<TradeEventDto>();
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
