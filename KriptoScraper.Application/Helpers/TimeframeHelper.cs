using KriptoScraper.Domain.Enums;

namespace KriptoScraper.Application.Helpers;
public static class TimeframeHelper
{
    public static TimeSpan ToTimeSpan(this Timeframe tf) => tf switch
    {
        Timeframe.OneMinute => TimeSpan.FromMinutes(1),
        Timeframe.FiveMinutes => TimeSpan.FromMinutes(5),
        Timeframe.FifteenMinutes => TimeSpan.FromMinutes(15),
        Timeframe.OneHour => TimeSpan.FromHours(1),
        _ => throw new ArgumentOutOfRangeException(nameof(tf), tf, null)
    };
}

