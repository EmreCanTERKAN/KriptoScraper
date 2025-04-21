using KriptoScraper.Interfaces.DataStorage;
using KriptoScraper.Interfaces.Tracking;
using KriptoScraper.Services.DataStorage;
using KriptoScraper.Services.Tracking;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var symbol = "BTCUSDT";

var directory = "logs";

Directory.CreateDirectory(directory);

var filePath = Path.Combine(directory, "btc_trades.csv");

IBinanceWebSocketClient webSocketClient = new BinanceWebSocketClient();
ITradeEventWriter writer = new CsvTradeEventWriter(filePath);

var logger = new TradeLoggerService(webSocketClient, writer);

await logger.StartLoggingAsync(symbol);

Console.ReadLine();