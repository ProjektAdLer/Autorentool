using ElectronNET.API.Entities;
using System.Threading.Tasks;

namespace ElectronWrapper;

interface IShellWrapper
{
    void Beep();
    Task<string> OpenExternalAsync(string url);
    Task<string> OpenExternalAsync(string url, OpenExternalOptions options);
    Task<string> OpenPathAsync(string path);
    Task<ShortcutDetails> ReadShortcutLinkAsync(string shortcutPath);
    Task ShowItemInFolderAsync(string fullPath);
    Task<bool> TrashItemAsync(string fullPath);
    Task<bool> WriteShortcutLinkAsync(string shortcutPath, ShortcutLinkOperation operation, ShortcutDetails options);
}