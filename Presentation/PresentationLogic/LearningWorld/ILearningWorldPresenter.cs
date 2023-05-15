using System.ComponentModel;
using BusinessLogic.Validation;
using Presentation.Components;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.Topic;
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
    /// <param name="description"></param>
    /// <param name="goals"></param>
    /// <param name="requiredPoints"></param>
    /// <param name="theme"></param>
    /// <param name="positionX"></param>
    /// <param name="positionY"></param>
    /// <param name="topic"></param>
    /// <exception cref="ApplicationException">Thrown if no learning world is currently selected.</exception>
    void CreateLearningSpace(string name, string description, string goals,
        int requiredPoints, Theme theme, double positionX = 0, double positionY = 0, TopicViewModel? topic = null);

    Task LoadLearningSpaceAsync();
    void EditLearningWorld(string name, string shortname, string authors, string language, string description, string goals);
    Task SaveLearningWorldAsync();
    Task SaveSelectedLearningSpaceAsync();
    void ShowSelectedLearningSpaceView();
    void CloseLearningSpaceView();
    void DeletePathWayCondition(PathWayConditionViewModel pathWayCondition);
    void OnSelectedViewModelsProviderOnPropertyChanged(object? caller, PropertyChangedEventArgs e);
    event EventHandler<CommandUndoRedoOrExecuteArgs> OnCommandUndoRedoOrExecute;
    void DragObjectInPathWay(object sender, DraggedEventArgs<IObjectInPathWayViewModel> draggedEventArgs);
    void RightClickOnObjectInPathWay(IObjectInPathWayViewModel learningSpace);
    void ClickOnObjectInWorld(ISelectableObjectInWorldViewModel obj);
    void DoubleClickOnObjectInPathway(IObjectInPathWayViewModel obj);
    void SwitchPathWayCondition(PathWayConditionViewModel pathWayCondition);
    void HideRightClickMenu();
    IObjectInPathWayViewModel? RightClickedLearningObject { get; }
    void SetSelectedLearningSpace(IObjectInPathWayViewModel obj);
    void DeleteLearningSpace(ILearningSpaceViewModel obj);
    void DeleteLearningObject(IObjectInPathWayViewModel obj);
    void CreatePathWayCondition(ConditionEnum condition = ConditionEnum.Or);
    void SetSelectedLearningElement(ILearningElementViewModel learningElement);

    void EditLearningElement(ILearningSpaceViewModel? elementParent, ILearningElementViewModel learningElement,
        string name,string description, string goals, LearningElementDifficultyEnum difficulty,
        int workload, int points, ILearningContentViewModel learningContent);

    IEnumerable<ILearningContentViewModel> GetAllContent();

    void CreateUnplacedLearningElement(string name, ILearningContentViewModel learningContent, string description,
        string goals, LearningElementDifficultyEnum difficulty, int workload, int points);
    Task ShowSelectedElementContentAsync(ILearningElementViewModel learningElement);

    void DeleteLearningElement(ILearningElementViewModel learningElement);
    void AddNewLearningSpace();
    void EditSelectedLearningSpace();
}