using D2Oracle.Core.Services;
using D2Oracle.Core.Services.Settings;

namespace D2Oracle.Core.ViewModels.Settings;

public class DotaConnectionSettingsViewModel : ViewModelBase
{
    private readonly IDotaConfigInstallationService dotaConfigInstallationService;
    private readonly INotificationService notificationService;
    private readonly IFilePickerService filePickerService;

    public DotaConnectionSettingsViewModel(IDotaConfigInstallationService dotaConfigInstallationService,
        INotificationService notificationService, IFilePickerService filePickerService)
    {
        this.dotaConfigInstallationService = dotaConfigInstallationService;
        this.notificationService = notificationService;
        this.filePickerService = filePickerService;
    }

    public async void AutomaticInstall()
    {
        var result = await this.filePickerService.OpenFolderPickerAsync(Resources.Resources.SelectDota2Folder);

        if (result.Any() == false)
        {
            return;
        }
        
        var folder = result.Single();

        if (this.dotaConfigInstallationService.IsCorrectDotaPath(folder) == false)
        {
            this.notificationService.ShowNotification(Resources.Resources.Error, Resources.Resources.IncorrectFolder);

            return;
        }        

        await this.dotaConfigInstallationService.InstallConfigAsync(folder);
        this.notificationService.ShowNotification(Resources.Resources.Success, Resources.Resources.ConfigFileCreated);
    }
}