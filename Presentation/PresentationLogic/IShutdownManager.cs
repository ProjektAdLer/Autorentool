namespace Presentation.PresentationLogic;

/// <summary>
/// Manages correct shutdown of the application. This is required because the Electron.NET Wrapper doesn't call our
/// OnShutdown callbacks before the application window closes.
/// </summary>
public interface IShutdownManager
{
    /// <summary>
    /// Fires when <see cref="RequestShutdownAsync"/> is first called. Shutdown can be cancelled by calling <see cref="BeforeShutdownEventArgs.CancelShutdownState"/>.
    /// </summary>
    event AsyncEventHandler<BeforeShutdownEventArgs> BeforeShutdown;
    
    /// <summary>
    /// Fires after <see cref="BeforeShutdown"/> finishes and shutdown wasn't cancelled.
    /// </summary>
    event AsyncEventHandler OnShutdown;
    
    /// <summary>
    /// Tries to shut down the application.
    /// </summary>
    /// <returns>Whether or not the shutdown was cancelled.</returns>
    Task<bool> RequestShutdownAsync();
}