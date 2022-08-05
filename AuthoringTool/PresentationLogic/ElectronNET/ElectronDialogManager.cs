using ElectronWrapper;

namespace AuthoringTool.PresentationLogic.ElectronNET;

public class ElectronDialogManager : IElectronDialogManager
{
    public ElectronDialogManager(IWindowManagerWrapper windowManager, IDialogWrapper dialogWrapper)
    {
        WindowManager = windowManager;
        DialogWrapper = dialogWrapper;
    }
    
    private BrowserWindow? BrowserWindow => WindowManager.BrowserWindows.FirstOrDefault();
    internal IWindowManagerWrapper WindowManager { get; }
    internal IDialogWrapper DialogWrapper { get; }
    
    /// <inheritdoc cref="IElectronDialogManager.ShowSaveAsDialogAsync"/>
    public async Task<string> ShowSaveAsDialogAsync(string title, string? defaultPath = null,
        IEnumerable<FileFilterProxy>? fileFilters = null)
    {
        if (BrowserWindow == null)
        {
            throw new Exception("BrowserWindow was unexpectedly null");
        }
        var options = new SaveDialogOptions
        {
            Title = title,
            //if default path is null use mydocuments special folder as fallback
            DefaultPath = defaultPath ?? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            //map proxy to actual object
            Filters = ToFileFilterArray(fileFilters)
        };

        var pathResult = await DialogWrapper.ShowSaveDialogAsync(BrowserWindow, options);
        if (string.IsNullOrEmpty(pathResult))
        {
            throw new OperationCanceledException("Cancelled by user");
        }

        return pathResult;
    }

    /// <inheritdoc cref="IElectronDialogManager.ShowOpenDialogAsync"/>
    public async Task<IEnumerable<string>> ShowOpenDialogAsync(string title, bool directory = false, bool multiSelect = false,
        string? defaultPath = null, IEnumerable<FileFilterProxy>? fileFilters = null)
    {
        if (BrowserWindow == null)
        {
            throw new Exception("BrowserWindow was unexpectedly null");
        }
        var openDialogProperties = new List<OpenDialogProperty>
        {
            directory ? OpenDialogProperty.OpenDirectory : OpenDialogProperty.OpenFile,
        };
        if (multiSelect) openDialogProperties.Add(OpenDialogProperty.MultiSelections);
        
        var options = new OpenDialogOptions
        {
            Title = title,
            DefaultPath = defaultPath ?? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            Filters = ToFileFilterArray(fileFilters),
            Properties = openDialogProperties.ToArray()
        };
        var pathResult = await DialogWrapper.ShowOpenDialogAsync(BrowserWindow, options);
        if (pathResult.All(string.IsNullOrEmpty))
        {
            throw new OperationCanceledException("Cancelled by user");
        }

        return pathResult.Where(res => !string.IsNullOrEmpty(res));
    }
    
    /// <inheritdoc cref="IElectronDialogManager.ShowOpenFileDialogAsync"/>
    public async Task<string> ShowOpenFileDialogAsync(string title, string? defaultPath = null,
        IEnumerable<FileFilterProxy>? fileFilters = null)
    {
        return (await ShowOpenDialogAsync(title, false, false, defaultPath, fileFilters)).First();
    }

    /// <inheritdoc cref="IElectronDialogManager.ShowOpenDirectoryDialogAsync"/>
    public async Task<string> ShowOpenDirectoryDialogAsync(string title, string? defaultPath = null)
    {
        return (await ShowOpenDialogAsync(title, true, false, defaultPath)).First();
    }

    private static FileFilter[] ToFileFilterArray(IEnumerable<FileFilterProxy>? fileFilters)
    {
        return (fileFilters ?? Array.Empty<FileFilterProxy>()).Select(filter =>
            new FileFilter { Name = filter.Name, Extensions = filter.Extensions }).ToArray();
    }
}