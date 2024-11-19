namespace H5pPlayer.BusinessLogic.Api.FileSystemDataAccess;

public interface IFileSystemDataAccess
{

    void ExtractZipFile(string sourceZipFilePath, string destinationDirectoryPath);
    void DeleteDirectoryRecursively(string directoryForCleaning);
}