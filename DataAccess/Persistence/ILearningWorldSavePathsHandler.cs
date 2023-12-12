using System.IO.Abstractions;
using Shared;
using Shared.Configuration;

namespace DataAccess.Persistence;

public interface ILearningWorldSavePathsHandler
{
    /// <summary>
    /// Gets all paths to learning world files saved in <see cref="ApplicationPaths.SavedWorldsFolder"/>.
    /// </summary>
    /// <returns>An enumerable of <see cref="IFileInfo"/>.</returns>
    IEnumerable<IFileInfo> GetSavedLearningWorldPaths();
}