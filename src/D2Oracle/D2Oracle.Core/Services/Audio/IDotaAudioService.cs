namespace D2Oracle.Core.Services.Audio;

public interface IDotaAudioService
{
    Task PlaySoundAsync(DotaSoundType soundType);
}