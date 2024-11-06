using H5pPlayer.BusinessLogic.Api.FileSystemDataAccess;

namespace H5pPlayer.BusinessLogic.BusinessRules;

public class TemporaryArchiveManager
{
    private IFileSystemDataAccess FileSystemDataAccess { get; }

    public TemporaryArchiveManager(IFileSystemDataAccess fileSystemDataAccess)
    {
        FileSystemDataAccess = fileSystemDataAccess;
    }

    public void CleanDirectoryForTemporaryH5psInWwwroot()
    {
        FileSystemDataAccess.DeleteAllFilesInDirectory(BuildTemporaryDirectoryFullNameForAllH5ps());
    }

 
    /// <summary>
    /// to reach the wwwroot-directory we need a path like that:
    /// C:\Users\%USERPROFILE%\Documents\GitHub\Autorentool\AuthoringTool\
    ///     We get this from: <see cref="Environment.CurrentDirectory"/>
    /// And this:
    ///    wwwroot\H5pStandalone\h5p-folder
    /// which is currently hard coded.
    ///
    /// To get a Path like this
    /// C:\Users\%USERPROFILE%\Documents\GitHub\Autorentool\AuthoringTool\wwwroot\H5pStandalone\h5p-folder\h5pFileNameWithoutExtension
    /// </summary>
    public string BuildTemporaryDirectoryFullNameForOneH5p(string h5pFileNameWithoutExtension)
    {
        return Path.Combine(BuildTemporaryDirectoryFullNameForAllH5ps(),h5pFileNameWithoutExtension);
    }
    
    
    private string BuildTemporaryDirectoryFullNameForAllH5ps()
    {
        var paths = new string[]
        {
            Environment.CurrentDirectory,
            "wwwroot",
            "H5pStandalone",
            "h5p-folder",
        };
        var temporaryDirectoryFullName = Path.Combine(paths);
        return temporaryDirectoryFullName;
    }

}