using System.IO.Abstractions;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.SelectedViewModels;

namespace Presentation.PresentationLogic.MyLearningWorlds;

public class MyLearningWorldsProvider : IMyLearningWorldsProvider
{
    private readonly FileInfoFullNameComparer _fileInfoComparer;

    public MyLearningWorldsProvider(ILogger<MyLearningWorldsProvider> logger,
        IPresentationLogic presentationLogic,
        IAuthoringToolWorkspacePresenter workspacePresenter,
        ISelectedViewModelsProvider selectedViewModelsProvider
    )
    {
        PresentationLogic = presentationLogic;
        WorkspacePresenter = workspacePresenter;
        Logger = logger;
        SelectedViewModelsProvider = selectedViewModelsProvider;
        _fileInfoComparer = new FileInfoFullNameComparer();
    }

    internal ILogger<MyLearningWorldsProvider> Logger { get; }
    internal IPresentationLogic PresentationLogic { get; }
    internal IAuthoringToolWorkspacePresenter WorkspacePresenter { get; }
    internal ISelectedViewModelsProvider SelectedViewModelsProvider { get; }
    internal IAuthoringToolWorkspaceViewModel WorkspaceVm => WorkspacePresenter.AuthoringToolWorkspaceVm;

    private IEnumerable<IFileInfo?> LoadedWorldsFileInfos =>
        WorkspacePresenter.AuthoringToolWorkspaceVm.LearningWorlds.Select(world => PresentationLogic
            .GetFileInfoForLearningWorld(world));

    public void ReloadLearningWorldsInWorkspace()
    {
        var newPaths = GetSavedLearningWorldPaths().Except(LoadedWorldsFileInfos, _fileInfoComparer).Cast<IFileInfo>();
        foreach (var newPath in newPaths)
        {
            PresentationLogic.LoadLearningWorldFromPath(WorkspaceVm, newPath.FullName, false);
        }
    }

    public IFileInfo? GetFileInfoFromPath(string path)
    {
        return LoadedWorldsFileInfos.SingleOrDefault(fileInfo => fileInfo?.FullName == path);
    }

    private IEnumerable<IFileInfo> GetSavedLearningWorldPaths()
    {
        return PresentationLogic.GetSavedLearningWorldPaths();
    }
}