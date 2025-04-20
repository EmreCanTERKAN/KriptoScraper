using KriptoScraper;
using KriptoScraper.Interfaces.Tracking;
using KriptoScraper.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices(DependencyInjection.ConfigureServices)
    .Build();

Console.WriteLine("🔁 Kripto Para takip otomasyonu başlatıldı.");

var solanaTracker = host.Services.GetRequiredService<IBinanceTracker<SolanaLog>>();
var ethereumTracker = host.Services.GetRequiredService<IBinanceTracker<EthereumLog>>();

await solanaTracker.StartAsync();
await ethereumTracker.StartAsync();
Console.ReadLine();