using System;

namespace D2Oracle.Extensions;

public static class DotaExtensions
{
    public static string FormatAsDotaTime(this TimeSpan? time)
    {
        return time is null ? string.Empty : FormatAsDotaTime(time.Value);
    }
    
    public static string FormatAsDotaTime(this TimeSpan time)
    {
        const string dotaTimeFormat = @"mm\:ss";
        var sign = time < TimeSpan.Zero ? "-" : "";

        return $"{sign}{time.ToString(dotaTimeFormat)}";
    }
}