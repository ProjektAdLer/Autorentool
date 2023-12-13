namespace Shared;

public static class FileEndings
{
    /// <summary>
    /// File ending of world files (without the dot).
    /// </summary>
    public static readonly string WorldFileEnding = "awf";
    
    /// <summary>
    /// File ending of space files (without the dot).
    /// </summary>
    public static readonly string SpaceFileEnding = "asf";
    
    /// <summary>
    /// File ending of element files (without the dot).
    /// </summary>
    public static readonly string ElementFileEnding = "aef";
    
    /// <summary>
    /// File ending of world files (with the dot).
    /// </summary>
    public static readonly string WorldFileEndingWithDot = $".{WorldFileEnding}";
    
    /// <summary>
    /// File ending of space files (with the dot).
    /// </summary>
    public static readonly string SpaceFileEndingWithDot = $".{SpaceFileEnding}";
    
    /// <summary>
    /// File ending of element files (with the dot).
    /// </summary>
    public static readonly string ElementFileEndingWithDot = $".{ElementFileEnding}";
    
}