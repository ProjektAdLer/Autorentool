﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronSharp.API;
using ElectronSharp.API.Entities;

namespace ElectronWrapper;

class WindowManagerWrapper : IWindowManagerWrapper
{ 
    private readonly WindowManager _windowManager = Electron.WindowManager;

    /// <summary>
    /// Quit when all windows are closed. (Default is true)
    /// </summary>
    /// <value>
    ///   <c>true</c> if [quit window all closed]; otherwise, <c>false</c>.
    /// </value>
    public bool IsQuitOnWindowAllClosed
    {
        get => _windowManager.IsQuitOnWindowAllClosed;
        set => _windowManager.IsQuitOnWindowAllClosed = value;
    }

    /// <summary>
    /// Gets the browser windows.
    /// </summary>
    /// <value>
    /// The browser windows.
    /// </value>
    public IReadOnlyCollection<BrowserWindow> BrowserWindows =>
        _windowManager.BrowserWindows.Select(bw => new BrowserWindow(bw)).ToList().AsReadOnly();

    /// <summary>
    /// Gets the browser views.
    /// </summary>
    /// <value>
    /// The browser view.
    /// </value>
    public IReadOnlyCollection<BrowserView> BrowserViews => _windowManager.BrowserViews;

    /// <summary>
    /// Creates the window asynchronous.
    /// </summary>
    /// <param name="loadUrl">The load URL.</param>
    /// <returns></returns>
    public async Task<BrowserWindow> CreateWindowAsync(string loadUrl = "http://localhost")
    {
        var innerBrowserWindow = await _windowManager.CreateWindowAsync(loadUrl);
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
        var innerBrowserWindow = await _windowManager.CreateWindowAsync(options, loadUrl);
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
        return _windowManager.CreateBrowserViewAsync();
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
        return _windowManager.CreateBrowserViewAsync(options);
    }
}