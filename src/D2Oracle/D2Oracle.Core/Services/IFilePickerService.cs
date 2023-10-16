namespace D2Oracle.Core.Services;

public interface IFilePickerService
{
    Task<IEnumerable<string>> OpenFilePickerAsync(string title, bool allowMultiple = false);
    
    Task<IEnumerable<string>> OpenFolderPickerAsync(string title, bool allowMultiple = false);
}