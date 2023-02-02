using Presentation.Components.ModalDialog;
using Presentation.PresentationLogic.World;

namespace Presentation.PresentationLogic.AuthoringToolWorkspace;

public interface IAuthoringToolWorkspacePresenter
{
    IAuthoringToolWorkspaceViewModel AuthoringToolWorkspaceVm { get; }
    bool CreateWorldDialogOpen { get; set; }
    bool EditWorldDialogOpen { get; set; }
    bool SaveUnsavedChangesDialogOpen { get; set; }
    bool WorldSelected { get; }
    WorldViewModel? DeletedUnsavedWorld { get; set; }
    string? InformationMessageToShow { get; set; }
    Queue<WorldViewModel>? UnsavedWorldsQueue { get; set; }

    event Action? OnForceViewUpdate;

    void AddNewWorld();
    /// <summary>
    /// Sets the selected <see cref="WorldViewModel"/> in the view model.
    /// </summary>
    /// <param name="worldName">The name of the world that should be selected.</param>
    /// <exception cref="ArgumentException">Thrown when no world with that name is registered in the view model.</exception>
    void SetSelectedWorld(string worldName);

    /// <summary>
    /// Deletes the currently selected world from the view model and selects the last world in the
    /// collection, if any remain.
    /// </summary>
    void DeleteSelectedWorld();
    void OpenEditSelectedWorldDialog();
    Task LoadWorldAsync();
    Task SaveSelectedWorldAsync();
    void OnCreateWorldDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void OnEditWorldDialogClose(ModalDialogOnCloseResult returnValueTuple);
    Task ProcessDragAndDropResult(Tuple<string, MemoryStream> result);
    void OnSaveWorldDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void OnSaveDeletedWorldDialogClose(ModalDialogOnCloseResult returnValueTuple);
}