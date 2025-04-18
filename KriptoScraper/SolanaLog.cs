using CsvHelper.Configuration.Attributes;

namespace KriptoScraper;
public class SolanaLog
{
    [Name("Zaman")]
    public DateTime Timestamp { get; set; }
    [Name("Fiyat")]
    public decimal Price { get; set; }
}
