using Binance.Net.Clients;
using KriptoScraper.Application.Interfaces;
using KriptoScraper.Domain.Entities;
using KriptoScraper.Domain.Enums;

namespace KriptoScraper.Infrastructure.WebSocketClients;
public class BinanceWebSocketClient : IBinanceWebSocketClient
{
    public async Task SubscribeToTradeEventsAsync(string symbol, Timeframe timeframe, Func<TradeEvent, Task> onMessage)
    {
        var socketClient = new BinanceSocketClient();

        while (true) // Reconnect döngüsü
        {
            try
            {
                var result = await socketClient.SpotApi.ExchangeData.SubscribeToTradeUpdatesAsync(symbol, async msg =>
                {
                    var data = msg.Data;

                    var tradeEvent = new TradeEvent(
                        Symbol: data.Symbol,
                        Price: data.Price,
                        Quantity: data.Quantity,
                        EventTimeUtc: data.TradeTime.ToUniversalTime(),
                        ReceiveTimeUtc: DateTime.UtcNow,
                        IsBuyerMaker: data.BuyerIsMaker
                    );

                    await onMessage(tradeEvent);
                });

                if (!result.Success)
                {
                    Console.WriteLine($"❌ {symbol} WebSocket bağlantı hatası: {result.Error?.Message}");
                    await Task.Delay(TimeSpan.FromSeconds(10));
                    continue;
                }

                // Bağlantı başarılı, çıkmak yok — bağlantı sonsuz dinliyor
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ {symbol} için WebSocket hatası: {ex.Message}");
                await Task.Delay(TimeSpan.FromSeconds(10)); // Reconnect denemesi için biraz bekle
            }
        }
    }
}

