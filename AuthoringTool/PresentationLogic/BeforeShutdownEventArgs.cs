namespace AuthoringTool.PresentationLogic;

public class BeforeShutdownEventArgs
{
    internal bool CancelShutdownState { get; private set; }
    
    /// <summary>
    /// Cancel current shutdown event.
    /// </summary>
    public void CancelShutdown()
    {
        CancelShutdownState = true;
    }
}