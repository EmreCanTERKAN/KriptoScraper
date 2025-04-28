namespace KriptoScraper.Application.Helpers;
public static class TimeHelper
{
    public static DateTime TruncateToMinute(DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day,
                    dateTime.Hour, dateTime.Minute, 0, DateTimeKind.Utc);
    }
}
