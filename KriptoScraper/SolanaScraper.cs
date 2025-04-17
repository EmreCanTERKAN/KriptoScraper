using KriptoScraper;
using Microsoft.Playwright;
using System.Globalization;

public class SolanaScraper
{
    public static async Task GetSolanaPrice()
    {
        using var playWright = await Playwright.CreateAsync();
        await using var browser = await playWright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });

        var page = await browser.NewPageAsync();
        await page.GotoAsync("https://www.mexc.co/en-TR/exchange/SOL_USDT");

        await page.WaitForSelectorAsync("span.headline_ellipsisContent__dDkIb");

        var priceElement = await page.QuerySelectorAsync("span.headline_ellipsisContent__dDkIb");

        var priceText = await priceElement!.InnerTextAsync();

        var now = DateTime.Now;

        Console.WriteLine($"{now:yyyy-MM-dd HH:mm:ss} - 📈 Solana/USDT Fiyatı (MEXC): {priceText} $");

        if (decimal.TryParse(priceText, NumberStyles.Any,CultureInfo.InvariantCulture, out var price))
        {
            var log = new SolanaLog
            {
                Timestamp = now,
                Price = price
            };

            //var csvService = new CsvService();

            //var folder = "logs";
            //Directory.CreateDirectory(folder); // klasörü oluşturur (varsa dokunmaz)

            //var filePath = Path.Combine(folder, "solana_log.csv");
            //csvService.WriteToCsv(new List<SolanaLog> { log }, filePath);

        }
    }
}
