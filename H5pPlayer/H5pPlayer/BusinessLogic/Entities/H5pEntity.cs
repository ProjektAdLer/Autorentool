using H5pPlayer.General.Path;

namespace H5pPlayer.BusinessLogic.Entities;

public class H5pEntity
{

    private string _h5pZipSourcePath;
    private string _unzippedH5PsPath;
    private readonly PathValidator _pathValidator;

    public H5pEntity()
    {
        ActiveDisplayMode = H5pDisplayMode.Display;
        // init by empty cause of unix mac windows usage. 
        // -> otherwise we must deside the OS here 
        _h5pZipSourcePath = string.Empty;
        _unzippedH5PsPath = string.Empty;
        _pathValidator = new PathValidator();
    }
    
    
    public H5pDisplayMode ActiveDisplayMode { get; set; }

    
    /// <summary>
    /// Path to "H5P-Filename.h5p" for playing in H5P-Player.
    /// This is the source. From this source we copy the H5P to
    /// the temporary directory.
    /// </summary>
    /// <example>
    /// Something like that:
    ///  C:\Users\%USERPROFILE%\AppData\Roaming\AdLerAuthoring\ContentFiles\Accordion_Test.h5p
    /// </example>
    /// <inheritdoc cref="ThrowExceptionIfH5pZipSourcePathIsNotValid"/>
    public string H5pZipSourcePath
    {
        get => _h5pZipSourcePath;
        set
        {
            ThrowExceptionIfH5pZipSourcePathIsNotValid(value);
            _h5pZipSourcePath = value;
        }
    }


    /// <exception cref="ArgumentNullException">If path is null.</exception>
    /// <exception cref="ArgumentException">If path is empty or whitespace.</exception>
    /// <exception cref="ArgumentException">If path contains invalid path chars from
    /// <see cref="System.IO.Path.GetInvalidPathChars()"/></exception>
    /// <exception cref="ArgumentException">If path dos not end with .h5p</exception>
    /// <exception cref="ArgumentException">If path is not rooted</exception>
    private void ThrowExceptionIfH5pZipSourcePathIsNotValid(string path)
    {
        _pathValidator.ThrowArgumentNullExceptionIfPathIsNull(path, nameof(H5pZipSourcePath));
        _pathValidator.ThrowArgumentExceptionIfPathIsEmpty(path, nameof(H5pZipSourcePath) + " was empty or whitespace!");
        _pathValidator.ThrowArgumentExceptionIfPathIsNullOrWhitespace(path, nameof(H5pZipSourcePath) + " was empty or whitespace!");
        _pathValidator.ThrowArgumentExceptionIfPathContainsInvalidPathChars(path, nameof(H5pZipSourcePath) + " contains invalid path chars!");
        ThrowArgumentExceptionIfPathDoNotEndsWithH5pFileExtension(path);
        _pathValidator.ThrowArgumentExceptionIfPathIsNotRooted(path, nameof(H5pZipSourcePath) +  " must be rooted!");
    }

    private static void ThrowArgumentExceptionIfPathDoNotEndsWithH5pFileExtension(string path)
    {
        if(PathDoNotEndsWithH5pFileExtension(path))
            throw new ArgumentException(nameof(H5pZipSourcePath) + " misses .h5p extension!");
    }

    private static bool PathDoNotEndsWithH5pFileExtension(string path)
    {
        return !path.EndsWith(".h5p");
    }

    /// <summary>
    /// Path to directory for temporary stored h5p-files 
    /// <para>
    ///     Attention: directory name must be: h5p-folder
    /// </para>
    /// </summary>
    /// <example>
    /// Something like that:
    ///  https://localhost:8001/H5pStandalone/h5p-folder
    /// </example>
    /// <inheritdoc cref="ThrowExceptionIfUnzippedH5psPathIsNotValid"/>
    public string UnzippedH5psPath
    {
        get => _unzippedH5PsPath;
        set
        {
            ThrowExceptionIfUnzippedH5psPathIsNotValid(value);
            _unzippedH5PsPath = value;
        }
    }
    
    
    
    /// <exception cref="ArgumentNullException">If path is null.</exception>
    /// <exception cref="ArgumentException">If path is empty or whitespace.</exception>
    /// <exception cref="ArgumentException">If path contains invalid path chars from
    /// <see cref="System.IO.Path.GetInvalidPathChars()"/></exception>
    private void ThrowExceptionIfUnzippedH5psPathIsNotValid(string path)
    {
        _pathValidator.ThrowArgumentNullExceptionIfPathIsNull(path, nameof(UnzippedH5psPath));
        _pathValidator.ThrowArgumentExceptionIfPathIsEmpty(path, nameof(UnzippedH5psPath) + " was empty or whitespace!");
        _pathValidator.ThrowArgumentExceptionIfPathIsNullOrWhitespace(path, nameof(UnzippedH5psPath) + " was empty or whitespace!");
        _pathValidator.ThrowArgumentExceptionIfPathContainsInvalidPathChars(path, nameof(UnzippedH5psPath) + " contains invalid path chars!");
    }
}