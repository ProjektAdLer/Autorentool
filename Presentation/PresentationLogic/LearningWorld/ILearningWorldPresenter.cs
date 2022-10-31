using System.ComponentModel;
using Presentation.Components;
using Presentation.Components.ModalDialog;
using Presentation.PresentationLogic.LearningContent;

namespace Presentation.PresentationLogic.LearningWorld;

public interface ILearningWorldPresenter : INotifyPropertyChanged, INotifyPropertyChanging, IPositioningService
{
    bool CreateLearningSpaceDialogOpen { get; }
    bool CreatePathWayConditionDialogOpen { get; }
    bool EditLearningSpaceDialogOpen { get; }
    bool EditPathWayConditionDialogOpen { get; }
    Dictionary<string, string>? EditSpaceDialogInitialValues { get; }
    Dictionary<string, string>? EditConditionDialogInitialValues { get; }
    Dictionary<string, string>? EditSpaceDialogAnnotations { get; }
    ILearningWorldViewModel? LearningWorldVm { get; }
    bool SelectedLearningObjectIsSpace { get; }
    bool ShowingLearningSpaceView { get; }
    void SetSelectedLearningObject(ISelectableObjectInWorldViewModel learningObject);
    void DeleteSelectedLearningObject();
    Task LoadLearningSpaceAsync();
    Task SaveSelectedLearningSpaceAsync();
    void OnCreateSpaceDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void OnCreatePathWayConditionDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void OpenEditSelectedObjectDialog();
    void OnEditPathWayConditionDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void OnEditSpaceDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void ShowSelectedLearningSpaceView();
    void CloseLearningSpaceView();
    LearningContentViewModel? DragAndDropLearningContent { get; }
    void AddNewLearningSpace();
    void AddNewPathWayCondition();
    void OnWorkspacePropertyChanged(object? caller, PropertyChangedEventArgs e);
    event Action OnUndoRedoPerformed;
    void DragObjectInPathWay(object sender, DraggedEventArgs<IObjectInPathWayViewModel> draggedEventArgs);
}