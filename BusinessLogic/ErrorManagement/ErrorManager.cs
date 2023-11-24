using BusinessLogic.ErrorManagement.BackendAccess;
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
        _logger.LogError(exception, "an error has occurred: {Message}", exception.Message);

        throw exception;
    }

    /// <inheritdoc cref="IErrorManager.LogAndRethrowUndoError" />
    public void LogAndRethrowUndoError(Exception exception)
    {
        _logger.LogError(exception, "an error has occurred during undo operation: {Message}", exception.Message);

        throw new UndoException(exception.Message, exception);
    }

    /// <inheritdoc cref="IErrorManager.LogAndRethrowRedoError" />
    public void LogAndRethrowRedoError(Exception exception)
    {
        _logger.LogError(exception, "an error has occurred during redo operation: {Message}", exception.Message);

        throw new RedoException(exception.Message, exception);
    }

    /// <inheritdoc cref="IErrorManager.LogAndRethrowGeneratorError" />
    public void LogAndRethrowGeneratorError(Exception exception)
    {
        _logger.LogError(exception, "an error has occurred during generator operation: {Message}", exception.Message);

        throw new GeneratorException(exception.Message, exception);
    }

    public void LogAndRethrowBackendAccessError(Exception exception)
    {
        _logger.LogError(exception, "an error has occurred during backend access operation: {Message}",
            exception.Message);

        throw new BackendWorldDeletionException(exception.Message, exception);
    }
}