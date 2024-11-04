namespace BusinessLogic.ErrorManagement.BackendAccess;

public static class ErrorCodes
{
    /// <summary>
    ///     Error code for using an invalid token
    /// </summary>
    public const string LmsTokenInvalid = "invalid_token";

    /// <summary>
    ///     Error code for wrong username or password
    /// </summary>
    public const string InvalidLogin = "invalid_login";

    /// <summary>
    ///     Error if a resource is not found
    /// </summary>
    public const string NotFound = "not_found";

    /// <summary>
    ///     Error if a resource is forbidden
    /// </summary>
    public const string Forbidden = "forbidden";

    /// <summary>
    ///     Error if a request is invalid
    /// </summary>
    public const string ValidationError = "validation_error";

    /// <summary>
    ///     Error code for unknown errors
    /// </summary>
    public const string UnknownError = "unknown_error";

    /// <summary>
    ///     Error code for generic LMS errors
    /// </summary>
    public const string LmsError = "lms_error";

    /// <summary>
    ///     Error code when a world could not be created
    /// </summary>
    public const string WorldCreationErrorDuplicate = "world_creation_error";
}