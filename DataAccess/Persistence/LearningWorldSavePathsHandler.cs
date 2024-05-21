using System.IO.Abstractions;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Configuration;

namespace DataAccess.Persistence;

public class LearningWorldSavePathsHandler : ILearningWorldSavePathsHandler
{
    internal IFileSystem FileSystem { get; init; }
    internal ILogger<LearningWorldSavePathsHandler> Logger { get; init; }

    public LearningWorldSavePathsHandler(ILogger<LearningWorldSavePathsHandler> logger, IFileSystem fileSystem)
    {
        FileSystem = fileSystem;
        Logger = logger;
        AssertLearningWorldSavePathsFolderExists();
        Logger.LogInformation("LearningWorldSavePathsFolderPath is {LearningWorldSavePathsFolderPath}",
            ApplicationPaths.SavedWorldsFolder);
    }


    /// <inheritdoc cref="ILearningWorldSavePathsHandler.GetSavedLearningWorldPaths"/>
    public IEnumerable<IFileInfo> GetSavedLearningWorldPaths()
    {
        return FileSystem.Directory
            .EnumerateFiles(ApplicationPaths.SavedWorldsFolder)
            .Where(FilterWorldPaths)
            .Select(FileSystem.FileInfo.New);
    }

    private bool FilterWorldPaths(string path)
    {
        return FileSystem.Path.GetExtension(path) == FileEndings.WorldFileEndingWithDot;
    }

    private void AssertLearningWorldSavePathsFolderExists()
    {
        if (FileSystem.Directory.Exists(ApplicationPaths.SavedWorldsFolder)) return;
        Logger.LogDebug("Folder {LearningWorldSavePathsFolderPath} did not exist, creating",
            ApplicationPaths.SavedWorldsFolder);
        FileSystem.Directory.CreateDirectory(ApplicationPaths.SavedWorldsFolder);
    }
}