using System.IO.Abstractions;
using System.IO.Compression;
using DataAccess.Extensions;
using H5pPlayer.BusinessLogic.Api.FileSystemDataAccess;

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
    }
    

 
    public void ExtractZipFile(string sourceArchiveFileName, string destinationDirectoryName)
    {
        var zipArchive = ZipExtensions.GetZipArchive(FileSystem, sourceArchiveFileName);
        zipArchive.ExtractToDirectory(FileSystem, destinationDirectoryName);
    }

    /// <exception cref="DirectoryNotFoundException"></exception>
    public void DeleteAllFilesInDirectory(string directory)
    {
        ThrowExceptionIfDirectoryDoesNotExist(directory);
        DeleteAllFilesIn(directory);
        DeleteAllSubDirectoriesIn(directory);
    }
    private void ThrowExceptionIfDirectoryDoesNotExist(string directory)
    {
        if (string.IsNullOrWhiteSpace(directory) || !FileSystem.Directory.Exists(directory))
            throw new DirectoryNotFoundException("The specified directory does not exist or the path is empty.");
    }
    private void DeleteAllFilesIn(string directory)
    {
        foreach (var file in FileSystem.Directory.GetFiles(directory))
        {
            FileSystem.File.Delete(file);
        }
    }
    private void DeleteAllSubDirectoriesIn(string directory)
    {
        foreach (var subDirectory in FileSystem.Directory.GetDirectories(directory))
        {
            FileSystem.Directory.Delete(subDirectory, true);
        }
    }




    private IFileSystem FileSystem { get; }

}