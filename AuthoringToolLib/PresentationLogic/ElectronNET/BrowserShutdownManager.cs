namespace AuthoringToolLib.PresentationLogic.ElectronNET;

public class BrowserShutdownManager : IShutdownManager
{
    /// <inheritdoc cref="IShutdownManager.BeforeShutdown"/>
    public event IShutdownManager.BeforeShutdownEventHandler? BeforeShutdown;
    
    /// <inheritdoc cref="IShutdownManager.OnShutdown"/>
    public event IShutdownManager.OnShutdownEventHandler? OnShutdown;
    
    /// <inheritdoc cref="IShutdownManager.BeginShutdown"/>
    bool IShutdownManager.BeginShutdown() => true; //we just always pretend to be shutting down
}