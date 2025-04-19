using KriptoScraper.BinanceTrackers;
using KriptoScraper.Services;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("🔁 Solana takip otomasyonu başlatıldı.");
var logger = new CsvConsoleLogger();
var monitor = new PriceMonitorService(logger);
var tracker = new BinanceSolanaTracker(monitor, "SOLUSDT");

await tracker.StartAsync();

Console.ReadLine();