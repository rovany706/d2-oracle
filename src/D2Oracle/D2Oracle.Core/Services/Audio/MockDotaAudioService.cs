namespace D2Oracle.Core.Services.Audio;

public class MockDotaAudioService : IDotaAudioService
{
    public Task PlaySoundAsync(DotaSoundType soundType)
    {
        return Task.CompletedTask;
    }
}