namespace H5pPlayer.BusinessLogic.Domain;

public static class PathValidator
{
    public static void ThrowArgumentNullExceptionIfPathIsNull(string value, string parameterName)
    {
        if (value == null)
            throw new ArgumentNullException(parameterName);
    }

    public static void ThrowArgumentExceptionIfPathIsEmpty(string value, string message)
    {
        if (value == string.Empty)
            throw new ArgumentException(message);
    }

    public static void ThrowArgumentExceptionIfPathIsNullOrWhitespace(string value, string message)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(message);
    }

    public static void ThrowArgumentExceptionIfPathContainsInvalidPathChars(string value, string message)
    {
        if (value.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            throw new ArgumentException(message);
    }
}