using System.Collections.Generic;
using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;

namespace ElectronWrapper;

public interface IWindowManagerWrapper
{
    bool IsQuitOnWindowAllClosed { get; set; }
    IReadOnlyCollection<BrowserWindow> BrowserWindows { get; }
    IReadOnlyCollection<BrowserView> BrowserViews { get; }
    Task<BrowserWindow> CreateWindowAsync(string loadurl);
    Task<BrowserView> CreateBrowserViewAsync();
    Task<BrowserWindow> CreateWindowAsync(BrowserWindowOptions options, string loadUrl = "http://localhost");
    Task<BrowserView> CreateBrowserViewAsync(BrowserViewConstructorOptions options);
}