using H5pPlayer.BusinessLogic.Api.FileSystemDataAccess;

namespace H5pPlayer.BusinessLogic.BusinessRules;

public class TemporaryArchiveManager
{
    private IFileSystemDataAccess FileSystemDataAccess { get; }

    public TemporaryArchiveManager(IFileSystemDataAccess fileSystemDataAccess)
    {
        FileSystemDataAccess = fileSystemDataAccess;
    }
    public void CleanDirectoryForTemporaryH5psInWwwroot(string directoryForCleaning)
    {
        FileSystemDataAccess.DeleteAllFilesInDirectory(directoryForCleaning);
    }
}