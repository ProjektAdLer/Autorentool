using System.ComponentModel;
using BusinessLogic.Validation;
using Presentation.Components;
using Presentation.Components.Forms.Models;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningSpace.LearningOutcomeViewModel;
using Presentation.PresentationLogic.Topic;
using Shared;
using Shared.Command;
using Shared.Theme;

namespace Presentation.PresentationLogic.LearningWorld;

public interface ILearningWorldPresenter : INotifyPropertyChanged, INotifyPropertyChanging, IPositioningService,
    ILearningSpaceNamesProvider, ISelectedSetter
{
    /// <summary>
    /// The currently selected LearningWorldViewModel.
    /// </summary>
    ILearningWorldViewModel? LearningWorldVm { get; }

    /// <summary>
    /// Deletes the selected learning object in the currently selected learning world and sets an other space or element as selected learning object.
    /// </summary>
    void DeleteSelectedLearningObject();

    /// <summary>
    /// Creates a new learning space in the currently selected learning world.
    /// </summary>
    /// <param name="name">The name of the learning space.</param>
    /// <param name="description">The description of the learning space.</param>
    /// <param name="learningOutcomeCollectionVm">The learning outcomes of the learning space.</param>
    /// <param name="requiredPoints">The required points for the learning space.</param>
    /// <param name="spaceTheme">The theme of the learning space.</param>
    /// <param name="topic">The topic of the learning space (optional).</param>
    void CreateLearningSpace(string name, string description,
        LearningOutcomeCollectionViewModel learningOutcomeCollectionVm,
        int requiredPoints, SpaceTheme spaceTheme, TopicViewModel? topic = null);

    /// <summary>
    /// Edits an existing learning world.
    /// </summary>
    /// <param name="name">The name of the learning world.</param>
    /// <param name="shortname">The short name of the learning world.</param>
    /// <param name="authors">The authors of the learning world.</param>
    /// <param name="language">The language of the learning world.</param>
    /// <param name="description">The description of the learning world.</param>
    /// <param name="goals">The goals of the learning world.</param>
    /// <param name="evaluationLink">Link to the evaluation displayed on completion.</param>
    /// <param name="enrolmentKey">Key for users to enrol in the learning world.</param>
    /// <param name="storyStart">The story start of the learning world.</param>
    /// <param name="storyEnd">The story end of the learning world.</param>
    void EditLearningWorld(string name, string shortname, string authors, string language, string description,
        string goals, string evaluationLink, string enrolmentKey, string storyStart, string storyEnd);

    /// <summary>
    /// Calls the respective Save methode for Learning Space or Learning Element depending on which learning object is selected
    /// </summary>
    void SaveLearningWorld();

    /// <summary>
    /// Deletes the specified pathway condition from the current learning world.
    /// </summary>
    /// <param name="pathWayCondition">The pathway condition to delete, represented as a PathWayConditionViewModel.</param>
    void DeletePathWayCondition(PathWayConditionViewModel pathWayCondition);

    event EventHandler<CommandUndoRedoOrExecuteArgs> OnCommandUndoRedoOrExecute;

    /// <summary>
    /// Handles the dragging of an object in the pathway, updating its position.
    /// </summary>
    /// <param name="sender">The source of the event that triggered the drag action.</param>
    /// <param name="draggedEventArgs">The arguments containing the learning object being dragged, and its old X and Y positions.</param>
    void DragObjectInPathWay(object sender, DraggedEventArgs<IObjectInPathWayViewModel> draggedEventArgs);

    /// <summary>
    /// Handles a click event on an object in the world, setting the selected learning object.
    /// </summary>
    void ClickOnObjectInWorld(ISelectableObjectInWorldViewModel obj);

    /// <summary>
    /// Handles a double-click event on an object in the pathway, setting the selected learning object and potentially switching the pathway condition.
    /// </summary>
    void DoubleClickOnObjectInPathway(IObjectInPathWayViewModel obj);

    /// <summary>
    /// Switches the logical condition of the specified pathway condition between And and Or.
    /// </summary>
    /// <param name="pathWayCondition">The pathway condition to switch, represented as a PathWayConditionViewModel.</param>
    void SwitchPathWayCondition(PathWayConditionViewModel pathWayCondition);

    /// <summary>
    /// Deletes the specified learning space from the current learning world.
    /// </summary>
    /// <param name="learningSpaceViewModel">The learning space to delete, represented as an ILearningSpaceViewModel.</param>
    void DeleteLearningSpace(ILearningSpaceViewModel learningSpaceViewModel);

    /// <summary>
    /// Deletes a learning object in the pathway based on its type.
    /// </summary>
    /// <param name="objectInPathWayViewModel">The object in the pathway to be deleted.</param>
    void DeleteLearningObject(IObjectInPathWayViewModel objectInPathWayViewModel);

    /// <summary>
    /// Creates a new pathway condition in the current learning world with the specified logical condition.
    /// </summary>
    /// <param name="condition">The logical condition for the pathway condition, defaults to ConditionEnum.Or.</param>
    void CreatePathWayCondition(ConditionEnum condition = ConditionEnum.Or);

    /// <summary>
    /// Edits a given learning element.
    /// </summary>
    /// <param name="elementParent">The parent of the learning element which is either a learning space or null.</param>
    /// <param name="learningElement">The learning element to edit.</param>
    /// <param name="name">The new name of the element.</param>
    /// <param name="description">The new description of the element.</param>
    /// <param name="goals">The new goals of the element.</param>
    /// <param name="difficulty">The new difficulty of the element.</param>
    /// <param name="elementModel">The Theme of the element.</param>
    /// <param name="workload">The new workload of the element.</param>
    /// <param name="points">The new points of the element.</param>
    /// <param name="learningContent">The new learning content of the element.</param>
    void EditLearningElement(ILearningSpaceViewModel? elementParent, ILearningElementViewModel learningElement,
        string name, string description, string goals, LearningElementDifficultyEnum difficulty,
        ElementModel elementModel, int workload, int points, ILearningContentViewModel learningContent);

    IEnumerable<ILearningContentViewModel> GetAllContent();

    /// <summary>
    /// Creates an unplaced learning element in the current learning world with the specified properties.
    /// </summary>
    /// <param name="name">The name of the learning element.</param>
    /// <param name="learningContent">The learning content associated with the learning element, represented as an ILearningContentViewModel.</param>
    /// <param name="description">The description of the learning element.</param>
    /// <param name="goals">The goals of the learning element.</param>
    /// <param name="difficulty">The difficulty level of the learning element, represented as a LearningElementDifficultyEnum.</param>
    /// <param name="elementModel">The element model for the learning element.</param>
    /// <param name="workload">The workload associated with the learning element.</param>
    /// <param name="points">The points associated with the learning element.</param>
    void CreateUnplacedLearningElement(string name, ILearningContentViewModel learningContent, string description,
        string goals, LearningElementDifficultyEnum difficulty, ElementModel elementModel, int workload, int points);

    /// <summary>
    /// Calls the the show learning element content method for the selected learning element.
    /// </summary>
    /// <param name="learningElement"></param>
    Task ShowSelectedElementContentAsync(ILearningElementViewModel learningElement);

    /// <summary>
    /// Deletes the given learning element.
    /// </summary>
    /// <param name="learningElement">Learning element to delete.</param>
    void DeleteLearningElement(ILearningElementViewModel learningElement);

    /// <summary>
    /// Initiates the creation of a new learning space and requests to open the space dialog.
    /// </summary>
    void AddNewLearningSpace();

    /// <summary>
    /// Initiates the editing of the selected learning space associated with the current learning world.
    /// </summary>
    void EditSelectedLearningSpace();

    void CreateUnplacedLearningElementFromFormModel(LearningElementFormModel model);

    void EditLearningElementFromFormModel(ILearningSpaceViewModel? parent, ILearningElementViewModel elementToEdit,
        LearningElementFormModel model);
}