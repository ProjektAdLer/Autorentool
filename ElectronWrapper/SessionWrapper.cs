using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;
namespace ElectronWrapper;

/// <summary>
/// Manage browser sessions, cookies, cache, proxy settings, etc.
/// </summary>
class SessionWrapper : ISessionWrapper
{
    private Session session;

    public SessionWrapper()
    {
        var temp = Electron.WindowManager.CreateWindowAsync();

        while (temp.Result == null)
        {
            Task.Delay(100).Wait();
        }
        session = temp.Result.WebContents.Session;
    }

    /// <summary>
    /// Gets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    public int Id => session.Id;


    /// <summary>
    /// Query and modify a session's cookies.
    /// </summary>
    public Cookies Cookies => session.Cookies;


    /// <summary>
    /// Dynamically sets whether to always send credentials for HTTP NTLM or Negotiate authentication.
    /// </summary>
    /// <param name="domains">A comma-separated list of servers for which integrated authentication is enabled.</param>
    public void AllowNtlmCredentialsForDomains(string domains)
    {
        session.AllowNTLMCredentialsForDomains(domains);
    }

    /// <summary>
    /// Clears the session’s HTTP authentication cache.
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public Task ClearAuthCacheAsync(RemovePassword options)
    {
        return session.ClearAuthCacheAsync(options);
    }

    /// <summary>
    /// Clears the session’s HTTP authentication cache.
    /// </summary>
    public Task ClearAuthCacheAsync()
    {
        return session.ClearAuthCacheAsync();
    }

    /// <summary>
    /// Clears the session’s HTTP cache.
    /// </summary>
    /// <returns></returns>
    public Task ClearCacheAsync()
    {
        return session.ClearCacheAsync();
    }

    /// <summary>
    /// Clears the host resolver cache.
    /// </summary>
    /// <returns></returns>
    public Task ClearHostResolverCacheAsync()
    {
        return session.ClearHostResolverCacheAsync();
    }

    /// <summary>
    /// Clears the data of web storages.
    /// </summary>
    /// <returns></returns>
    public Task ClearStorageDataAsync()
    {
        return session.ClearStorageDataAsync();
    }
    /// <summary>
    /// Clears the data of web storages.
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public Task ClearStorageDataAsync(ClearStorageDataOptions options)
    {
        return session.ClearStorageDataAsync(options);
    }
    /// <summary>
    /// Allows resuming cancelled or interrupted downloads from previous Session. The
    /// API will generate a DownloadItem that can be accessed with the will-download
    /// event. The DownloadItem will not have any WebContents associated with it and the
    /// initial state will be interrupted. The download will start only when the resume
    /// API is called on the DownloadItem.
    /// </summary>
    /// <param name="options"></param>
    public void CreateInterruptedDownload(CreateInterruptedDownloadOptions options)
    {
        session.CreateInterruptedDownload(options);
    }

    /// <summary>
    /// Disables any network emulation already active for the session. Resets to the
    /// original network configuration.
    /// </summary>
    public void DisableNetworkEmulation()
    {
        session.DisableNetworkEmulation();
    }
    /// <summary>
    /// Emulates network with the given configuration for the session.
    /// </summary>
    /// <param name="options"></param>
    public void EnableNetworkEmulation(EnableNetworkEmulationOptions options)
    {
        session.EnableNetworkEmulation(options);
    }


    /// <summary>
    /// Writes any unwritten DOMStorage data to disk.
    /// </summary>
    public void FlushStorageData()
    {
        session.FlushStorageData();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="identifier"></param>
    /// <returns></returns>
    public Task<int[]> GetBlobDataAsync(string identifier)
    {
        return session.GetBlobDataAsync(identifier);
    }
    /// <summary>
    /// Get session's current cache size.
    /// </summary>
    /// <returns>Callback is invoked with the session's current cache size.</returns>
    public Task<int> GetCacheSizeAsync()
    {
        return session.GetCacheSizeAsync();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Task<string[]> GetPreloadsAsync()
    {
        return session.GetPreloadsAsync();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Task<string> GetUserAgent()
    {
        return session.GetUserAgent();
    }

    /// <summary>
    /// Resolves the proxy information for url. The callback will be called with
    /// callback(proxy) when the request is performed.
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public Task<string> ResolveProxyAsync(string url)
    {
        return session.ResolveProxyAsync(url);
    }
    /// <summary>
    /// Sets download saving directory. By default, the download directory will be the
    /// Downloads under the respective app folder.
    /// </summary>
    /// <param name="path"></param>
    public void SetDownloadPath(string path)
    {
        session.SetDownloadPath(path);
    }

    /// <summary>
    /// Adds scripts that will be executed on ALL web contents that are associated with
    /// this session just before normal preload scripts run.
    /// </summary>
    /// <param name="preloads"></param>
    public void SetPreloads(string[] preloads)
    {
        session.SetPreloads(preloads);
    }

    /// <summary>
    /// Sets the proxy settings. When pacScript and proxyRules are provided together,
    /// the proxyRules option is ignored and pacScript configuration is applied.
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    public Task SetProxyAsync(ProxyConfig config)
    {
        return session.SetProxyAsync(config);
    }

    /// <summary>
    /// Overrides the userAgent for this session. This doesn't affect existing WebContents, and
    /// each WebContents can use webContents.setUserAgent to override the session-wide
    /// user agent.
    /// </summary>
    /// <param name="userAgent"></param>
    public void SetUserAgent(string userAgent)
    {
        session.SetUserAgent(userAgent);
    }

    /// <summary>
    /// Overrides the userAgent and acceptLanguages for this session. The
    /// acceptLanguages must a comma separated ordered list of language codes, for
    /// example "en-US,fr,de,ko,zh-CN,ja". This doesn't affect existing WebContents, and
    /// each WebContents can use webContents.setUserAgent to override the session-wide
    /// user agent.
    /// </summary>
    /// <param name="userAgent"></param>
    /// <param name="acceptLanguages">The
    /// acceptLanguages must a comma separated ordered list of language codes, for
    /// example "en-US,fr,de,ko,zh-CN,ja".</param>
    public void SetUserAgent(string userAgent, string acceptLanguages)
    {
        session.SetUserAgent(userAgent, acceptLanguages);
    }
    /// <summary>
    /// The keys are the extension names and each value is an object containing name and version properties.
    /// Note: This API cannot be called before the ready event of the app module is emitted.
    /// </summary>
    /// <returns></returns>
    public Task<ChromeExtensionInfo[]> GetAllExtensionsAsync()
    {
        return session.GetAllExtensionsAsync();
    }


    /// <summary>
    /// Remove Chrome extension with the specified name.
    /// Note: This API cannot be called before the ready event of the app module is emitted.
    /// </summary>
    /// <param name="name">Name of the Chrome extension to remove</param>
    public void RemoveExtension(string name)
    {
        session.RemoveExtension(name);
    }

    /// <summary>
    /// resolves when the extension is loaded.
    ///
    /// This method will raise an exception if the extension could not be loaded.If
    /// there are warnings when installing the extension (e.g. if the extension requests
    /// an API that Electron does not support) then they will be logged to the console.
    ///
    /// Note that Electron does not support the full range of Chrome extensions APIs.
    /// See Supported Extensions APIs for more details on what is supported.
    ///
    /// Note that in previous versions of Electron, extensions that were loaded would be
    /// remembered for future runs of the application.This is no longer the case:
    /// `loadExtension` must be called on every boot of your app if you want the
    /// extension to be loaded.
    ///
    /// This API does not support loading packed (.crx) extensions.
    ///
    ///** Note:** This API cannot be called before the `ready` event of the `app` module
    /// is emitted.
    ///
    ///** Note:** Loading extensions into in-memory(non-persistent) sessions is not supported and will throw an error.
    /// </summary>
    /// <param name="path">Path to the Chrome extension</param>
    /// <param name="allowFileAccess">Whether to allow the extension to read local files over `file://` protocol and
    /// inject content scripts into `file://` pages. This is required e.g. for loading
    /// devtools extensions on `file://` URLs. Defaults to false.</param>
    /// <returns></returns>
    public Task<Extension> LoadExtensionAsync(string path, bool allowFileAccess = false)
    {
        return session.LoadExtensionAsync(path, allowFileAccess);
    }
}