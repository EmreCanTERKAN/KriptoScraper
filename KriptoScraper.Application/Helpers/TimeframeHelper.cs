namespace KriptoScraper.Application.Helpers;
public static class TimeframeHelper
{
    public static TimeSpan ToTimeSpan(this string timeframe)
    {
        return timeframe switch
        {
            "1m" => TimeSpan.FromMinutes(1),
            "5m" => TimeSpan.FromMinutes(5),
            "15m" => TimeSpan.FromMinutes(15),
            "1h" => TimeSpan.FromHours(1),
            _ => throw new ArgumentException($"Unsupported timeframe: {timeframe}")
        };
    }
}
