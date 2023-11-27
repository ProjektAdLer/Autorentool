using System.IO.Abstractions;
using System.Xml.Serialization;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Configuration;

namespace DataAccess.Persistence;

public class LearningWorldSavePathsHandler : ILearningWorldSavePathsHandler
{
    private readonly IFileSystem _fileSystem;
    private readonly ILogger<LearningWorldSavePathsHandler> _logger;

    public LearningWorldSavePathsHandler(ILogger<LearningWorldSavePathsHandler> logger, IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
        _logger = logger;
        AssertLearningWorldSavePathsFolderExists();
        _logger.LogInformation("LearningWorldSavePathsFolderPath is {LearningWorldSavePathsFolderPath}",
            ApplicationPaths.SavedWorldsFolder);

    }


    /// <inheritdoc cref="ILearningWorldSavePathsHandler.GetSavedLearningWorldPaths"/>
    public IEnumerable<IFileInfo> GetSavedLearningWorldPaths()
    {
        return _fileSystem.Directory.EnumerateFiles(ApplicationPaths.SavedWorldsFolder).Where(FilterWorldPaths).Select(_fileSystem.FileInfo.FromFileName);
    }

    private bool FilterWorldPaths(string path)
    {
        return _fileSystem.Path.GetExtension(path) == FileEndings.WorldFileEndingWithDot;
    }

    private void AssertLearningWorldSavePathsFolderExists()
    {
        if (_fileSystem.Directory.Exists(ApplicationPaths.SavedWorldsFolder)) return;
        _logger.LogDebug("Folder {LearningWorldSavePathsFolderPath} did not exist, creating",
            ApplicationPaths.SavedWorldsFolder);
        _fileSystem.Directory.CreateDirectory(ApplicationPaths.SavedWorldsFolder);
    }
}