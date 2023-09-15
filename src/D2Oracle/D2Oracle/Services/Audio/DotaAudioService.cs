using System.IO;
using NetCoreAudio;

namespace D2Oracle.Services.Audio;

public class DotaAudioService : IDotaAudioService
{
    private const string ResourcePath = "Resource";

    private Player player = new();

    public async void PlaySound(DotaSoundType soundType)
    {
        var path = Path.Combine(ResourcePath, DotaSounds.DotaSoundsFileNames[soundType]);

        await player.Play(path);
    }
}