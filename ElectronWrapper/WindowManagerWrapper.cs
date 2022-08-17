using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;

namespace ElectronWrapper;

class WindowManagerWrapper : IWindowManagerWrapper
{ 
    private WindowManager windowManager;

    public WindowManagerWrapper()
    {
        windowManager = Electron.WindowManager;
    }

    /// <summary>
    /// Quit when all windows are closed. (Default is true)
    /// </summary>
    /// <value>
    ///   <c>true</c> if [quit window all closed]; otherwise, <c>false</c>.
    /// </value>
    public bool IsQuitOnWindowAllClosed
    {
        get => windowManager.IsQuitOnWindowAllClosed;
        set => windowManager.IsQuitOnWindowAllClosed = value;
    }

    /// <summary>
    /// Gets the browser windows.
    /// </summary>
    /// <value>
    /// The browser windows.
    /// </value>
    public IReadOnlyCollection<BrowserWindow> BrowserWindows =>
        windowManager.BrowserWindows.Select(bw => new BrowserWindow(bw)).ToList().AsReadOnly();

    /// <summary>
    /// Gets the browser views.
    /// </summary>
    /// <value>
    /// The browser view.
    /// </value>
    public IReadOnlyCollection<BrowserView> BrowserViews => windowManager.BrowserViews;

    /// <summary>
    /// Creates the window asynchronous.
    /// </summary>
    /// <param name="loadUrl">The load URL.</param>
    /// <returns></returns>
    public async Task<BrowserWindow> CreateWindowAsync(string loadUrl = "http://localhost")
    {
        var innerBrowserWindow = await windowManager.CreateWindowAsync(loadUrl);
        return new BrowserWindow(innerBrowserWindow);
    }

    /// <summary>
    /// Creates the window asynchronous.
    /// </summary>
    /// <param name="options">The options.</param>
    /// <param name="loadUrl">The load URL.</param>
    /// <returns></returns>
    public async Task<BrowserWindow> CreateWindowAsync(BrowserWindowOptions options, string loadUrl = "http://localhost")
    {
        var innerBrowserWindow = await windowManager.CreateWindowAsync(options, loadUrl);
        return new BrowserWindow(innerBrowserWindow);
    }

    /// <summary>
    /// A BrowserView can be used to embed additional web content into a BrowserWindow. 
    /// It is like a child window, except that it is positioned relative to its owning window. 
    /// It is meant to be an alternative to the webview tag.
    /// </summary>
    /// <returns></returns>
    public Task<BrowserView> CreateBrowserViewAsync()
    {
        return windowManager.CreateBrowserViewAsync();
    }

    /// <summary>
    /// A BrowserView can be used to embed additional web content into a BrowserWindow. 
    /// It is like a child window, except that it is positioned relative to its owning window. 
    /// It is meant to be an alternative to the webview tag.
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public Task<BrowserView> CreateBrowserViewAsync(BrowserViewConstructorOptions options)
    {
        return windowManager.CreateBrowserViewAsync(options);
    }
}