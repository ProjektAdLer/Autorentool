namespace Presentation.PresentationLogic.ElectronNET;

public class BrowserShutdownManager : IShutdownManager
{
    /// <inheritdoc cref="IShutdownManager.BeforeShutdown"/>
    public event AsyncEventHandler<BeforeShutdownEventArgs>? BeforeShutdown;
    
    /// <inheritdoc cref="IShutdownManager.OnShutdown"/>
    public event AsyncEventHandler? OnShutdown;
    
    /// <inheritdoc cref="IShutdownManager.RequestShutdownAsync"/>
    Task<bool> IShutdownManager.RequestShutdownAsync() => Task.FromResult(true); //we just always pretend to be shutting down
}