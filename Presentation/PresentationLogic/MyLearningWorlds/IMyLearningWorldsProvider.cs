using System.IO.Abstractions;

namespace Presentation.PresentationLogic.MyLearningWorlds;

public interface IMyLearningWorldsProvider
{
    void ReloadLearningWorldsInWorkspace();
    IFileInfo? GetFileInfoFromPath(string path);
}