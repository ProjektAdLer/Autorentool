using System.IO.Abstractions;
using System.IO.Compression;

namespace DataAccess.Extensions;
/// <inheritdoc cref="ZipExtensions"/>
public class ZipExtensionsWrapper
{
    /// <summary>
    /// Gets the file system used by this wrapper.
    /// </summary>
    public IFileSystem FileSystem { get; private set; }

    
    public ZipExtensionsWrapper(IFileSystem fileSystem)
    {
        FileSystem = fileSystem;
    }
    
    /// <summary>
    /// Opens a zip archive in read mode.
    /// </summary>
    /// <param name="archivePath">The path to the zip archive file.</param>
    /// <returns>A <see cref="ZipArchive"/> instance representing the archive.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="archivePath"/> is null.</exception>
    /// <exception cref="FileNotFoundException">Thrown if the specified file does not exist.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the caller does not have the required permissions.</exception>
    /// <exception cref="InvalidDataException">Thrown if the file is not a valid zip archive.</exception>
    public ZipArchive GetZipArchive(string archivePath)
    {
        return ZipExtensions.GetZipArchive(FileSystem, archivePath);
    }

    /// <summary>
    /// Extracts all entries from a zip archive to the specified directory.
    /// </summary>
    /// <param name="archive">The zip archive to extract.</param>
    /// <param name="destination">The directory to extract the archive's contents to.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="destination"/> is null.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the caller does not have write access to the destination directory.</exception>
    /// <exception cref="IOException">Thrown if an I/O error occurs during extraction.</exception>
    public void ExtractToDirectory(ZipArchive archive, string destination)
    {
        archive.ExtractToDirectory(FileSystem, destination);
    }

    /// <summary>
    /// Creates a zip archive from the contents of a directory asynchronously.
    /// </summary>
    /// <param name="sourcePath">The source directory to compress.</param>
    /// <param name="destinationPath">The path of the resulting zip archive.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="sourcePath"/> or <paramref name="destinationPath"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="sourcePath"/> or <paramref name="destinationPath"/> is invalid. This includes paths with invalid characters, empty strings, or relative paths.</exception>
    /// <exception cref="DirectoryNotFoundException">Thrown if the source directory does not exist.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the caller does not have the required permissions.</exception>
    /// <exception cref="IOException">Thrown if an I/O error occurs during compression.</exception>
    public async Task CreateFromDirectoryAsync(string sourcePath, string destinationPath)
    {
        await ZipExtensions.CreateFromDirectoryAsync(FileSystem, sourcePath, destinationPath);
    }
}