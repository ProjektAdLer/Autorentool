using System.Threading.Tasks;

namespace ElectronWrapper;

interface IHostHookWrapper
{
    void Call(string socketEventName, params dynamic[] arguments);
    Task<T> CallAsync<T>(string socketEventName, params dynamic[] arguments);
}