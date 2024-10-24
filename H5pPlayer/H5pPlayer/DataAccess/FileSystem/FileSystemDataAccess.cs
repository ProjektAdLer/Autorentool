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
    

 
    public void ExtractZipFile(string sourceArchiveFileName, string destinationDirectoryName)
    {
        var zipArchive = ZipExtensions.GetZipArchive(FileSystem, sourceArchiveFileName);
        zipArchive.ExtractToDirectory(FileSystem, destinationDirectoryName);
    }
    
    
    
    private IFileSystem FileSystem { get; }

}