using CsvHelper.Configuration.Attributes;

namespace KriptoScraper.Models;
public abstract class CryptoLog
{
    [Name("Zaman")]
    public DateTime Timestamp { get; set; }

    [Name("Fiyat")]
    public decimal Price { get; set; }

    public override string ToString()
    {
        return $"{Timestamp:dd-MM-yyyy HH:mm:ss} - {Price} USD";
    }
}
