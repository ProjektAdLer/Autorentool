using ElectronNET.API;
using ElectronNET.API.Entities;

namespace AuthoringTool.PresentationLogic.ElectronNET;

public class FileFilterProxy
{
    internal readonly string Name;
    internal readonly string[] Extensions;

    public FileFilterProxy(string name, string[] extensions)
    {
        Name = name;
        Extensions = extensions;
    }
}

public class ElectronDialogManager
{
    private BrowserWindow BrowserWindow => Electron.WindowManager.BrowserWindows.First();


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

    /// <summary>
    /// Shows an Electron Open Dialog to the user.
    /// </summary>
    /// <param name="title">Title of the dialog.</param>
    /// <param name="directory">Whether or not a directory should be selected.</param>
    /// <param name="multiSelect">Whether or not multiple selections should be allowed.</param>
    /// <param name="defaultPath">A default path that should be preselected for the user, optional. Defaults to Documents folder.</param>
    /// <param name="fileFilters">Optional FileFilters.</param>
    /// <returns>Returns </returns>
    /// <exception cref="OperationCanceledException"></exception>
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
    
    /// <summary>
    /// Shorthand method for opening a single file open dialog.
    /// </summary>
    /// <param name="title">Title of the dialog.</param>
    /// <param name="defaultPath">A default path that should be preselected for the user, optional. Defaults to Documents folder.</param>
    /// <param name="fileFilters">Optional FileFilters.</param>
    /// <returns>Filepath to be opened.</returns>
    /// <exception cref="OperationCanceledException">Operation was cancelled by user.</exception>
    public async Task<string> ShowOpenFileDialog(string title, string? defaultPath = null,
        IEnumerable<FileFilterProxy>? fileFilters = null)
    {
        return (await ShowOpenDialog(title, false, false, defaultPath, fileFilters)).First();
    }

    /// <summary>
    /// Shorthand method for opening a single directory open dialog.
    /// </summary>
    /// <param name="title">Title of the dialog.</param>
    /// <param name="defaultPath">A default path that should be preselected for the user, optional. Defaults to Documents folder.</param>
    /// <returns>Directory to be opened.</returns>
    /// <exception cref="OperationCanceledException">Operation was cancelled by user.</exception>
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