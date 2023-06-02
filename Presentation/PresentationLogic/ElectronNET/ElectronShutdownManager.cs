using ElectronWrapper;

namespace Presentation.PresentationLogic.ElectronNET;

public class ElectronShutdownManager : IShutdownManager
{
    private readonly IAppWrapper _appWrapper;

    public ElectronShutdownManager(IAppWrapper appWrapper)
    {
        _appWrapper = appWrapper;
    }
    
    /// <inheritdoc cref="IShutdownManager.BeforeShutdown"/>
    public event AsyncEventHandler<BeforeShutdownEventArgs>? BeforeShutdown;
    
    /// <inheritdoc cref="IShutdownManager.OnShutdown"/>
    public event AsyncEventHandler? OnShutdown;
    
    /// <inheritdoc cref="IShutdownManager.RequestShutdownAsync"/>
    async Task<bool> IShutdownManager.RequestShutdownAsync()
    {
        var eventArgs = new BeforeShutdownEventArgs();
        if(BeforeShutdown is not null) await BeforeShutdown(this, eventArgs);
        if (eventArgs.CancelShutdownState) return true;
        if(OnShutdown is not null) await OnShutdown(this);
        _appWrapper.Exit();
        return false;
    }
}