namespace H5pPlayer.BusinessLogic.Domain;

public class H5pEntity
{

    public H5pEntity()
    {
        ActiveDisplayMode = H5pDisplayMode.Display;
        // init by empty cause of unix mac windows usage. 
        // -> otherwise we must deside the OS here 
        h5pZipSourcePath = string.Empty;
        _pathValidator = new PathValidator();
    }
    
    private string h5pZipSourcePath;
    private readonly PathValidator _pathValidator;

    /// <exception cref="ArgumentNullException">If path is null.</exception>
    /// <exception cref="ArgumentException">If path is empty or whitespace.</exception>
    /// <exception cref="ArgumentException">If path contains invalid path chars from
    /// <see cref="System.IO.Path.GetInvalidPathChars()"/></exception>
    /// <exception cref="ArgumentException">If path do not end with .zip</exception>
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
    /// <exception cref="ArgumentException">If path do not end with .zip</exception>
    private void ThrowExceptionIfH5pZipSourcePathIsNotValid(string value)
    {
        _pathValidator.ThrowArgumentNullExceptionIfPathIsNull(value, nameof(H5pZipSourcePath));
        _pathValidator.ThrowArgumentExceptionIfPathIsEmpty(value, nameof(H5pZipSourcePath));
        _pathValidator.ThrowArgumentExceptionIfPathIsNullOrWhitespace(value, nameof(H5pZipSourcePath));
        _pathValidator.ThrowArgumentExceptionIfPathContainsInvalidPathChars(value, "H5pZipSourcePath contains invalid path chars!");
        ThrowArgumentExceptionIfPathMissesZipExtension(value);
        _pathValidator.ThrowArgumentExceptionIfPathIsNotRooted(value, nameof(H5pZipSourcePath));
    }

    private static void ThrowArgumentExceptionIfPathMissesZipExtension(string value)
    {
        if(PathDoNotContainZipFileEnding(value))
            throw new ArgumentException(nameof(H5pZipSourcePath));
    }

    private static bool PathDoNotContainZipFileEnding(string value)
    {
        return !value.Contains(".zip");
    }

    public H5pDisplayMode ActiveDisplayMode { get; set; }

  
}