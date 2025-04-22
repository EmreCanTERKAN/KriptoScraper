using Binance.Net.Clients;
using KriptoScraper.Domain.Entities;
using KriptoScraper.Domain.Interfaces;

namespace KriptoScraper.Infrastructure.Services;
public class BinanceWebSocketClient : IBinanceWebSocketClient
{
    public async Task SubscribeToTradeEventsAsync(string symbol, Func<TradeEvent, Task> onMessage)
    {
        var socketClient = new BinanceSocketClient();

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
            Console.WriteLine("❌ WebSocket aboneliği başarısız: " + result.Error?.Message);
        }
    }
}
