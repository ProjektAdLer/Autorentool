namespace AuthoringTool.PresentationLogic.ElectronNET;

public interface IElectronDialogManager
{
    /// <summary>
    /// Shows an Electron Save As Dialog to the user.
    /// </summary>
    /// <param name="title">Title of the dialog.</param>
    /// <param name="defaultPath">A default path that should be preselected for the user, optional. Defaults to Documents folder.</param>
    /// <param name="fileFilters">Optional FileFilters.</param>
    /// <returns>Filepath to be saved.</returns>
    /// <exception cref="OperationCanceledException">Operation was cancelled by user.</exception>
    /// <exception cref="Exception">No browser window exists.</exception>
    Task<string> ShowSaveAsDialog(string title, string? defaultPath = null, IEnumerable<FileFilterProxy>? fileFilters = null);

    /// <summary>
    /// Shows an Electron Open Dialog to the user.
    /// </summary>
    /// <param name="title">Title of the dialog.</param>
    /// <param name="directory">Whether or not a directory should be selected.</param>
    /// <param name="multiSelect">Whether or not multiple selections should be allowed.</param>
    /// <param name="defaultPath">A default path that should be preselected for the user, optional. Defaults to Documents folder.</param>
    /// <param name="fileFilters">Optional FileFilters.</param>
    /// <returns>Path(s) to be opened.</returns>
    /// <exception cref="OperationCanceledException">Operation was cancelled by user.</exception>
    /// <exception cref="Exception">No browser window exists.</exception>
    Task<IEnumerable<string>> ShowOpenDialog(string title, bool directory = false, bool multiSelect = false,
        string? defaultPath = null, IEnumerable<FileFilterProxy>? fileFilters = null);

    /// <summary>
    /// Shorthand method for opening a single file open dialog.
    /// </summary>
    /// <param name="title">Title of the dialog.</param>
    /// <param name="defaultPath">A default path that should be preselected for the user, optional. Defaults to Documents folder.</param>
    /// <param name="fileFilters">Optional FileFilters.</param>
    /// <returns>Filepath to be opened.</returns>
    /// <exception cref="OperationCanceledException">Operation was cancelled by user.</exception>
    /// <exception cref="Exception">No browser window exists.</exception>
    Task<string> ShowOpenFileDialog(string title, string? defaultPath = null,
        IEnumerable<FileFilterProxy>? fileFilters = null);

    /// <summary>
    /// Shorthand method for opening a single directory open dialog.
    /// </summary>
    /// <param name="title">Title of the dialog.</param>
    /// <param name="defaultPath">A default path that should be preselected for the user, optional. Defaults to Documents folder.</param>
    /// <returns>Directory to be opened.</returns>
    /// <exception cref="OperationCanceledException">Operation was cancelled by user.</exception>
    /// <exception cref="Exception">No browser window exists.</exception>
    Task<string> ShowOpenDirectoryDialog(string title, string? defaultPath = null);
}