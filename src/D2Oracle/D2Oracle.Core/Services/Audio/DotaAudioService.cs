using NetCoreAudio;

namespace D2Oracle.Core.Services.Audio;

public class DotaAudioService : IDotaAudioService
{
    private readonly Player player = new();

    public Task PlaySound(DotaSoundType soundType)
    {
        var path = Path.Combine(Constants.ResourcesFolderPath, DotaSounds.DotaSoundsFileNames[soundType]);

        return player.Play(path);
    }
}