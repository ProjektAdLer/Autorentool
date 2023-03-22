using System.ComponentModel;
using BusinessLogic.Validation;
using Presentation.Components;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Shared;
using Shared.Command;

namespace Presentation.PresentationLogic.LearningWorld;

public interface ILearningWorldPresenter : INotifyPropertyChanged, INotifyPropertyChanging, IPositioningService,
    ILearningSpaceNamesProvider
{
    ILearningWorldViewModel? LearningWorldVm { get; }
    bool SelectedLearningObjectIsSpace { get; }
    bool ShowingLearningSpaceView { get; }
    void DeleteSelectedLearningObject();

    /// <summary>
    /// Creates a new learning space in the currently selected learning world.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="shortname"></param>
    /// <param name="authors"></param>
    /// <param name="description"></param>
    /// <param name="goals"></param>
    /// <param name="requiredPoints"></param>
    /// <param name="positionX"></param>
    /// <param name="positionY"></param>
    /// <exception cref="ApplicationException">Thrown if no learning world is currently selected.</exception>
    void CreateLearningSpace(string name, string shortname, string authors, string description, string goals,
        int requiredPoints, double positionX = 0, double positionY = 0);

    Task LoadLearningSpaceAsync();
    Task SaveLearningWorldAsync();
    Task SaveSelectedLearningSpaceAsync();
    void ShowSelectedLearningSpaceView();
    void CloseLearningSpaceView();
    void DeletePathWayCondition(PathWayConditionViewModel pathWayCondition);
    void OnWorkspacePropertyChanged(object? caller, PropertyChangedEventArgs e);
    event EventHandler<CommandUndoRedoOrExecuteArgs> OnCommandUndoRedoOrExecute;
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