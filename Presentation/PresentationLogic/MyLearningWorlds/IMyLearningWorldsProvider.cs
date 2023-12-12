using System.IO.Abstractions;
using Presentation.PresentationLogic.LearningWorld;
using Shared;

namespace Presentation.PresentationLogic.MyLearningWorlds;

public interface IMyLearningWorldsProvider
{
    void ReloadLearningWorldsInWorkspace();
    IFileInfo? GetFileInfoFromPath(string path);
}