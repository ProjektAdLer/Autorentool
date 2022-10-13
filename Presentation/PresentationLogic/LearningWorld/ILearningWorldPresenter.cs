using System.ComponentModel;
using Presentation.Components.ModalDialog;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningSpace;

namespace Presentation.PresentationLogic.LearningWorld;

public interface ILearningWorldPresenter : INotifyPropertyChanged, INotifyPropertyChanging, IPositioningService
{
    bool CreateLearningSpaceDialogOpen { get; }
    bool EditLearningSpaceDialogOpen { get; }
    Dictionary<string, string>? EditSpaceDialogInitialValues { get; }
    Dictionary<string, string>? EditSpaceDialogAnnotations { get; }
    ILearningWorldViewModel? LearningWorldVm { get; }
    bool SelectedLearningObjectIsSpace { get; }
    bool ShowingLearningSpaceView { get; }
    void SetSelectedLearningSpace(ILearningSpaceViewModel learningSpace);
    void DeleteSelectedLearningSpace();
    Task LoadLearningSpaceAsync();
    Task SaveSelectedLearningSpaceAsync();
    void OnCreateSpaceDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void OpenEditSelectedLearningSpaceDialog();
    void OnEditSpaceDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void ShowSelectedLearningSpaceView();
    void CloseLearningSpaceView();
    LearningContentViewModel? DragAndDropLearningContent { get; }
    void AddNewLearningSpace();
    void OnWorkspacePropertyChanged(object? caller, PropertyChangedEventArgs e);
    event Action OnUndoRedoPerformed;
    void DragLearningSpace((ILearningSpaceViewModel, double, double) obj);
}