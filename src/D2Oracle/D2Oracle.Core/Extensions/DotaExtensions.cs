namespace D2Oracle.Core.Extensions;

public static class DotaExtensions
{
    public static string FormatAsDotaTime(this TimeSpan? time)
    {
        return time is null ? string.Empty : FormatAsDotaTime(time.Value);
    }
    
    public static string FormatAsDotaTime(this TimeSpan time)
    {
        const string dotaTimeFormat = @"{0}:{1}";
        var totalMinutes = (int) Math.Abs(time.TotalMinutes);
        var sign = time < TimeSpan.Zero ? "-" : "";

        return $"{sign}{string.Format(dotaTimeFormat, totalMinutes, time.ToString("ss"))}";
    }
}