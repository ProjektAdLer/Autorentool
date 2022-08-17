using System;
using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;

namespace ElectronWrapper;

/// <summary>
/// Render and control web pages.
/// </summary>
class WebContentsWrapper : IWebContentsWrapper
{
    private WebContents webContents;

    public WebContentsWrapper()
    {
        var temp = Electron.WindowManager.CreateWindowAsync();

        while (temp.Result == null)
        {
            Task.Delay(100).Wait();
        }
        webContents = temp.Result.WebContents;
    }


    /// <summary>
    /// Gets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    public int Id => webContents.Id;

    /// <summary>
    /// Manage browser sessions, cookies, cache, proxy settings, etc.
    /// </summary>
    public Session Session => webContents.Session;


    /// <summary>
    /// Emitted when the renderer process crashes or is killed.
    /// </summary>
    public event Action<bool> OnCrashed
    {
        add => webContents.OnCrashed += value;
        remove => webContents.OnCrashed -= value;
    }

    /// <summary>
    /// Emitted when the navigation is done, i.e. the spinner of the tab has
    /// stopped spinning, and the onload event was dispatched.
    /// </summary>
    public event Action OnDidFinishLoad
    {
        add => webContents.OnDidFinishLoad += value;
        remove => webContents.OnDidFinishLoad -= value;
    }

    /// <summary>
    /// Opens the devtools.
    /// </summary>
    public void OpenDevTools()
    {
        webContents.OpenDevTools();
    }

    /// <summary>
    /// Opens the devtools.
    /// </summary>
    /// <param name="openDevToolsOptions"></param>
    public void OpenDevTools(OpenDevToolsOptions openDevToolsOptions)
    {
        webContents.OpenDevTools(openDevToolsOptions);
    }

    /// <summary>
    /// Get system printers.
    /// </summary>
    /// <returns>printers</returns>
    public Task<PrinterInfo[]> GetPrintersAsync()
    {
        return webContents.GetPrintersAsync();
    }

    /// <summary>
    /// Prints window's web page.
    /// </summary>
    /// <param name="options"></param>
    /// <returns>success</returns>
    public Task<bool> PrintAsync(PrintOptions? options = null)
    {
        return webContents.PrintAsync(options);
    }


    /// <summary>
    /// Prints window's web page as PDF with Chromium's preview printing custom
    /// settings.The landscape will be ignored if @page CSS at-rule is used in the web page. 
    /// By default, an empty options will be regarded as: Use page-break-before: always; 
    /// CSS style to force to print to a new page.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="options"></param>
    /// <returns>success</returns>
    public Task<bool> PrintToPdfAsync(string path, PrintToPDFOptions? options = null)
    {
        return webContents.PrintToPDFAsync(path, options);
    }

    /// <summary>
    /// Is used to get the Url of the loaded page.
    /// It's usefull if a web-server redirects you and you need to know where it redirects. For instance, It's useful in case of Implicit Authorization.
    /// </summary>
    /// <returns>URL of the loaded page</returns>
    public Task<string> GetUrl()
    {
        return webContents.GetUrl();
    }

    /// <summary>
    /// The async method will resolve when the page has finished loading, 
    /// and rejects if the page fails to load.
    /// 
    /// A noop rejection handler is already attached, which avoids unhandled rejection
    /// errors.
    ///
    /// Loads the `url` in the window. The `url` must contain the protocol prefix, e.g.
    /// the `http://` or `file://`. If the load should bypass http cache then use the
    /// `pragma` header to achieve it.
    /// </summary>
    /// <param name="url"></param>
    public Task LoadUrlAsync(string url)
    {
        return webContents.LoadURLAsync(url);
    }

    /// <summary>
    /// The async method will resolve when the page has finished loading, 
    /// and rejects if the page fails to load.
    /// 
    /// A noop rejection handler is already attached, which avoids unhandled rejection
    /// errors.
    ///
    /// Loads the `url` in the window. The `url` must contain the protocol prefix, e.g.
    /// the `http://` or `file://`. If the load should bypass http cache then use the
    /// `pragma` header to achieve it.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="options"></param>
    public Task LoadUrlAsync(string url, LoadURLOptions options)
    {
        return webContents.LoadURLAsync(url, options);
    }

    /// <summary>
    /// Inserts CSS into the web page.
    /// See: https://www.electronjs.org/docs/api/web-contents#contentsinsertcsscss-options
    /// Works for both BrowserWindows and BrowserViews.
    /// </summary>
    /// <param name="isBrowserWindow">Whether the webContents belong to a BrowserWindow or not (the other option is a BrowserView)</param>
    /// <param name="path">Absolute path to the CSS file location</param>
    public void InsertCss(bool isBrowserWindow, string path)
    {
        webContents.InsertCSS(isBrowserWindow, path);
    }
}