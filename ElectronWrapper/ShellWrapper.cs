using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;

namespace ElectronWrapper;

/// <summary>
/// Manage files and URLs using their default applications.
/// </summary>
class ShellWrapper : IShellWrapper
{
    private Shell shell;


    public ShellWrapper()
    {
        shell = Electron.Shell;
    }
    /// <summary>
    /// Show the given file in a file manager. If possible, select the file.
    /// </summary>
    /// <param name="fullPath">The full path to the directory / file.</param>
    public Task ShowItemInFolderAsync(string fullPath)
    {
        return shell.ShowItemInFolderAsync(fullPath);
    }

    /// <summary>
    /// Open the given file in the desktop's default manner.
    /// </summary>
    /// <param name="path">The path to the directory / file.</param>
    /// <returns>The error message corresponding to the failure if a failure occurred, otherwise <see cref="string.Empty"/>.</returns>
    public Task<string> OpenPathAsync(string path)
    {
        return shell.OpenPathAsync(path);
    }

    /// <summary>
    /// Open the given external protocol URL in the desktop’s default manner. 
    /// (For example, mailto: URLs in the user’s default mail agent).
    /// </summary>
    /// <param name="url">Max 2081 characters on windows.</param>
    /// <returns>The error message corresponding to the failure if a failure occurred, otherwise <see cref="string.Empty"/>.</returns>
    public Task<string> OpenExternalAsync(string url)
    {
        return shell.OpenExternalAsync(url);
    }

    /// <summary>
    /// Open the given external protocol URL in the desktop’s default manner. 
    /// (For example, mailto: URLs in the user’s default mail agent).
    /// </summary>
    /// <param name="url">Max 2081 characters on windows.</param>
    /// <param name="options">Controls the behavior of OpenExternal.</param>
    /// <returns>The error message corresponding to the failure if a failure occurred, otherwise <see cref="string.Empty"/>.</returns>
    public Task<string> OpenExternalAsync(string url, OpenExternalOptions options)
    {
        return shell.OpenExternalAsync(url, options);
    }

    /// <summary>
    /// Move the given file to trash and returns a <see cref="bool"/> status for the operation.
    /// </summary>
    /// <param name="fullPath">The full path to the directory / file.</param>
    /// <returns> Whether the item was successfully moved to the trash.</returns>
    public Task<bool> TrashItemAsync(string fullPath)
    {
        return shell.TrashItemAsync(fullPath);
    }

    /// <summary>
    /// Play the beep sound.
    /// </summary>
    public void Beep()
    {
        shell.Beep();
    }

    /// <summary>
    /// Creates or updates a shortcut link at shortcutPath.
    /// </summary>
    /// <param name="shortcutPath">The path to the shortcut.</param>
    /// <param name="operation">Default is <see cref="ShortcutLinkOperation.Create"/></param>
    /// <param name="options">Structure of a shortcut.</param>
    /// <returns>Whether the shortcut was created successfully.</returns>
    public Task<bool> WriteShortcutLinkAsync(string shortcutPath, ShortcutLinkOperation operation, ShortcutDetails options)
    {
        return shell.WriteShortcutLinkAsync(shortcutPath, operation, options);
    }

    /// <summary>
    /// Resolves the shortcut link at shortcutPath.
    /// An exception will be thrown when any error happens.
    /// </summary>
    /// <param name="shortcutPath">The path tot the shortcut.</param>
    /// <returns><see cref="ShortcutDetails"/> of the shortcut.</returns>
    public Task<ShortcutDetails> ReadShortcutLinkAsync(string shortcutPath)
    {
        return shell.ReadShortcutLinkAsync(shortcutPath);
    }
}