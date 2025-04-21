using KriptoScraper.Interfaces.DataStorage;
using KriptoScraper.Models;

namespace KriptoScraper.Services.DataStorage;
public class MinuteSummaryWriter : IMinuteSummaryWriter
{
    private readonly string _filePath;

    public MinuteSummaryWriter(string fileName)
    {
        // 'logs' klasöründe dosya yolunu oluştur
        var logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "logs");

        // Eğer logs klasörü yoksa, oluştur
        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }

        // Dosya yolunu belirle
        _filePath = Path.Combine(logDirectory, fileName);
    }

    public async Task WriteAsync(MinuteSummary summary)
    {
        var line = $"{summary.Minute:o},{summary.Open},{summary.High},{summary.Low},{summary.Close},{summary.Volume},{summary.TradeCount},{summary.BuyerMakerRatio}";

        // Dosyaya yazma işlemi
        using (var writer = new StreamWriter(_filePath, append: true))
        {
            await writer.WriteLineAsync(line);
        }
    }
}



