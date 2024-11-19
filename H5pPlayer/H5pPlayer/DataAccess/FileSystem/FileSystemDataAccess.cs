using System.IO.Abstractions;
using System.IO.Compression;
using DataAccess.Extensions;
using H5pPlayer.BusinessLogic.Api.FileSystemDataAccess;
using H5pPlayer.General.Path;

namespace H5pPlayer.DataAccess.FileSystem;

public class FileSystemDataAccess : IFileSystemDataAccess
{
    public FileSystemDataAccess()
    { 
        FileSystem = new System.IO.Abstractions.FileSystem();
        
    }
    
    /// <summary>
    /// Constructor for Tests
    /// </summary>
    public FileSystemDataAccess(IFileSystem fileSystem)
    { 
        FileSystem = fileSystem;
        PathValidator = new PathValidator();
        
    }
    
 
    public void ExtractZipFile(string sourceArchiveFileName, string destinationDirectoryName)
    {
        var zipArchive = ZipExtensions.GetZipArchive(FileSystem, sourceArchiveFileName);
        zipArchive.ExtractToDirectory(FileSystem, destinationDirectoryName);
    }

    public void DeleteDirectoryRecursively(string directory)
    {
        PathValidator.ThrowArgumentNullExceptionIfPathIsNull(directory, nameof(directory));
        PathValidator.ThrowArgumentExceptionIfPathIsEmpty(directory, nameof(directory));
        ThrowExceptionIfDirectoryNotExists(directory);
        FileSystem.Directory.Delete(directory, true);
    }

    private void ThrowExceptionIfDirectoryNotExists(string directory)
    {
        if (!FileSystem.Directory.Exists(directory))
        {
            throw new DirectoryNotFoundException($"The directory '{directory}' does not exist.");
        }
    }


    private IFileSystem FileSystem { get; }
    private PathValidator PathValidator { get; }

}