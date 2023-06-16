using Shared;

namespace Presentation.PresentationLogic.MyLearningWorlds;

public interface IMyLearningWorldsProvider
{
    IEnumerable<SavedLearningWorldPath> GetLoadedLearningWorlds();
    IEnumerable<SavedLearningWorldPath> GetSavedLearningWorlds();
    void OpenLearningWorld(SavedLearningWorldPath savedLearningWorldPath);
    void CreateLearningWorld();
    void DeletePathFromSavedLearningWorlds(SavedLearningWorldPath savedLearningWorldPath);
    Task<bool> LoadSavedLearningWorld();
    Task DeleteLearningWorld(SavedLearningWorldPath savedLearningWorldPath);
}