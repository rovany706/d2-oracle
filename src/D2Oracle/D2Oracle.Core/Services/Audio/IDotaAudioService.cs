namespace D2Oracle.Core.Services.Audio;

public interface IDotaAudioService
{
    Task PlaySound(DotaSoundType soundType);
}