﻿using H5pPlayer.BusinessLogic.Api.CleanupH5pPlayer;
using H5pPlayer.BusinessLogic.Api.FileSystemDataAccess;

namespace H5pPlayer.BusinessLogic.BusinessRules;

public class TemporaryH5psInWwwrootManager : ICleanupH5pPlayerPort
{
    private IFileSystemDataAccess FileSystemDataAccess { get; }

    internal TemporaryH5psInWwwrootManager(IFileSystemDataAccess fileSystemDataAccess)
    {
        FileSystemDataAccess = fileSystemDataAccess;
    }

    public void CleanDirectoryForTemporaryH5psInWwwroot()
    {
        if (FileSystemDataAccess.DirectoryExists(BuildTemporaryDirectoryFullNameForAllH5ps()))
        {
            FileSystemDataAccess.DeleteAllFilesAndDirectoriesIn(BuildTemporaryDirectoryFullNameForAllH5ps());
        }
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
        var paths = new[]
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