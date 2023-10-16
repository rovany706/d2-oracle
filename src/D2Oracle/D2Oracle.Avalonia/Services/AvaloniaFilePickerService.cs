using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using D2Oracle.Avalonia.Views;
using D2Oracle.Core.Services;

namespace D2Oracle.Avalonia.Services;

public class AvaloniaFilePickerService : IFilePickerService
{
    private readonly MainWindow mainWindow;

    public AvaloniaFilePickerService(MainWindow mainWindow)
    {
        this.mainWindow = mainWindow;
    }

    public async Task<IEnumerable<string>> OpenFilePickerAsync(string title, bool allowMultiple)
    {
        var files = await this.mainWindow.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = title,
            AllowMultiple = allowMultiple
        });

        return files.Select(x => x.Path.LocalPath);
    }

    public async Task<IEnumerable<string>> OpenFolderPickerAsync(string title, bool allowMultiple)
    {
        var folders = await this.mainWindow.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = title,
            AllowMultiple = allowMultiple
        });

        return folders.Select(x => x.Path.LocalPath);
    }
}