namespace Presentation.PresentationLogic.ElectronNET;

public class BrowserShutdownManager : IShutdownManager
{
    /// <inheritdoc cref="IShutdownManager.BeforeShutdown"/>
    public event AsyncEventHandler<BeforeShutdownEventArgs>? BeforeShutdown
        //explicit empty implementation suppresses CS0067: The event 'IShutdownManager.BeforeShutdown' is never used
    {
        add { }
        remove { }
    }

    /// <inheritdoc cref="IShutdownManager.OnShutdown"/>
    public event AsyncEventHandler? OnShutdown
        //explicit empty implementation suppresses CS0067: The event 'IShutdownManager.OnShutdown' is never used
    {
        add { }
        remove { }
    }

    /// <inheritdoc cref="IShutdownManager.RequestShutdownAsync"/>
    Task<bool> IShutdownManager.RequestShutdownAsync() =>
        Task.FromResult(true); //we just always pretend to be shutting down
}