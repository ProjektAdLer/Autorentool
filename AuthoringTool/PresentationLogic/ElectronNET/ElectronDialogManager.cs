using ElectronNET.API;
using ElectronNET.API.Entities;

namespace AuthoringTool.PresentationLogic.ElectronNET;

public class ElectronDialogManager : IElectronDialogManager
{
    private BrowserWindow BrowserWindow => Electron.WindowManager.BrowserWindows.First();

    /// <inheritdoc cref="IElectronDialogManager.ShowSaveAsDialog"/>
    public async Task<string> ShowSaveAsDialog(string title, string? defaultPath = null, IEnumerable<FileFilterProxy>? fileFilters = null)
    {
        var mainWindow = BrowserWindow;
        var options = new SaveDialogOptions
        {
            Title = title,
            //if default path is null use mydocuments special folder as fallback
            DefaultPath = defaultPath ?? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            //map proxy to actual object
            Filters = ToFileFilterArray(fileFilters)
        };

        var pathResult = await Electron.Dialog.ShowSaveDialogAsync(mainWindow, options);
        if (string.IsNullOrEmpty(pathResult))
        {
            throw new OperationCanceledException("Cancelled by user");
        }

        return pathResult;
    }

    /// <inheritdoc cref="IElectronDialogManager.ShowOpenDialog"/>
    public async Task<IEnumerable<string>> ShowOpenDialog(string title, bool directory = false, bool multiSelect = false,
        string? defaultPath = null, IEnumerable<FileFilterProxy>? fileFilters = null)
    {
        var mainWindow = BrowserWindow;
        
        var openDialogProperties = new List<OpenDialogProperty>
        {
            directory ? OpenDialogProperty.openDirectory : OpenDialogProperty.openFile,
        };
        if (multiSelect) openDialogProperties.Add(OpenDialogProperty.multiSelections);
        
        var options = new OpenDialogOptions
        {
            Title = title,
            DefaultPath = defaultPath ?? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            Filters = ToFileFilterArray(fileFilters),
            Properties = openDialogProperties.ToArray()
        };
        var pathResult = await Electron.Dialog.ShowOpenDialogAsync(mainWindow, options);
        if (pathResult.All(string.IsNullOrEmpty))
        {
            throw new OperationCanceledException("Cancelled by user");
        }

        return pathResult.Where(res => !string.IsNullOrEmpty(res));
    }
    
    /// <inheritdoc cref="IElectronDialogManager.ShowOpenFileDialog"/>
    public async Task<string> ShowOpenFileDialog(string title, string? defaultPath = null,
        IEnumerable<FileFilterProxy>? fileFilters = null)
    {
        return (await ShowOpenDialog(title, false, false, defaultPath, fileFilters)).First();
    }

    /// <inheritdoc cref="IElectronDialogManager.ShowOpenDirectoryDialog"/>
    public async Task<string> ShowOpenDirectoryDialog(string title, string? defaultPath = null)
    {
        return (await ShowOpenDialog(title, true, false, defaultPath, null)).First();
    }

    private static FileFilter[] ToFileFilterArray(IEnumerable<FileFilterProxy>? fileFilters)
    {
        return (fileFilters ?? Array.Empty<FileFilterProxy>()).Select(filter =>
            new FileFilter { Name = filter.Name, Extensions = filter.Extensions }).ToArray();
    }
}