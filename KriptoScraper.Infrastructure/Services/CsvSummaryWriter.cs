using CsvHelper;
using CsvHelper.Configuration;
using KriptoScraper.Application.Interfaces;
using KriptoScraper.Domain.Interfaces;
using System.Globalization;

namespace KriptoScraper.Infrastructure.Services;

public class CsvSummaryWriter<T> : ISummaryWriter<T>
    where T : ISummary
{
    private readonly string _filePath;
    private readonly CsvConfiguration _csvConfig;

    public CsvSummaryWriter(string fileName)
    {
        var logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "logs");

        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }

        var cleanFileName = Path.GetFileName(fileName);
        _filePath = Path.Combine(logDirectory, cleanFileName);

        _csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            ShouldQuote = args => true
        };
    }

    public async Task WriteAsync(T summary)
    {
        var fileExists = File.Exists(_filePath);

        using var stream = File.Open(_filePath, FileMode.Append, FileAccess.Write, FileShare.Read);
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





