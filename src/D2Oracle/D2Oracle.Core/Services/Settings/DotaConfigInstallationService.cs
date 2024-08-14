using System.IO.Abstractions;

namespace D2Oracle.Core.Services.Settings;

public class DotaConfigInstallationService : IDotaConfigInstallationService
{
    private readonly IFileSystem fileSystem;
    private const string ConfigFolder = @"game/dota/cfg";
    private const string GsiConfigFolder = "gamestate_integration";
    private const string GsiConfigFileName = "gamestate_integration_d2oracle.cfg";

    public DotaConfigInstallationService(IFileSystem fileSystem)
    {
        this.fileSystem = fileSystem;
    }
    
    public Task InstallConfigAsync(string dotaPath)
    {
        var gsiFolder = this.fileSystem.Path.Combine(dotaPath, ConfigFolder, GsiConfigFolder);
        this.fileSystem.Directory.CreateDirectory(gsiFolder);
        var gsiFilePath = this.fileSystem.Path.Combine(gsiFolder, GsiConfigFileName);
        
        if (this.fileSystem.File.Exists(gsiFilePath))
        {
            return Task.CompletedTask;
        }
        
        return this.fileSystem.File.WriteAllTextAsync(gsiFilePath, Resources.Resources.GsiFileContent);
    }

    public bool IsCorrectDotaPath(string path)
    {
        return this.fileSystem.Directory.Exists(Path.Combine(path, ConfigFolder));
    }
}