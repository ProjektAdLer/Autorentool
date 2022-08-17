using ElectronNET.API;
using ElectronNET.API.Entities;
using System.Threading.Tasks;

namespace ElectronWrapper;

interface ISessionWrapper
{
    Cookies Cookies { get; }
    int Id { get; }

    void AllowNtlmCredentialsForDomains(string domains);
    Task ClearAuthCacheAsync();
    Task ClearAuthCacheAsync(RemovePassword options);
    Task ClearCacheAsync();
    Task ClearHostResolverCacheAsync();
    Task ClearStorageDataAsync();
    Task ClearStorageDataAsync(ClearStorageDataOptions options);
    void CreateInterruptedDownload(CreateInterruptedDownloadOptions options);
    void DisableNetworkEmulation();
    void EnableNetworkEmulation(EnableNetworkEmulationOptions options);
    void FlushStorageData();
    Task<ChromeExtensionInfo[]> GetAllExtensionsAsync();
    Task<int[]> GetBlobDataAsync(string identifier);
    Task<int> GetCacheSizeAsync();
    Task<string[]> GetPreloadsAsync();
    Task<string> GetUserAgent();
    Task<Extension> LoadExtensionAsync(string path, bool allowFileAccess = false);
    void RemoveExtension(string name);
    Task<string> ResolveProxyAsync(string url);
    void SetDownloadPath(string path);
    void SetPreloads(string[] preloads);
    Task SetProxyAsync(ProxyConfig config);
    void SetUserAgent(string userAgent);
    void SetUserAgent(string userAgent, string acceptLanguages);
}