using Microsoft.Extensions.Logging;
using Shared.Exceptions;

namespace BusinessLogic.ErrorManagement;

public class ErrorManager : IErrorManager
{
    private readonly ILogger<ErrorManager> _logger;

    public ErrorManager(ILogger<ErrorManager> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc cref="IErrorManager.LogAndRethrowError" />
    public void LogAndRethrowError(Exception exception)
    {
        _logger.LogError(exception, "an error has occurred: {message}", exception.Message);

        throw exception;
    }
    
    /// <inheritdoc cref="IErrorManager.LogAndRethrowUndoError" />
    public void LogAndRethrowUndoError(Exception exception)
    {
        _logger.LogError(exception, "an error has occurred during undo operation: {message}", exception.Message);

        throw new UndoException(exception.Message ,exception);
    }

    /// <inheritdoc cref="IErrorManager.LogAndRethrowRedoError" />
    public void LogAndRethrowRedoError(Exception exception)
    {
        _logger.LogError(exception, "an error has occurred during redo operation: {message}", exception.Message);

        throw new RedoException(exception.Message, exception);
    }
}