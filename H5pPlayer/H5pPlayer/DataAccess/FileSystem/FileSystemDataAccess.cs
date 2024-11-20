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
        PathValidator = new PathValidator();
    }

    /// <summary>
    /// Constructor for Tests
    /// </summary>
    public FileSystemDataAccess(IFileSystem fileSystem)
    {
        FileSystem = fileSystem;
        PathValidator = new PathValidator();
    }


    /// <inheritdoc cref="IFileSystemDataAccess.ExtractZipFile"/>
    public void ExtractZipFile(string sourceArchiveFileName, string destinationDirectoryName)
    {
        var zipArchive = ZipExtensions.GetZipArchive(FileSystem, sourceArchiveFileName);
        zipArchive.ExtractToDirectory(FileSystem, destinationDirectoryName);
    }


    /// <inheritdoc cref="IFileSystemDataAccess.DeleteAllFilesAndDirectoriesIn"/>
    public void DeleteAllFilesAndDirectoriesIn(string directoryForCleaning)
    {
        PathValidator.ThrowArgumentNullExceptionIfPathIsNull(directoryForCleaning, nameof(directoryForCleaning));
        PathValidator.ThrowArgumentExceptionIfPathIsEmpty(directoryForCleaning, nameof(directoryForCleaning));
        ThrowExceptionIfDirectoryNotExists(directoryForCleaning);
        DeleteAllFilesIn(directoryForCleaning);
        DeleteAllDirectoriesIn(directoryForCleaning);
    }

    private void ThrowExceptionIfDirectoryNotExists(string directory)
    {
        if (!FileSystem.Directory.Exists(directory))
        {
            throw new DirectoryNotFoundException($"The directory '{directory}' does not exist.");
        }
    }

    private void DeleteAllFilesIn(string directoryForCleaning)
    {
        foreach (var file in FileSystem.Directory.GetFiles(directoryForCleaning))
        {
            FileSystem.File.Delete(file);
        }
    }

    private void DeleteAllDirectoriesIn(string directoryForCleaning)
    {
        foreach (var directory in FileSystem.Directory.GetDirectories(directoryForCleaning))
        {
            FileSystem.Directory.Delete(directory, true);
        }
    }


    private IFileSystem FileSystem { get; }
    private PathValidator PathValidator { get; }
}