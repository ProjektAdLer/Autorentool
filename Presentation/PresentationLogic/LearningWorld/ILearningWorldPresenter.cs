using System.ComponentModel;
using BusinessLogic.Validation;
using Presentation.Components;
using Presentation.Components.ModalDialog;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;

namespace Presentation.PresentationLogic.LearningWorld;

public interface ILearningWorldPresenter : INotifyPropertyChanged, INotifyPropertyChanging, IPositioningService, ILearningSpaceNamesProvider
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
    void AddNewLearningSpace();
    void AddNewPathWayCondition();
    void DeletePathWayCondition(PathWayConditionViewModel pathWayCondition);
    void OnWorkspacePropertyChanged(object? caller, PropertyChangedEventArgs e);
    event Action OnUndoRedoPerformed;
    void DragObjectInPathWay(object sender, DraggedEventArgs<IObjectInPathWayViewModel> draggedEventArgs);
    void RightClickOnObjectInPathWay(IObjectInPathWayViewModel objectInPathWay);
    void ClickOnObjectInWorld(ISelectableObjectInWorldViewModel obj);
    void DoubleClickOnLearningSpaceInWorld(IObjectInPathWayViewModel obj);
    void SwitchPathWayCondition(PathWayConditionViewModel pathWayCondition);
    void HideRightClickMenu();
    IObjectInPathWayViewModel? RightClickedLearningObject { get; }
    void EditObjectInPathWay(IObjectInPathWayViewModel obj);
    void DeleteLearningSpace(ILearningSpaceViewModel obj);
    void DeleteLearningObject(IObjectInPathWayViewModel obj);
}