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
            ThrowExceptionIfH5pZipSourcePathIsNotValid(value);
            h5pZipSourcePath = value;
        }
    }


    /// <exception cref="ArgumentNullException">If path is null.</exception>
    /// <exception cref="ArgumentException">If path is empty or whitespace.</exception>
    /// <exception cref="ArgumentException">If path contains invalid path chars from
    /// <see cref="System.IO.Path.GetInvalidPathChars()"/></exception>
    private static void ThrowExceptionIfH5pZipSourcePathIsNotValid(string value)
    {
        PathValidator.ThrowArgumentNullExceptionIfPathIsNull(value, nameof(H5pZipSourcePath));
        PathValidator.ThrowArgumentExceptionIfPathIsEmpty(value, nameof(H5pZipSourcePath));
        PathValidator.ThrowArgumentExceptionIfPathIsNullOrWhitespace(value, nameof(H5pZipSourcePath));
        PathValidator.ThrowArgumentExceptionIfPathContainsInvalidPathChars(value, "H5pZipSourcePath contains invalid path chars!");
    }


    public H5pDisplayMode ActiveDisplayMode { get; set; }

  
}