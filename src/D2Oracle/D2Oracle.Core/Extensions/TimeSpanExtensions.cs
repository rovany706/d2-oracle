namespace D2Oracle.Core.Extensions;

public static class TimeSpanExtensions
{
    public static bool EqualsWithPrecision(this TimeSpan? value, TimeSpan? otherValue, TimeSpan delta)
    {
        return value is not null && otherValue is not null && EqualsWithPrecision(value.Value, otherValue.Value, delta);
    }
    
    public static bool EqualsWithPrecision(this TimeSpan value, TimeSpan otherValue, TimeSpan delta)
    {
        return (value - otherValue).Duration() <= delta;
    }
}