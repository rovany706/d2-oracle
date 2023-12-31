﻿namespace D2Oracle.Core.Extensions;

public static class DoubleExtensions
{
    public static double ClosestMultipleCeil(this double value, double multiple)
    {
        if (multiple == 0)
        {
            return 0;
        }
        
        if (multiple > value)
        {
            return multiple;
        }

        return multiple * Math.Ceiling(value / multiple);
    }
    
    public static double ClosestMultipleFloor(this double value, double multiple)
    {
        if (multiple == 0)
        {
            return 0;
        }
        
        return multiple * Math.Floor(value / multiple);
    }
}