using System.IO.Abstractions;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.SelectedViewModels;
using Presentation.View;
using Shared;

namespace Presentation.PresentationLogic.MyLearningWorlds;

public class MyLearningWorldsProvider : IMyLearningWorldsProvider
{
    public MyLearningWorldsProvider(IPresentationLogic presentationLogic,
        IAuthoringToolWorkspacePresenter workspacePresenter, IFileSystem fileSystem,
        ILogger<MyLearningWorldsProvider> logger, ISelectedViewModelsProvider selectedViewModelsProvider)
    {
        PresentationLogic = presentationLogic;
        WorkspacePresenter = workspacePresenter;
        FileSystem = fileSystem;
        Logger = logger;
        SelectedViewModelsProvider = selectedViewModelsProvider;
    }

    internal ILogger<MyLearningWorldsProvider> Logger { get; }
    internal IPresentationLogic PresentationLogic { get; }

    internal IAuthoringToolWorkspacePresenter WorkspacePresenter { get; }
    internal IFileSystem FileSystem { get; }

    internal ISelectedViewModelsProvider SelectedViewModelsProvider { get; }
    internal IAuthoringToolWorkspaceViewModel WorkspaceVm => WorkspacePresenter.AuthoringToolWorkspaceVm;


    private ExceptionWrapper? ErrorState { get; set; }

    public IEnumerable<SavedLearningWorldPath> GetLoadedLearningWorlds()
    {
        return WorkspaceVm.LearningWorlds.Select(x => new SavedLearningWorldPath
        {
            Id = x.Id,
            Name = x.Name,
            Path = x.SavePath
        });
    }

    public IEnumerable<SavedLearningWorldPath> GetSavedLearningWorlds()
    {
        return PresentationLogic.GetSavedLearningWorldPaths()
            .Where(world => GetLoadedLearningWorlds().All(x => x.Id != world.Id));
    }

    public void OpenLearningWorld(SavedLearningWorldPath savedLearningWorldPath)
    {
        if (GetLoadedLearningWorlds().Any(x => x.Id == savedLearningWorldPath.Id))
        {
            OpenLoadedLearningWorld(savedLearningWorldPath);
        }
        else if (PresentationLogic.GetSavedLearningWorldPaths().Contains(savedLearningWorldPath))
        {
            LoadLearningWorldFromSaved(savedLearningWorldPath);
        }
        else
        {
            Logger.LogDebug("Learning world {} is not loaded and not saved", savedLearningWorldPath.Name);
            throw new ArgumentException("Learning world is not loaded and not saved");
        }
    }

    private void LoadLearningWorldFromSaved(SavedLearningWorldPath savedLearningWorldPath)
    {
        if (SavedPathExists(savedLearningWorldPath.Path))
        {
            Logger.LogDebug("Learning world {} is not loaded, but is saved", savedLearningWorldPath.Name);
            PresentationLogic.LoadLearningWorldFromPath(WorkspaceVm,
                savedLearningWorldPath.Path);
            PresentationLogic.UpdateIdOfSavedLearningWorldPath(savedLearningWorldPath,
                WorkspaceVm.LearningWorlds.Last().Id);
        }
        else
        {
            Logger.LogDebug("Saved learning world {} does not exist anymore", savedLearningWorldPath.Name);
            DeletePathFromSavedLearningWorlds(savedLearningWorldPath);
        }
    }

    private void OpenLoadedLearningWorld(SavedLearningWorldPath savedLearningWorldPath)
    {
        Logger.LogDebug("Learning world with id {} is already loaded", savedLearningWorldPath.Id);
        SelectedViewModelsProvider.SetLearningWorld(
            WorkspaceVm.LearningWorlds.First(x => x.Id == savedLearningWorldPath.Id), null);
    }

    public void DeletePathFromSavedLearningWorlds(SavedLearningWorldPath savedLearningWorldPath)
    {
        PresentationLogic.RemoveSavedLearningWorldPath(savedLearningWorldPath);
    }

    public void CreateLearningWorld()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> LoadSavedLearningWorld()
    {
        try
        {
            var filepath = await PresentationLogic.GetWorldSavePath();
            var savedLearningWorldPath = PresentationLogic.AddSavedLearningWorldPathByPathOnly(filepath);
            LoadLearningWorldFromSaved(savedLearningWorldPath);
            return true;
        }
        catch (OperationCanceledException)
        {
            //nothing to do, perhaps we want to show a notification?
            return false;
        }
        catch (Exception exception)
        {
            ErrorState = new ExceptionWrapper("Loading learning world", exception);
            return false;
        }
    }

    public async Task DeleteLearningWorld(SavedLearningWorldPath savedLearningWorldPath)
    {
        var learningWorld = WorkspaceVm.LearningWorlds.FirstOrDefault(w => w.Id == savedLearningWorldPath.Id);
        if (learningWorld != null)
            await WorkspacePresenter.DeleteLearningWorld(learningWorld);
    }

    private bool SavedPathExists(string path)
    {
        return FileSystem.File.Exists(path);
    }
}