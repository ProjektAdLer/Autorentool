using System.Threading.Tasks;
using ElectronSharp.API;
using ElectronSharp.API.Entities;

namespace ElectronWrapper;

/// <summary>
/// Manage files and URLs using their default applications.
/// </summary>
public class ShellWrapper : IShellWrapper
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
}