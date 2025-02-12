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
    
    /// <inheritdoc cref="ZipExtensions.GetZipArchive"/>
    public ZipArchive GetZipArchive(string archivePath)
    {
        return ZipExtensions.GetZipArchive(FileSystem, archivePath);
    }

    /// <inheritdoc cref="ZipExtensions.ExtractToDirectory"/>
    public void ExtractToDirectory(ZipArchive archive, string destination)
    {
        ZipExtensions.ExtractToDirectory(archive, FileSystem, destination);
    }

    /// <inheritdoc cref="ZipExtensions.CreateFromDirectoryAsync"/>
    public async Task CreateFromDirectoryAsync(string sourcePath, string destinationPath)
    {
        await ZipExtensions.CreateFromDirectoryAsync(FileSystem, sourcePath, destinationPath);
    }
}