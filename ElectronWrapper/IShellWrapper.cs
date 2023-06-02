using ElectronNET.API.Entities;
using System.Threading.Tasks;

namespace ElectronWrapper;

public interface IShellWrapper
{
    void Beep();
    
    /// <summary>
    /// Open the given file in the desktop's default manner.
    /// </summary>
    /// <param name="path">The path to the directory / file.</param>
    /// <returns>The error message corresponding to the failure if a failure occurred, otherwise <see cref="string.Empty"/>.</returns>
    Task<string> OpenPathAsync(string path);
    Task<ShortcutDetails> ReadShortcutLinkAsync(string shortcutPath);
    Task ShowItemInFolderAsync(string fullPath);
    Task<bool> TrashItemAsync(string fullPath);
    Task<bool> WriteShortcutLinkAsync(string shortcutPath, ShortcutLinkOperation operation, ShortcutDetails options);

    /// <summary>
    /// Open the given external protocol URL in the desktop’s default manner. 
    /// (For example, mailto: URLs in the user’s default mail agent).
    /// </summary>
    /// <param name="url">Max 2081 characters on windows.</param>
    /// <returns>The error message corresponding to the failure if a failure occurred, otherwise <see cref="string.Empty"/>.</returns>
    Task<string> OpenExternalAsync(string url);

    /// <summary>
    /// Open the given external protocol URL in the desktop’s default manner. 
    /// (For example, mailto: URLs in the user’s default mail agent).
    /// </summary>
    /// <param name="url">Max 2081 characters on windows.</param>
    /// <param name="options">Controls the behavior of OpenExternal.</param>
    /// <returns>The error message corresponding to the failure if a failure occurred, otherwise <see cref="string.Empty"/>.</returns>
    Task<string> OpenExternalAsync(string url, OpenExternalOptions options);
}