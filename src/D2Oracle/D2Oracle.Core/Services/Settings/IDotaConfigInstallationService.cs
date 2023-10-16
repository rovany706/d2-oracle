namespace D2Oracle.Core.Services.Settings;

public interface IDotaConfigInstallationService
{
    Task InstallConfigAsync(string dotaPath);

    bool IsCorrectDotaPath(string path);
}