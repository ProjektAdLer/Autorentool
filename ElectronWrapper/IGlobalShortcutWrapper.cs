using System;
using System.Threading.Tasks;

namespace ElectronWrapper;

interface IGlobalShortcutWrapper
{
    Task<bool> IsRegisteredAsync(string accelerator);
    void Register(string accelerator, Action function);
    void Unregister(string accelerator);
    void UnregisterAll();
}