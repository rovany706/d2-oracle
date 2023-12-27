using NetCoreAudio;

namespace D2Oracle.Core.Services.Audio;

public class DotaAudioService : IDotaAudioService
{
    private readonly Player player = new();

    public Task PlaySoundAsync(DotaSoundType soundType)
    {
        var path = Path.Combine(Constants.ResourcesFolderPath, DotaSounds.DotaSoundsFileNames[soundType]);

        return this.player.Play(path);
    }
}