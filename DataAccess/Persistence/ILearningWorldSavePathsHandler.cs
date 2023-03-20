using Shared;

namespace DataAccess.Persistence;

public interface ILearningWorldSavePathsHandler
{
    /// <summary>
    /// Deserializes the saved learning world paths file from disk.
    /// </summary>
    /// <returns>The save paths of the saved learning worlds.</returns>
    IEnumerable<SavedLearningWorldPath> GetSavedLearningWorldPaths();
    /// <summary>
    /// Adds a new saved learning world path to the list of saved learning world paths.
    /// </summary>
    /// <param name="savedLearningWorldPath">SavedLearningWorldPath object with name, path and id.</param>
    void AddSavedLearningWorldPath(SavedLearningWorldPath savedLearningWorldPath);
    /// <summary>
    /// Adds a new saved learning world path to the list of saved learning world paths by paths only.
    /// </summary>
    /// <param name="path">Path to the learning world. Name and id are generated automatically.</param>
    /// <returns></returns>
    SavedLearningWorldPath AddSavedLearningWorldPathByPathOnly(string path);
    /// <summary>
    /// Updates the id of a saved learning world path.
    /// </summary>
    /// <param name="savedLearningWorldPath">SavedLearningWorldPath object of the saved learning world to change.</param>
    /// <param name="id">New id for the saved learning world.</param>
    void UpdateIdOfSavedLearningWorldPath(SavedLearningWorldPath savedLearningWorldPath, Guid id);
    /// <summary>
    /// Removes a saved learning world path from the list of saved learning world paths.
    /// </summary>
    /// <param name="savedLearningWorldPath">SavedLearningWorldPath object of the learning world to remove.</param>
    void RemoveSavedLearningWorldPath(SavedLearningWorldPath savedLearningWorldPath);
}