using Binance.Net.Enums;
using Binance.Net.Interfaces;
using Binance.Net.Interfaces.Clients;
using CryptoExchange.Net.Objects.Sockets;
using KriptoScraper.Domain.Entities;
using KriptoScraper.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace KriptoScraper.Infrastructure.WebSocketClients;
public class BinanceWebSocketClient(
    IBinanceSocketClient socketClient,
    ILogger<BinanceWebSocketClient> logger) : IBinanceWebSocketClient
{

    public async Task SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, IEnumerable<KlineInterval> intervals, Action<DataEvent<IBinanceStreamKlineData>> onMessage, CancellationToken ct = default)
    {
        try
        {
            var result = await socketClient.SpotApi.ExchangeData.SubscribeToKlineUpdatesAsync(
                symbols,
                intervals,
                onMessage,
                ct
            );

            if (!result.Success)
            {
                logger.LogError($"WebSocket bağlantı hatası: {result.Error?.Message}");
                return;
            }

            logger.LogInformation("✅ WebSocket bağlantısı kuruldu.");
        }
        catch (Exception ex)
        {
            logger.LogError($"WebSocket hata: {ex.Message}");
            throw; // Rethrow or handle based on your needs
        }
    }

    public async Task SubscribeToKline1mAsync(string symbol, Func<KlineEvent, Task> onKlineReceived)
    {
        await SubscribeToKlineUpdatesAsync(new[] { symbol }, new[] { KlineInterval.OneMinute }, async msg =>
        {
            var klineData = msg.Data;

            if (klineData?.Data?.Final == true)
            {
                var kline = klineData.Data;
                var klineEvent = new KlineEvent(
                    Symbol: klineData.Symbol,
                    OpenTime: kline.OpenTime,
                    Open: kline.OpenPrice,
                    High: kline.HighPrice,
                    Low: kline.LowPrice,
                    Close: kline.ClosePrice,
                    Volume: kline.Volume,
                    CloseTime: kline.CloseTime,
                    NumberOfTrades: kline.TradeCount
                );

                await onKlineReceived(klineEvent);
            }
        });
    }
}


