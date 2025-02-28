namespace ElectronWrapper;

public class BrowserWindow
{
    public BrowserWindow()
    {
        
    }

    internal BrowserWindow(ElectronSharp.API.BrowserWindow innerBrowserWindow)
    {
        InnerBrowserWindow = innerBrowserWindow;
    }
    
    internal ElectronSharp.API.BrowserWindow? InnerBrowserWindow { get; }
    
}