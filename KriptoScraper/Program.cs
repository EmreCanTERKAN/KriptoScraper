using KriptoScraper;
using KriptoScraper.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices(DependencyInjection.ConfigureServices)
    .Build();

Console.WriteLine("🔁 Solana takip otomasyonu başlatıldı.");

var tracker = host.Services.GetRequiredService<IBinanceSolanaTracker>();

await tracker.StartAsync();

Console.ReadLine();