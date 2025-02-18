namespace H5pPlayer.BusinessLogic.Api.FileSystemDataAccess;

public interface IFileSystemDataAccess
{

    /// <summary>
    /// Extracts all files from a ZIP archive to a specified destination directory.
    /// </summary>
    /// <param name="sourceArchiveFileName">
    /// The full path to the ZIP archive to be extracted.
    /// </param>
    /// <param name="destinationDirectoryName">
    /// The full path to the directory where the files should be extracted.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="sourceArchiveFileName"/> or <paramref name="destinationDirectoryName"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="FileNotFoundException">
    /// Thrown if the specified ZIP archive file does not exist.
    /// </exception>
    /// <exception cref="DirectoryNotFoundException">
    /// Thrown if the specified destination directory path is invalid.
    /// </exception>
    /// <exception cref="UnauthorizedAccessException">
    /// Thrown if the caller does not have the required permission to access the file or directory.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// Thrown if the ZIP archive is invalid or corrupted.
    /// </exception>
    /// <exception cref="IOException">
    /// Thrown if an I/O error occurs during the extraction process.
    /// </exception>
    /// <exception cref="PathTooLongException">
    /// Thrown if the specified path, file name, or combined path and file name exceeds the system-defined maximum length.
    /// </exception>
    /// <remarks>
    /// This method uses the file system abstraction provided by the <c>FileSystem</c> object. Ensure that the 
    /// <c>FileSystem</c> is properly initialized before calling this method.
    /// </remarks>
    void ExtractZipFile(string sourceArchiveFileName, string destinationDirectoryName);
    
    /// <summary>
    /// Deletes all files and subdirectories in the specified directory.
    /// The directory itself remains intact.
    /// </summary>
    /// <param name="directoryForCleaning">The path to the directory that should be cleaned.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="directoryForCleaning"/> is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="directoryForCleaning"/> is an empty string or contains only whitespace.
    /// </exception>
    /// <exception cref="DirectoryNotFoundException">
    /// Thrown if the specified directory does not exist.
    /// </exception>
    /// <exception cref="UnauthorizedAccessException">
    /// Thrown if the caller does not have the required permissions to access or modify the files or directories.
    /// </exception>
    /// <exception cref="IOException">
    /// Thrown if an I/O error occurs while deleting a file or directory. This can happen if a file or directory
    /// is in use by another process.
    /// </exception>
    /// <exception cref="PathTooLongException">
    /// Thrown if the specified path, or a file/directory within the path, exceeds the system-defined maximum length.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// Thrown if the format of <paramref name="directoryForCleaning"/> is invalid.
    /// </exception>
    void DeleteAllFilesAndDirectoriesIn(string directoryForCleaning);
}