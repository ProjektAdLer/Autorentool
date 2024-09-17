namespace H5pPlayer.BusinessLogic.Domain;

public class H5pEntity
{
    private string h5pJsonSourcePath;


    

    /// <exception cref="ArgumentNullException">If path is null.</exception>
    /// <exception cref="ArgumentException">If path is empty or whitespace.</exception>
    /// <exception cref="ArgumentException">If path contains invalid path chars from
    /// <see cref="System.IO.Path.GetInvalidPathChars()"/></exception>
    public string H5pJsonSourcePath
    {
        get => h5pJsonSourcePath;
        set
        {
            ThrowExceptionIfPathToSetIsNotValid(value);
            h5pJsonSourcePath = value;
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
            throw new ArgumentNullException(nameof(H5pJsonSourcePath));
    }
    
    private static void ThrowArgumentExceptionIfStringIsEmpty(string value)
    {
        if (value == string.Empty)
            throw new ArgumentException(nameof(H5pJsonSourcePath));
    }
    
    private static void ThrowArgumentExceptionIfStringIsNullOrWhitespace(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(nameof(H5pJsonSourcePath));
    }
    private static void ThrowArgumentExceptionIfPathContainsInvalidPathChars(string value)
    {
        if (value.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            throw new ArgumentException("H5pJsonSourcePath contains invalid path chars!");
    }

 
    
  
}