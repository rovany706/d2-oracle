namespace D2Oracle.Core.Services.Audio;

public enum DotaSoundType
{
    MinRoshanTime,
    MaxRoshanTime
}

public static class DotaSounds
{
    public static readonly Dictionary<DotaSoundType, string> DotaSoundsFileNames = new()
    {
        { DotaSoundType.MinRoshanTime, "8min.mp3" },
        { DotaSoundType.MaxRoshanTime, "11min.mp3" }
    };
}