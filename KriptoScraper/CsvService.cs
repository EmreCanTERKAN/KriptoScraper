using CsvHelper;
using System.Globalization;

namespace KriptoScraper;

public class CsvService
{
    public void WriteToCsv(List<SolanaLog> dataList, string filePath)
    {
        using var writer = new StreamWriter(filePath, append: true);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        // Başlık yazılmıyor
        csv.WriteRecords(dataList);
    }
}


