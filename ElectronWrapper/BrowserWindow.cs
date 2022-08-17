namespace ElectronWrapper;

public class BrowserWindow
{
    public BrowserWindow()
    {
        
    }

    internal BrowserWindow(ElectronNET.API.BrowserWindow innerBrowserWindow)
    {
        InnerBrowserWindow = innerBrowserWindow;
    }
    
    internal ElectronNET.API.BrowserWindow? InnerBrowserWindow { get; }
    
}