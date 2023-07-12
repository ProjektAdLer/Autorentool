using Microsoft.Extensions.Logging;

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
}