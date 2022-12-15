using System.ComponentModel;
using Presentation.Components;
using Presentation.Components.ModalDialog;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;

namespace Presentation.PresentationLogic.LearningWorld;

public interface ILearningWorldPresenter : INotifyPropertyChanged, INotifyPropertyChanging, IPositioningService
{
    bool CreateLearningSpaceDialogOpen { get; }
    bool CreatePathWayConditionDialogOpen { get; }
    bool CreateTopicDialogOpen { get; }
    bool EditLearningSpaceDialogOpen { get; }
    bool EditPathWayConditionDialogOpen { get; }
    bool EditTopicDialogOpen { get; }
    bool DeleteTopicDialogOpen { get; }
    Dictionary<string, string>? EditSpaceDialogInitialValues { get; }
    Dictionary<string, string>? EditConditionDialogInitialValues { get; }
    List<string>? EditTopicDialogInitialValues { get; }
    List<string>? DeleteTopicDialogInitialValues { get; }
    Dictionary<string, string>? EditSpaceDialogAnnotations { get; }
    ILearningWorldViewModel? LearningWorldVm { get; }
    bool SelectedLearningObjectIsSpace { get; }
    bool ShowingLearningSpaceView { get; }
    void DeleteSelectedLearningObject();
    Task LoadLearningSpaceAsync();
    Task SaveSelectedLearningSpaceAsync();
    void OnCreateSpaceDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void OnCreatePathWayConditionDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void OnCreateTopicDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void OpenEditSelectedObjectDialog();
    void OpenEditTopicDialog();
    void OnEditPathWayConditionDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void OnEditSpaceDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void OnEditTopicDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void ShowSelectedLearningSpaceView();
    void CloseLearningSpaceView();
    void AddNewLearningSpace();
    void AddNewPathWayCondition();
    void AddNewTopic();
    void DeletePathWayCondition(PathWayConditionViewModel pathWayCondition);
    void OpenDeleteTopicDialog();
    void OnDeleteTopicDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void OnWorkspacePropertyChanged(object? caller, PropertyChangedEventArgs e);
    event Action OnUndoRedoPerformed;
    void DragObjectInPathWay(object sender, DraggedEventArgs<IObjectInPathWayViewModel> draggedEventArgs);
    void RightClickOnObjectInPathWay(IObjectInPathWayViewModel objectInPathWay);
    void ClickOnObjectInWorld(ISelectableObjectInWorldViewModel obj);
    void DoubleClickOnObjectInWorld(IObjectInPathWayViewModel obj);
    void HideRightClickMenu();
    IObjectInPathWayViewModel? RightClickedLearningObject { get; }
    void EditObjectInPathWay(IObjectInPathWayViewModel obj);
    void DeleteLearningSpace(ILearningSpaceViewModel obj);
}