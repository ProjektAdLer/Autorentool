namespace H5pPlayer.BusinessLogic.Domain;

public class H5pEntity
{

    public H5pEntity()
    {
        ActiveDisplayMode = H5pDisplayMode.Display;
        // init by empty cause of unix mac windows usage. 
        // -> otherwise we must deside the OS here 
        h5pZipSourcePath = string.Empty;
    }
    
    private string h5pZipSourcePath;
    /// <exception cref="ArgumentNullException">If path is null.</exception>
    /// <exception cref="ArgumentException">If path is empty or whitespace.</exception>
    /// <exception cref="ArgumentException">If path contains invalid path chars from
    /// <see cref="System.IO.Path.GetInvalidPathChars()"/></exception>
    public string H5pZipSourcePath
    {
        get => h5pZipSourcePath;
        set
        {
            ThrowExceptionIfPathToSetIsNotValid(value);
            h5pZipSourcePath = value;
        }
    }


    /// <exception cref="ArgumentNullException">If path is null.</exception>
    /// <exception cref="ArgumentException">If path is empty or whitespace.</exception>
    /// <exception cref="ArgumentException">If path contains invalid path chars from
    /// <see cref="System.IO.Path.GetInvalidPathChars()"/></exception>
    private static void ThrowExceptionIfPathToSetIsNotValid(string value)
    {
        ThrowArgumentNullExceptionIfStringIsNull(value);
        ThrowArgumentExceptionIfStringIsEmpty(value);
        ThrowArgumentExceptionIfStringIsNullOrWhitespace(value);
        ThrowArgumentExceptionIfPathContainsInvalidPathChars(value);
    }
    private static void ThrowArgumentNullExceptionIfStringIsNull(string value)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(H5pZipSourcePath));
    }
    private static void ThrowArgumentExceptionIfStringIsEmpty(string value)
    {
        if (value == string.Empty)
            throw new ArgumentException(nameof(H5pZipSourcePath));
    }
    private static void ThrowArgumentExceptionIfStringIsNullOrWhitespace(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(nameof(H5pZipSourcePath));
    }
    private static void ThrowArgumentExceptionIfPathContainsInvalidPathChars(string value)
    {
        if (value.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            throw new ArgumentException("H5pZipSourcePath contains invalid path chars!");
    }


    public H5pDisplayMode ActiveDisplayMode { get; set; }

  
}