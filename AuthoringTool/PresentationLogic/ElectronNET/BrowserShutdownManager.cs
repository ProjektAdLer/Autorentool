using ElectronWrapper;

namespace AuthoringTool.PresentationLogic.ElectronNET;

public class BrowserShutdownManager : IShutdownManager
{
    private readonly IAppWrapper _appWrapper;

    public BrowserShutdownManager(IAppWrapper appWrapper)
    {
        _appWrapper = appWrapper;
    }
    
    /// <inheritdoc cref="IShutdownManager.BeforeShutdown"/>
    public event IShutdownManager.BeforeShutdownEventHandler? BeforeShutdown;
    
    /// <inheritdoc cref="IShutdownManager.OnShutdown"/>
    public event IShutdownManager.OnShutdownEventHandler? OnShutdown;
    
    /// <inheritdoc cref="IShutdownManager.BeginShutdown"/>
    bool IShutdownManager.BeginShutdown()
    {
        return true;
    }
}