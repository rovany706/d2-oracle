namespace D2Oracle.Core.Extensions;

public static class TimeSpanExtensions
{
    public static bool EqualsWithPrecision(this TimeSpan? value, TimeSpan? otherValue, TimeSpan delta)
    {
        if (value is null && otherValue is null)
        {
            return true;
        }

        if (value is null || otherValue is null)
        {
            return false;
        }
        
        return EqualsWithPrecision(value.Value, otherValue.Value, delta);
    }
    
    public static bool EqualsWithPrecision(this TimeSpan value, TimeSpan otherValue, TimeSpan delta)
    {
        return (value - otherValue).Duration() <= delta;
    }
}