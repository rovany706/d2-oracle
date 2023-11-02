using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using D2Oracle.Core.Services.Settings;

namespace D2Oracle.Core.Tests.Services.Settings;

public class DotaConfigInstallationServiceTests
{
    [Test]
    public void IsCorrectDotaPath_WhenDirectoryNotExist_ReturnFalse()
    {
        var fileSystem = new MockFileSystem();
        var path = fileSystem.CurrentDirectory().SubDirectory("dota 2 beta").FullName;

        var sut = new DotaConfigInstallationService(fileSystem);

        Assert.That(sut.IsCorrectDotaPath(path), Is.False);
    }

    [Test]
    public void IsCorrectDotaPath_WhenDirectoryExist_ReturnTrue()
    {
        var fileSystem = new MockFileSystem();
        var path = fileSystem.CurrentDirectory().SubDirectory("dota 2 beta");
        path.SubDirectory("game", "dota", "cfg").Create();

        var sut = new DotaConfigInstallationService(fileSystem);

        Assert.That(sut.IsCorrectDotaPath(path.FullName), Is.True);
    }

    [Test]
    public async Task InstallConfigAsync_WhenConfigFileExists_DoNotRewriteConfigFile()
    {
        const string expected = "test";
        var fileSystem = new MockFileSystem();
        var directory = fileSystem.CurrentDirectory()
            .SubDirectory("dota 2 beta", "game", "dota", "cfg", "gamestate_integration");
        directory.Create();
        var configFile = directory.File("gamestate_integration_d2oracle.cfg");

        await using (var writer = configFile.CreateText())
        {
            await writer.WriteAsync(expected);
        }

        var sut = new DotaConfigInstallationService(fileSystem);

        await sut.InstallConfigAsync(fileSystem.CurrentDirectory().SubDirectory("dota 2 beta").FullName);

        Assert.That(await fileSystem.File.ReadAllTextAsync(configFile.FullName), Is.EqualTo(expected));
    }

    [Test]
    public async Task InstallConfigAsync_WhenConfigFileNotExist_CreateConfigFile()
    {
        var expected = Resources.Resources.GsiFileContent;
        var fileSystem = new MockFileSystem();
        var directory = fileSystem.CurrentDirectory()
            .SubDirectory("dota 2 beta", "game", "dota", "cfg");
        directory.Create();

        var sut = new DotaConfigInstallationService(fileSystem);

        await sut.InstallConfigAsync(fileSystem.CurrentDirectory().SubDirectory("dota 2 beta").FullName);

        var configFile = directory.File("gamestate_integration", "gamestate_integration_d2oracle.cfg");
        Assert.That(await fileSystem.File.ReadAllTextAsync(configFile.FullName), Is.EqualTo(expected));
    }
}