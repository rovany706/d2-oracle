namespace D2Oracle.Core.Services.Audio;

public class MockDotaAudioService : IDotaAudioService
{
    public Task PlaySound(DotaSoundType soundType)
    {
        return Task.CompletedTask;
    }
}