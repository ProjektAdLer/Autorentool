using ElectronWrapper;

namespace AuthoringToolLib.PresentationLogic.ElectronNET;

public class ElectronShutdownManager : IShutdownManager
{
    private readonly IAppWrapper _appWrapper;

    public ElectronShutdownManager(IAppWrapper appWrapper)
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
        var eventArgs = new BeforeShutdownEventArgs();
        BeforeShutdown?.Invoke(this, eventArgs);
        if (eventArgs.CancelShutdownState) return true;
        OnShutdown?.Invoke(this);
        _appWrapper.Exit();
        return false;
    }
}