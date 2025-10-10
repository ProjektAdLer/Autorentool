using H5pPlayer.BusinessLogic.Api.CleanupH5pPlayer;
using H5pPlayer.BusinessLogic.Api.FileSystemDataAccess;
using H5pPlayer.BusinessLogic.Entities;
using Shared.Configuration;

namespace H5pPlayer.BusinessLogic.BusinessRules;

public class TemporaryH5psManager : ICleanupH5pPlayerPort
{
    private IFileSystemDataAccess FileSystemDataAccess { get; }

    internal TemporaryH5psManager(IFileSystemDataAccess fileSystemDataAccess)
    {
        FileSystemDataAccess = fileSystemDataAccess;
    }

    /// <summary>
    /// Documentation for UnzippedH5psPath:
    /// <see cref="H5pEntity.UnzippedH5psPath"/>
    /// </summary>
    public void CleanDirectoryForTemporaryUnzippedH5ps()
    {
        if (FileSystemDataAccess.DirectoryExists(BuildDirectoryFullNameForTemporaryUnzippedH5ps()))
        {
            FileSystemDataAccess.DeleteAllFilesAndDirectoriesIn(BuildDirectoryFullNameForTemporaryUnzippedH5ps());
        }
    }
 
    /// <summary>
    /// to reach the %APPDATA%-Roaming directory we need a path like that:
    /// C:\Users\%USERPROFILE%\AppData\Roaming\
    ///     We get this from: <see cref="Environment.SpecialFolder.ApplicationData"/>
    /// And this:
    ///    AdlerAuthoring\h5p-folder
    /// which is currently hard coded.
    ///
    /// To get a Path like this
    /// C:\Users\%USERPROFILE%\AppData\Roaming\AdLerAuthoring\h5p-folder\h5pFileNameWithoutExtension
    /// </summary>
    public string BuildTemporaryDirectoryFullNameForCurrentH5p(string h5pFileNameWithoutExtension)
    {
        return Path.Combine(BuildDirectoryFullNameForTemporaryUnzippedH5ps(),h5pFileNameWithoutExtension);
    }
    
    /// <summary>
    /// Documentation for UnzippedH5psPath:
    /// <see cref="H5pEntity.UnzippedH5psPath"/>
    /// </summary>
    private string BuildDirectoryFullNameForTemporaryUnzippedH5ps()
    {
        var applicationDataPath = new[]
        {
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "AdLerAuthoring",
            "h5p-folder",
        };
        var temporaryDirectoryFullName = Path.Combine(applicationDataPath);
        return temporaryDirectoryFullName;
    }



}