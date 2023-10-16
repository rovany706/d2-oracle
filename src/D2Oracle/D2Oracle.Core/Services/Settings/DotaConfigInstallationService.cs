namespace D2Oracle.Core.Services.Settings;

public class DotaConfigInstallationService : IDotaConfigInstallationService
{
    private const string ConfigFolder = @"game\dota\cfg";
    private const string GsiConfigFolder = "gamestate_integration";
    private const string GsiConfigFileName = "gamestate_integration_d2oracle.cfg";
    
    public async Task InstallConfigAsync(string dotaPath)
    {
        var gsiFolder = Path.Combine(dotaPath, ConfigFolder, GsiConfigFolder);
        Directory.CreateDirectory(gsiFolder);
        var gsiFilePath = Path.Combine(gsiFolder, GsiConfigFileName);
        
        if (File.Exists(gsiFilePath))
        {
            return;
        }
        
        await File.WriteAllTextAsync(gsiFilePath, Resources.Resources.GsiFileContent);
    }

    public bool IsCorrectDotaPath(string path)
    {
        return Directory.Exists(Path.Combine(path, ConfigFolder));
    }
}