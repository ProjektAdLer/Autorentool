namespace BusinessLogic.ErrorManagement;

public interface IErrorManager
{
    /// <summary>
    /// Logs the specified exception and then rethrows it.
    /// </summary>
    /// <param name="exception">The exception to be logged and rethrown.</param>
    /// <exception cref="Exception">The original exception that is logged and then rethrown.</exception>
    void LogAndRethrowError(Exception exception);
}