using CsvHelper;
using System.Globalization;

namespace KriptoScraper;

public class CsvService
{
    public void WriteToCsv(SolanaLog data, string filePath)
    {
        var fileExists = File.Exists(filePath);

        using var writer = new StreamWriter(filePath, append: true);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        if (!fileExists)
        {
            csv.WriteHeader<SolanaLog>();
            csv.NextRecord();
        }

        csv.WriteRecord(data);
        csv.NextRecord();
    }
}


