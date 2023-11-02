using System.IO.Abstractions.TestingHelpers;
using D2Oracle.Core.Services.Settings;

namespace D2Oracle.Core.Tests.Services.Settings;

public class DotaConfigInstallationServiceTests
{
    [Test]
    public void IsCorrectDotaPath_WhenDirectoryNotExist_ReturnFalse()
    {
        var fileSystem = new MockFileSystem();

        var sut = new DotaConfigInstallationService(fileSystem);
        
        Assert.That(sut.IsCorrectDotaPath(@"c:\dota 2 beta"), Is.False);
    }
    
    [Test]
    public void IsCorrectDotaPath_WhenDirectoryExist_ReturnTrue()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.Directory.CreateDirectory(@"c:\dota 2 beta\game\dota\cfg");

        var sut = new DotaConfigInstallationService(fileSystem);
        
        Assert.That(sut.IsCorrectDotaPath(@"c:\dota 2 beta"), Is.True);
    }

    [Test]
    public async Task InstallConfigAsync_WhenConfigFileExists_DoNotRewriteConfigFile()
    {
        const string expected = "test";
        const string configFilePath = @"c:\dota 2 beta\game\dota\cfg\gamestate_integration\gamestate_integration_d2oracle.cfg";
        
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            {
                configFilePath,
                new MockFileData(expected)
            }
        });

        var sut = new DotaConfigInstallationService(fileSystem);

        await sut.InstallConfigAsync(@"c:\dota 2 beta");
        
        Assert.That(await fileSystem.File.ReadAllTextAsync(configFilePath), Is.EqualTo(expected));
    }
    
    [Test]
    public async Task InstallConfigAsync_WhenConfigFileNotExist_CreateConfigFile()
    {
        var expected = Resources.Resources.GsiFileContent;
        const string configFilePath = @"c:\dota 2 beta\game\dota\cfg\gamestate_integration\gamestate_integration_d2oracle.cfg";
        
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            {
                @"c:\dota 2 beta\game\dota\cfg\",
                new MockDirectoryData()
            }
        });

        var sut = new DotaConfigInstallationService(fileSystem);

        await sut.InstallConfigAsync(@"c:\dota 2 beta");
        
        Assert.That(await fileSystem.File.ReadAllTextAsync(configFilePath), Is.EqualTo(expected));
    }
    
    [Test]
    public async Task InstallConfigAsync_WhenConfigFileNotExist_CreateConfigDirectory()
    {
        const string configFileDirectory = @"c:\dota 2 beta\game\dota\cfg\gamestate_integration";
        
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            {
                @"c:\dota 2 beta\game\dota\cfg\",
                new MockDirectoryData()
            }
        });

        var sut = new DotaConfigInstallationService(fileSystem);

        await sut.InstallConfigAsync(@"c:\dota 2 beta");
        
        Assert.That(fileSystem.Directory.Exists(configFileDirectory), Is.True);
    }
}