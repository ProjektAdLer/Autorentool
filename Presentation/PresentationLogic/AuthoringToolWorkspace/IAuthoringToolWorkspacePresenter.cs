using Presentation.Components.ModalDialog;
using Presentation.PresentationLogic.LearningWorld;

namespace Presentation.PresentationLogic.AuthoringToolWorkspace;

public interface IAuthoringToolWorkspacePresenter
{
    IAuthoringToolWorkspaceViewModel AuthoringToolWorkspaceVm { get; }
    bool CreateLearningWorldDialogOpen { get; set; }
    bool EditLearningWorldDialogOpen { get; set; }
    bool SaveUnsavedChangesDialogOpen { get; set; }
    bool LearningWorldSelected { get; }
    LearningWorldViewModel? WorldToReplaceWith { get; set; }
    LearningWorldViewModel? ReplacedUnsavedWorld { get; set; }
    LearningWorldViewModel? DeletedUnsavedWorld { get; set; }
    string? InformationMessageToShow { get; set; }
    Queue<LearningWorldViewModel>? UnsavedWorldsQueue { get; set; }

    event Action? OnForceViewUpdate;

    void AddNewLearningWorld();
    /// <summary>
    /// Sets the selected <see cref="LearningWorldViewModel"/> in the view model.
    /// </summary>
    /// <param name="worldName">The name of the world that should be selected.</param>
    /// <exception cref="ArgumentException">Thrown when no world with that name is registered in the view model.</exception>
    void SetSelectedLearningWorld(string worldName);

    /// <summary>
    /// Deletes the currently selected learning world from the view model and selects the last learning world in the
    /// collection, if any remain.
    /// </summary>
    void DeleteSelectedLearningWorld();
    void OpenEditSelectedLearningWorldDialog();
    Task LoadLearningWorldAsync();
    Task SaveSelectedLearningWorldAsync();
    void OnCreateWorldDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void OnEditWorldDialogClose(ModalDialogOnCloseResult returnValueTuple);
    Task ProcessDragAndDropResult(Tuple<string, Stream> result);
    void OnSaveWorldDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void OnSaveReplacedWorldDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void OnReplaceDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void OnSaveDeletedWorldDialogClose(ModalDialogOnCloseResult returnValueTuple);
}