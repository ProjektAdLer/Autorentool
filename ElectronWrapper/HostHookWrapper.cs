using System.Threading.Tasks;
using ElectronNET.API;

namespace ElectronWrapper;

/// <summary>
/// Allows you to execute native JavaScript/TypeScript code from the host process.
/// 
/// It is only possible if the Electron.NET CLI has previously added an 
/// ElectronHostHook directory:
/// <c>electronize add HostHook</c>
/// </summary>
class HostHookWrapper : IHostHookWrapper
{
    private HostHook hostHook;

    public HostHookWrapper()
    {
        hostHook = Electron.HostHook;
    }

    /// <summary>
    /// Execute native JavaScript/TypeScript code.
    /// </summary>
    /// <param name="socketEventName">Socket name registered on the host.</param>
    /// <param name="arguments">Optional parameters.</param>
    public void Call(string socketEventName, params dynamic[] arguments)
    {
        hostHook.Call(socketEventName, arguments);
    }

    /// <summary>
    /// Execute native JavaScript/TypeScript code.
    /// </summary>
    /// <typeparam name="T">Results from the executed host code.</typeparam>
    /// <param name="socketEventName">Socket name registered on the host.</param>
    /// <param name="arguments">Optional parameters.</param>
    /// <returns></returns>
    public Task<T> CallAsync<T>(string socketEventName, params dynamic[] arguments)
    {
        return hostHook.CallAsync<T>(socketEventName, arguments);
    }
}