namespace H5pPlayer.BusinessLogic.Domain;

public class PathValidator
{
    public PathValidator()
    {
    }

    public void ThrowArgumentExceptionIfPathIsNotRooted(string path, string message)
    {
        if(!Path.IsPathRooted(path))
            throw new ArgumentException(message);
    }

    public void ThrowArgumentNullExceptionIfPathIsNull(string path, string parameterName)
    {
        if (path == null)
            throw new ArgumentNullException(parameterName);
    }

    public void ThrowArgumentExceptionIfPathIsEmpty(string path, string message)
    {
        if (path == string.Empty)
            throw new ArgumentException(message);
    }

    public void ThrowArgumentExceptionIfPathIsNullOrWhitespace(string path, string message)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException(message);
    }

    public void ThrowArgumentExceptionIfPathContainsInvalidPathChars(string path, string message)
    {
        if (path.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            throw new ArgumentException(message);
    }
}