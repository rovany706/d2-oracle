﻿using System.IO;
using NetCoreAudio;

namespace D2Oracle.Services.Audio;

public class DotaAudioService : IDotaAudioService
{
    private const string ResourcesPath = "Resources";

    private Player player = new();

    public async void PlaySound(DotaSoundType soundType)
    {
        var path = Path.Combine(ResourcesPath, DotaSounds.DotaSoundsFileNames[soundType]);

        await player.Play(path);
    }
}