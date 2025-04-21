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
ITradeEventWriter tradeEventWriter = new CsvTradeEventWriter(filePath);

IMinuteSummaryWriter minuteSummaryWriter = new MinuteSummaryWriter("logs/minute_summary.csv");
ITradeBuffer tradeBuffer = new TradeBuffer();
ITradeAggregator tradeAggregator = new TradeAggregator();
ITradeAggregatorService tradeAggregatorService = new TradeAggregatorService(tradeBuffer, tradeAggregator, minuteSummaryWriter, TimeSpan.FromMinutes(1));

var logger = new TradeLoggerService(webSocketClient, tradeEventWriter, tradeAggregatorService);

await logger.StartLoggingAsync(symbol);

Console.ReadLine();
