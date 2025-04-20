using CsvHelper;
using KriptoScraper.Interfaces;
using KriptoScraper.LogInfos;
using System.Globalization;

namespace KriptoScraper.Services;

public class CsvService : ICsvService
{
    public async Task WriteToCsvAsync<T>(T data, string filePath)
    {
        var fileExists = File.Exists(filePath);

        await using var stream = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.Read, bufferSize: 4096, useAsync: true);
        await using var writer = new StreamWriter(stream);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        if (!fileExists)
        {
            csv.WriteHeader<T>();
            await csv.NextRecordAsync();
        }
        
        csv.WriteRecord(data);
        await csv.NextRecordAsync();
    }
}


