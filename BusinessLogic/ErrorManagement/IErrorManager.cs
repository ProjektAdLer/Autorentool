using Shared.Exceptions;

namespace BusinessLogic.ErrorManagement;

public interface IErrorManager
{
    /// <summary>
    /// Logs the specified exception and then rethrows it.
    /// </summary>
    /// <param name="exception">The exception to be logged and rethrown.</param>
    /// <exception cref="Exception">The original exception that is logged and then rethrown.</exception>
    void LogAndRethrowError(Exception exception);

    /// <summary>
    /// Logs the details of the exception and then rethrows it wrapped in a new UndoException.
    /// </summary>
    /// <param name="exception">The original exception that occurred.</param>
    /// <exception cref="UndoException">Throws a new UndoException with the original exception as the inner exception.</exception>
    void LogAndRethrowUndoError(Exception exception);

    /// <summary>
    /// Logs the details of the exception and then rethrows it wrapped in a new RedoException.
    /// </summary>
    /// <param name="exception">The original exception that occurred.</param>
    /// <exception cref="RedoException">Throws a new RedoException with the original exception as the inner exception.</exception>
    public void LogAndRethrowRedoError(Exception exception);
    
    /// <summary>
    /// Logs the details of the exception and then rethrows it wrapped in a new GeneratorException.
    /// </summary>
    /// <param name="exception">The original exception that occurred.</param>
    /// <exception cref="GeneratorException">Throws a new GeneratorException with the original exception as the inner exception.</exception>
    public void LogAndRethrowGeneratorError(Exception exception);
}