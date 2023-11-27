using System.IO.Abstractions;
using Shared;

namespace DataAccess.Persistence;

public interface ILearningWorldSavePathsHandler
{
    /// <summary>
    /// Deserializes the saved learning world paths file from disk.
    /// </summary>
    /// <returns>The save paths of the saved learning worlds.</returns>
    IEnumerable<IFileInfo> GetSavedLearningWorldPaths();
}