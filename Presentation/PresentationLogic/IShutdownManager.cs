namespace Presentation.PresentationLogic;

/// <summary>
/// Manages correct shutdown of the application. This is required because the Electron.NET Wrapper doesn't call our
/// OnShutdown callbacks before the application window closes.
/// </summary>
public interface IShutdownManager
{
    /// <summary>
    /// Delegate for <see cref="IShutdownManager.BeforeShutdown"/>
    /// </summary>
    public delegate void BeforeShutdownEventHandler(object sender, BeforeShutdownEventArgs e);
    
    /// <summary>
    /// Delegate for <see cref="IShutdownManager.OnShutdown"/>.
    /// </summary>
    public delegate void OnShutdownEventHandler(object sender);
    
    /// <summary>
    /// Fires when <see cref="BeginShutdown"/> is first called. Shutdown can be cancelled by calling <see cref="BeforeShutdownEventArgs.CancelShutdownState"/>.
    /// </summary>
    event BeforeShutdownEventHandler BeforeShutdown;
    
    /// <summary>
    /// Fires after <see cref="BeforeShutdown"/> finishes and shutdown wasn't cancelled.
    /// </summary>
    event OnShutdownEventHandler OnShutdown;
    
    /// <summary>
    /// Tries to shut down the application.
    /// </summary>
    /// <returns>Whether or not the shutdown was cancelled.</returns>
    bool BeginShutdown();
}