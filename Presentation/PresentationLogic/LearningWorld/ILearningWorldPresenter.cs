using System.ComponentModel;
using BusinessLogic.Validation;
using Presentation.Components;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Shared;

namespace Presentation.PresentationLogic.LearningWorld;

public interface ILearningWorldPresenter : INotifyPropertyChanged, INotifyPropertyChanging, IPositioningService, ILearningSpaceNamesProvider
{
    ILearningWorldViewModel? LearningWorldVm { get; }
    bool SelectedLearningObjectIsSpace { get; }
    bool ShowingLearningSpaceView { get; }
    void DeleteSelectedLearningObject();
    Task LoadLearningSpaceAsync();
    Task SaveSelectedLearningSpaceAsync();
    void ShowSelectedLearningSpaceView();
    void CloseLearningSpaceView();
    void DeletePathWayCondition(PathWayConditionViewModel pathWayCondition);
    void OnWorkspacePropertyChanged(object? caller, PropertyChangedEventArgs e);
    event Action OnUndoRedoPerformed;
    void DragObjectInPathWay(object sender, DraggedEventArgs<IObjectInPathWayViewModel> draggedEventArgs);
    void RightClickOnObjectInPathWay(IObjectInPathWayViewModel objectInPathWay);
    void ClickOnObjectInWorld(ISelectableObjectInWorldViewModel obj);
    void DoubleClickOnObjectInPathway(IObjectInPathWayViewModel obj);
    void SwitchPathWayCondition(PathWayConditionViewModel pathWayCondition);
    void HideRightClickMenu();
    IObjectInPathWayViewModel? RightClickedLearningObject { get; }
    void EditObjectInPathWay(IObjectInPathWayViewModel obj);
    void DeleteLearningSpace(ILearningSpaceViewModel obj);
    void DeleteLearningObject(IObjectInPathWayViewModel obj);
    void CreatePathWayCondition(ConditionEnum condition = ConditionEnum.Or);
}