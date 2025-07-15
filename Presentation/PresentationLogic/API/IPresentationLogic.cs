using System.Globalization;
using System.IO.Abstractions;
using BusinessLogic.API;
using BusinessLogic.ErrorManagement.DataAccess;
using Presentation.Components.Adaptivity.Dialogues;
using Presentation.Components.Adaptivity.Forms.Models;
using Presentation.Components.Forms.Models;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.ElectronNET;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Action;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Question;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Trigger;
using Presentation.PresentationLogic.LearningContent.FileContent;
using Presentation.PresentationLogic.LearningContent.LinkContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningSpace.LearningOutcomeViewModel;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.Topic;
using Shared;
using Shared.Adaptivity;
using Shared.Command;
using Shared.Configuration;
using Shared.Exceptions;
using Shared.LearningOutcomes;
using Shared.Theme;

namespace Presentation.PresentationLogic.API;

public interface IPresentationLogic
{
    /// <summary>
    /// AuthoringTool configuration object
    /// </summary>
    IApplicationConfiguration Configuration { get; }

    /// <summary>
    /// BusinessLogic dependency
    /// </summary>
    IBusinessLogic BusinessLogic { get; }

    bool RunningElectron { get; }

    /// <summary>
    /// Whether or not undo can be run.
    /// </summary>
    bool CanUndo { get; }

    /// <summary>
    /// Whether or not undo can be run.
    /// </summary>
    bool CanRedo { get; }

    /// <summary>
    /// Calls the business logic method to undo the last executed command.
    /// </summary>
    void UndoCommand();

    /// <summary>
    /// Calls the business logic method to redo the last undone command.
    /// </summary>
    void RedoCommand();

    /// <summary>
    /// Adds a new learning world in the authoring tool workspace with the corresponding command.
    /// </summary>
    /// <param name="authoringToolWorkspaceVm">Authoring Tool Workspace View Model to add the learning world in.</param>
    /// <param name="learningWorldVm">Learning world to add.</param>
    void AddLearningWorld(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm,
        ILearningWorldViewModel learningWorldVm);

    /// <summary>
    /// Creates a new learning world in the authoring tool workspace with the corresponding command.
    /// </summary>
    /// <param name="authoringToolWorkspaceVm">Authoring Tool Workspace View Model to create the learning world in.</param>
    /// <param name="name"></param>
    /// <param name="shortname"></param>
    /// <param name="authors"></param>
    /// <param name="language"></param>
    /// <param name="description"></param>
    /// <param name="goals"></param>
    /// <param name="worldTheme"></param>
    /// <param name="evaluationLink">Link to the evaluation displayed on completion.</param>
    /// <param name="enrolmentKey">Key for users to enrol in the learning world.</param>
    /// <param name="storyStart">The story start of the learning world.</param>
    /// <param name="storyEnd">The story end of the learning world.</param>
    void CreateLearningWorld(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm, string name, string shortname,
        string authors, string language, string description, string goals, WorldTheme worldTheme, string evaluationLink,
        string enrolmentKey,
        string storyStart, string storyEnd);

    /// <summary>
    /// Edits a given learning world in the authoring tool workspace with the corresponding command.
    /// </summary>
    /// <param name="learningWorldVm">Learning world to edit.</param>
    /// <param name="name"></param>
    /// <param name="shortname"></param>
    /// <param name="authors"></param>
    /// <param name="language"></param>
    /// <param name="description"></param>
    /// <param name="goals"></param>
    /// <param name="worldTheme"></param>
    /// <param name="evaluationLink">Link to the evaluation displayed on completion.</param>
    /// <param name="enrolmentKey">Key for users to enrol in the learning world.</param>
    /// <param name="storyStart">The story start of the learning world.</param>
    /// <param name="storyEnd">The story end of the learning world.</param>
    void EditLearningWorld(ILearningWorldViewModel learningWorldVm, string name, string shortname, string authors,
        string language, string description, string goals, WorldTheme worldTheme, string evaluationLink,
        string enrolmentKey,
        string storyStart, string storyEnd);

    /// <summary>
    /// Deletes the given learning world in the authoring tool workspace.
    /// </summary>
    /// <param name="authoringToolWorkspaceVm"></param>
    /// <param name="worldVm">The learning world to delete.</param>
    void DeleteLearningWorld(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm,
        ILearningWorldViewModel worldVm);

    /// <summary>
    /// Asks user for path and saves <see cref="LearningWorldViewModel"/> to disk.
    /// </summary>
    /// <param name="learningWorldViewModel">The learning world which should be saved.</param>
    /// <returns>Task indicating completion.</returns>
    /// <exception cref="OperationCanceledException">Operation was cancelled by user.</exception>
    /// <exception cref="NotImplementedException">Thrown when we are not running in Electron.</exception>
    /// <exception cref="InvalidOperationException">Thrown when we are running in Electron but no <see cref="IElectronDialogManager"/>
    /// implementation is present in dependency injection container.</exception>
    void SaveLearningWorld(ILearningWorldViewModel learningWorldViewModel);

    /// <summary>
    /// Asks user for path and loads <see cref="LearningWorldViewModel"/> from disk.
    /// </summary>
    /// <returns>Task containing deserialized object.</returns>
    /// <exception cref="OperationCanceledException">Operation was cancelled by user.</exception>
    /// <exception cref="NotImplementedException">Thrown when we are not running in Electron.</exception>
    /// <exception cref="InvalidOperationException">Thrown when we are running in Electron but no <see cref="IElectronDialogManager"/>
    /// implementation is present in dependency injection container.</exception>
    Task LoadLearningWorldAsync(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm);

    /// <summary>
    /// Loads <see cref="LearningWorldViewModel"/> from given path.
    /// </summary>
    /// <param name="authoringToolWorkspaceVm">The Workspace ViewModel.</param>
    /// <param name="path">The Path, the Learning World should loaded from.</param>
    /// <param name="setAsSelected">Whether or not the loaded Learning World should be set as the selected one.</param>
    void LoadLearningWorldFromPath(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm, string path,
        bool setAsSelected = true);

    /// <summary>
    /// Adds a new learning space in the given learning world with the corresponding command.
    /// </summary>
    /// <param name="learningWorldVm">Learning world to add the learning space in.</param>
    /// <param name="learningSpaceVm">Learning space to add.</param>
    void AddLearningSpace(ILearningWorldViewModel learningWorldVm, ILearningSpaceViewModel learningSpaceVm);

    /// <summary>
    /// Creates a new learning space in the given learning world with the corresponding command.
    /// </summary>
    /// <param name="learningWorldVm">The Learning World view model in which to create the Learning Space.</param>
    /// <param name="name">The name of the Learning Space.</param>
    /// <param name="description">The description of the Learning Space.</param>
    /// <param name="learningOutcomeCollectionVm">The learning outcomes of the Learning Space.</param>
    /// <param name="requiredPoints">The points required to access the Learning Space.</param>
    /// <param name="spaceTheme">The theme of the Learning Space.</param>
    /// <param name="positionX">The X-coordinate of the Learning Space's position.</param>
    /// <param name="positionY">The Y-coordinate of the Learning Space's position.</param>
    /// <param name="topicVm">The topic associated with the Learning Space. Can be null.</param>
    void CreateLearningSpace(ILearningWorldViewModel learningWorldVm, string name,
        string description, LearningOutcomeCollectionViewModel learningOutcomeCollectionVm, int requiredPoints,
        SpaceTheme spaceTheme,
        double positionX, double positionY,
        ITopicViewModel? topicVm = null);

    /// <summary>
    /// Edits a given learning space in the given learning world with the corresponding command.
    /// </summary>
    /// <param name="learningSpaceVm">The Learning Space view model to edit.</param>
    /// <param name="name">The new name of the Learning Space.</param>
    /// <param name="description">The new description of the Learning Space.</param>
    /// <param name="requiredPoints">The new points required to access the Learning Space.</param>
    /// <param name="spaceTheme">The new theme of the Learning Space.</param>
    /// <param name="topicVm">The new topic associated with the Learning Space. Can be null.</param>
    void EditLearningSpace(ILearningSpaceViewModel learningSpaceVm, string name,
        string description, int requiredPoints, SpaceTheme spaceTheme, ITopicViewModel? topicVm);

    /// <summary>
    /// Changes the layout of the given learning space to the given layout.
    /// </summary>
    /// <param name="learningSpaceVm">Learning space to edit.</param>
    /// <param name="learningWorldVm"></param>
    /// <param name="floorPlanName">Enum of the FloorPlan to change the layout to.</param>
    void ChangeLearningSpaceLayout(ILearningSpaceViewModel learningSpaceVm, ILearningWorldViewModel learningWorldVm,
        FloorPlanEnum floorPlanName);

    /// <summary>
    /// Deletes the given learning space in the given learning world.
    /// </summary>
    /// <param name="learningWorldVm">Parent learning world of the learning space.</param>
    /// <param name="learningSpaceVm">Learning space to delete.</param>
    void DeleteLearningSpace(ILearningWorldViewModel learningWorldVm, ILearningSpaceViewModel learningSpaceVm);

    /// <summary>
    /// Creates a new pathway condition in the given learning world with the corresponding command.
    /// </summary>
    /// <param name="learningWorldVm">Parent learning world of the condition to create.</param>
    /// <param name="condition">Enum that can either be an AND or an OR condition.</param>
    /// <param name="positionX">X-coordinate of the condition to create. </param>
    /// <param name="positionY">Y-coordinate of the condition to create.</param>
    void CreatePathWayCondition(ILearningWorldViewModel learningWorldVm, ConditionEnum condition, double positionX,
        double positionY);

    /// <summary>
    /// Creates a new pathway condition between two given objects in the given learning world.
    /// </summary>
    /// <param name="learningWorldVm">Parent learning world of the condition to create.</param>
    /// <param name="condition">Enum that can either be an AND or an OR condition.</param>
    /// <param name="sourceObject">Inbound object of the new pathway condition.</param>
    /// <param name="targetObject">Outbound object of the new pathway condition.</param>
    void CreatePathWayConditionBetweenObjects(ILearningWorldViewModel learningWorldVm, ConditionEnum condition,
        IObjectInPathWayViewModel sourceObject, ILearningSpaceViewModel targetObject);

    /// <summary>
    /// Edits the given pathway condition in the given learning world with the corresponding command.
    /// </summary>
    /// <param name="pathWayConditionVm">The path way condition to be edited.</param>
    /// <param name="newCondition">The new condition to be set.</param>
    void EditPathWayCondition(PathWayConditionViewModel pathWayConditionVm, ConditionEnum newCondition);

    /// <summary>
    /// Deletes the given pathway condition in the given learning world.
    /// </summary>
    /// <param name="learningWorldVm">Parent learning world.</param>
    /// <param name="pathWayConditionVm">Pathway condition to be deleted.</param>
    void DeletePathWayCondition(ILearningWorldViewModel learningWorldVm, PathWayConditionViewModel pathWayConditionVm);

    /// <summary>
    /// Creates a topic in the given learning world.
    /// </summary>
    /// <param name="learningWorldVm">Parent learning world of the condition to create.</param>
    /// <param name="name">Name of the Topic</param>
    void CreateTopic(ILearningWorldViewModel learningWorldVm, string name);

    /// <summary>
    /// Edits the topic in the given learning world with the corresponding command.
    /// </summary>
    /// <param name="topicVm">The topic to be edited.</param>
    /// <param name="newName">The new name set for the topic</param>
    void EditTopic(ITopicViewModel topicVm, string newName);

    /// <summary>
    /// Deletes the given topic in the given learning world.
    /// </summary>
    /// <param name="learningWorldVm">Parent learning world.</param>
    /// <param name="topicVm">Topic to be deleted.</param>
    void DeleteTopic(ILearningWorldViewModel learningWorldVm, ITopicViewModel topicVm);

    /// <summary>
    /// Adds a new learning pathway between two objects (learning space or pathway condition) in the given learning world.
    /// </summary>
    /// <param name="learningWorldVm">Learning world into which the learning pathway gets created.</param>
    /// <param name="sourceObjectVm">Learning object from which the path starts.</param>
    /// <param name="targetObjectVm">Learning object where the path ends.</param>
    void CreateLearningPathWay(ILearningWorldViewModel learningWorldVm, IObjectInPathWayViewModel sourceObjectVm,
        IObjectInPathWayViewModel targetObjectVm);

    /// <summary>
    /// Deletes the last pathway that was created to the targetSpace.
    /// </summary>
    /// <param name="learningWorldVm">Learning world in which the learning pathway gets deleted.</param>
    /// <param name="learningPathWayVm">Learning PathWay to delete.</param>
    void DeleteLearningPathWay(ILearningWorldViewModel learningWorldVm, ILearningPathWayViewModel learningPathWayVm);

    /// <summary>
    /// Adds a new learning element to its parent space.
    /// </summary>
    /// <param name="parentSpaceVm">Parent space of the element.</param>
    /// <param name="slotIndex">Index of the slot in which the element should be created.</param>
    /// <param name="learningElementVm">Learning element to add.</param>
    void AddLearningElement(ILearningSpaceViewModel parentSpaceVm, int slotIndex,
        ILearningElementViewModel learningElementVm);

    /// <summary>
    /// Creates a new learning element and assigns it to the opened learning space in the
    /// selected learning world.
    /// </summary>
    /// <param name="parentSpaceVm">Parent space of the element.</param>
    /// <param name="slotIndex">Index of the slot in which the element should be created.</param>
    /// <param name="name">Name of the element.</param>
    /// <param name="learningContentVm">The content of the element.</param>
    /// <param name="description">A description of the element.</param>
    /// <param name="goals">The goals of the element.</param>
    /// <param name="difficulty">The difficulty of the element.</param>
    /// <param name="elementModel">The 2d/3d model description of the learning element</param>
    /// <param name="workload">The time required to complete the learning element.</param>
    /// <param name="points">The number of points of the learning element.</param>
    /// <param name="positionX"></param>
    /// <param name="positionY"></param>
    void CreateLearningElementInSlot(ILearningSpaceViewModel parentSpaceVm, int slotIndex, string name,
        ILearningContentViewModel learningContentVm,
        string description, string goals, LearningElementDifficultyEnum difficulty, ElementModel elementModel,
        int workload, int points,
        double positionX = 0D, double positionY = 0D);

    void CreateStoryElementInSlot(ILearningSpaceViewModel learningSpaceVm, int slotIndex, string name,
        ILearningContentViewModel learningContent, string description, string goals,
        LearningElementDifficultyEnum difficulty, ElementModel elementModel, int workload, int points,
        double positionX = 0, double positionY = 0);

    /// <summary>
    /// Edits a given learning element in the opened learning space with the corresponding command.
    /// </summary>
    /// <param name="parentSpaceVm">Parent space of the element.</param>
    /// <param name="learningElementVm">Element to edit.</param>
    /// <param name="name">Name of the element.</param>
    /// <param name="description">A description of the element.</param>
    /// <param name="goals">The goals of the element.</param>
    /// <param name="difficulty">The difficulty of the element.</param>
    /// <param name="elementModel">The theme of the element.</param>
    /// <param name="workload">The time required to complete the learning element.</param>
    /// <param name="points">The number of points of the learning element.</param>
    /// <param name="learningContentViewModel"></param>
    void EditLearningElement(ILearningSpaceViewModel? parentSpaceVm, ILearningElementViewModel learningElementVm,
        string name, string description, string goals, LearningElementDifficultyEnum difficulty,
        ElementModel elementModel, int workload, int points, ILearningContentViewModel learningContentViewModel);

    /// <summary>
    /// Moves the given learning element from unplaced elements to the given slot index in the given learning space.
    /// </summary>
    /// <param name="learningWorldVm">Learning World with the unplaced elements.</param>
    /// <param name="learningSpaceVm">Learning space to place the learning element in.</param>
    /// <param name="learningElementVm">Learning element to place.</param>
    /// <param name="newSlotIndex">Index of the slot in the learning space to place the learning element in.</param>
    void DragLearningElementFromUnplaced(ILearningWorldViewModel learningWorldVm,
        ILearningSpaceViewModel learningSpaceVm, ILearningElementViewModel learningElementVm, int newSlotIndex);

    /// <summary>
    /// Moves the given story element from unplaced elements to the given slot index in the given learning space.
    /// </summary>
    /// <param name="learningWorldVm">Learning World with the unplaced elements.</param>
    /// <param name="learningSpaceVm">Learning space to place the learning element in.</param>
    /// <param name="learningElementVm">Learning element to place.</param>
    /// <param name="newSlotIndex">Index of the slot in the learning space to place the learning element in.</param>
    void DragStoryElementFromUnplaced(ILearningWorldViewModel learningWorldVm,
        ILearningSpaceViewModel learningSpaceVm, ILearningElementViewModel learningElementVm, int newSlotIndex);

    /// <summary>
    /// Moves the given learning element from the learning space to unplaced elements in the learning world.
    /// </summary>
    /// <param name="learningWorldVm">Learning World with the unplaced elements.</param>
    /// <param name="learningSpaceVm">Learning space from which the element should be removed.</param>
    /// <param name="learningElementVm">Learning element to remove.</param>
    void DragLearningElementToUnplaced(ILearningWorldViewModel learningWorldVm, ILearningSpaceViewModel learningSpaceVm,
        ILearningElementViewModel learningElementVm);

    /// <summary>
    /// Moves the given story element from the learning space to unplaced elements in the learning world.
    /// </summary>
    /// <param name="learningWorldVm">Learning World with the unplaced elements.</param>
    /// <param name="learningSpaceVm">Learning space from which the element should be removed.</param>
    /// <param name="learningElementVm">Learning element to remove.</param>
    void DragStoryElementToUnplaced(ILearningWorldViewModel learningWorldVm, ILearningSpaceViewModel learningSpaceVm,
        ILearningElementViewModel learningElementVm);

    /// <summary>
    /// Switches the slot of a Learning Element within a Learning Space.
    /// </summary>
    /// <param name="learningSpaceVm">The Learning Space view model in which the Learning Element resides.</param>
    /// <param name="learningElementVm">The Learning Element view model to switch the slot of.</param>
    /// <param name="newSlotIndex">The new slot index to place the Learning Element at.</param>
    /// <remarks>
    /// If the active slot in the Learning Space is the new slot index, it will be set to -1.
    /// </remarks>
    void SwitchLearningElementSlot(ILearningSpaceViewModel learningSpaceVm, ILearningElementViewModel learningElementVm,
        int newSlotIndex);

    /// <summary>
    /// Switches the slot of a Story Element within a Learning Space.
    /// </summary>
    /// <param name="learningSpaceVm">The Learning Space view model in which the Learning Element resides.</param>
    /// <param name="learningElementVm">The Learning Element view model to switch the slot of.</param>
    /// <param name="newSlotIndex">The new slot index to place the Learning Element at.</param>
    /// <remarks>
    /// If the active slot in the Learning Space is the new slot index, it will be set to -1.
    /// </remarks>
    void SwitchStoryElementSlot(ILearningSpaceViewModel learningSpaceVm, ILearningElementViewModel learningElementVm,
        int newSlotIndex);

    /// <summary>
    /// Deletes the given learning element in the given learning space.
    /// </summary>
    /// <param name="parentSpaceVm">Parent space of the element.</param>
    /// <param name="learningElementVm">Element to delete.</param>
    void DeleteLearningElementInSpace(ILearningSpaceViewModel parentSpaceVm,
        ILearningElementViewModel learningElementVm);

    void DeleteStoryElementInSpace(ILearningSpaceViewModel parentSpaceVm, ILearningElementViewModel learningElementVm);

    /// <summary>
    /// Deletes the given learning element in the given learning world.
    /// </summary>
    /// <param name="learningWorldVm">Parent world of the element.</param>
    /// <param name="learningElementVm">Element to delete.</param>
    void DeleteLearningElementInWorld(ILearningWorldViewModel learningWorldVm,
        ILearningElementViewModel learningElementVm);

    /// <summary>
    /// Open the given content file of the learning element in the desktop's default manner.
    /// </summary>
    /// <param name="learningElementVm">Element which contains the content file to be opened.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the LearningContent of the Learning Element is not of type FileContentViewModel or LinkContentViewModel.</exception>
    Task ShowLearningElementContentAsync(LearningElementViewModel learningElementVm);

    /// <summary>
    /// Loads a Learning Content view model from a given stream.
    /// </summary>
    /// <param name="name">The name of the Learning Content.</param>
    /// <param name="stream">The stream containing the data for the Learning Content.</param>
    /// <returns>The loaded Learning Content view model.</returns>
    /// <exception cref="HashExistsException">There is already a file with the same hash inside the content folder.</exception>
    Task<ILearningContentViewModel> LoadLearningContentViewModelAsync(string name, Stream stream);

    void EditH5PFileContent(IFileContentViewModel fileContentVm);
    
    /// <summary>
    /// Creates a Adaptivity Task in the given Adaptivity Content.
    /// </summary>
    /// <param name="adaptivityContentFm">The Adaptivity Content Form Model to create the Task in.</param>
    /// <param name="name">The name of the Task.</param>
    void CreateAdaptivityTask(AdaptivityContentFormModel adaptivityContentFm, string name);

    /// <summary>
    /// Edits a given Adaptivity Task
    /// </summary>
    /// <param name="adaptivityTaskVm">The Task to edit.</param>
    /// <param name="name">The new name for the Task.</param>
    /// <param name="minimumRequiredDifficulty">The minimum required Difficulty for the Task.</param>
    void EditAdaptivityTask(IAdaptivityTaskViewModel adaptivityTaskVm, string name,
        QuestionDifficulty? minimumRequiredDifficulty);

    /// <summary>
    /// Deletes a given Adaptivity Task from the given Adaptivity Content.
    /// </summary>
    /// <param name="adaptivityContentFm">The Adaptivity Content Form Model to delete the Task from.</param>
    /// <param name="adaptivityTaskVm">The Adaptivity Task to delete.</param>
    void DeleteAdaptivityTask(AdaptivityContentFormModel adaptivityContentFm,
        IAdaptivityTaskViewModel adaptivityTaskVm);

    /// <summary>
    /// Deletes a given Adaptivity Task from the given Adaptivity Content.
    /// </summary>
    /// <param name="adaptivityContentVm">The Adaptivity Content View Model to delete the Task from.</param>
    /// <param name="adaptivityTaskVm">The Adaptivity Task to delete.</param>
    void DeleteAdaptivityTask(AdaptivityContentViewModel adaptivityContentVm,
        IAdaptivityTaskViewModel adaptivityTaskVm);


    /// <summary>
    /// Gets all content files in the appdata folder.
    /// </summary>
    /// <returns>An enumerable of content files.</returns>
    IEnumerable<ILearningContentViewModel> GetAllContent();

    /// <summary>
    /// Deletes the file referenced by the given content object.
    /// </summary>
    /// <param name="content">The content whos file shall be deleted.</param>
    /// <exception cref="FileNotFoundException">The file corresponding to <paramref name="content"/> wasn't found.</exception>
    public void RemoveContent(ILearningContentViewModel content);

    /// <summary>
    /// Deletes the files referenced by the given content objects.
    /// </summary>
    /// <param name="contents">The contents whos file shall be deleted.</param>
    /// <exception cref="FileNotFoundException">Files corresponding to <paramref name="contents"/> weren't found.</exception>
    public void RemoveMultipleContents(IEnumerable<ILearningContentViewModel> contents);

    event EventHandler<CommandUndoRedoOrExecuteArgs> OnCommandUndoRedoOrExecute;

    /// <summary>
    /// Drags an object in the pathway from its old position to its current position.
    /// </summary>
    /// <param name="objectInPathWayVm">The view model of the object in the pathway to be moved. The object's new position is determined by its current 'PositionX' and 'PositionY' properties.</param>
    /// <param name="oldPositionX">The old X-coordinate of the object's position.</param>
    /// <param name="oldPositionY">The old Y-coordinate of the object's position.</param>
    void DragObjectInPathWay(IObjectInPathWayViewModel objectInPathWayVm, double oldPositionX, double oldPositionY);

    /// <summary>
    /// Asynchronously shows the content of a Learning Content view model.
    /// </summary>
    /// <param name="content">The Learning Content view model to display. It must be either a FileContentViewModel or a LinkContentViewModel.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the Learning Content is not of type FileContentViewModel or LinkContentViewModel.</exception>
    Task ShowLearningContentAsync(ILearningContentViewModel content);

    /// <summary>
    /// Asynchronously shows the content of a Learning Content view model.
    /// </summary>
    /// <param name="content">The Learning Content view model to display. It must be either a FileContentFormModel or a LinkContentViewModel.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the Learning Content is not of type FileContentFormModel or LinkContentViewModel.</exception>
    Task ShowLearningContentAsync(ILearningContentFormModel content);

    /// <summary>
    /// Saves a link associated with a LinkContentViewModel.
    /// </summary>
    /// <param name="linkContentVm">The LinkContentViewModel containing the link to save.</param>
    void SaveLink(LinkContentViewModel linkContentVm);

    /// <summary>
    /// Opens the folder containing all content files in the desktop's default manner.
    /// </summary>
    void OpenContentFilesFolder();

    /// <summary>
    /// Creates an unplaced Learning Element with the specified properties in a Learning World.
    /// </summary>
    /// <param name="learningWorldVm">The Learning World view model in which to create the Learning Element.</param>
    /// <param name="name">The name of the Learning Element.</param>
    /// <param name="learningContentVm">The learning content view model associated with the Learning Element.</param>
    /// <param name="description">The description of the Learning Element.</param>
    /// <param name="goals">The goals of the Learning Element.</param>
    /// <param name="difficulty">The difficulty level of the Learning Element.</param>
    /// <param name="elementModel">The element model of the Learning Element.</param>
    /// <param name="workload">The workload of the Learning Element.</param>
    /// <param name="points">The points awarded for completing the Learning Element.</param>
    /// <param name="positionX">The X-coordinate of the Learning Element's position. Default is 0.</param>
    /// <param name="positionY">The Y-coordinate of the Learning Element's position. Default is 0.</param>
    void CreateUnplacedLearningElement(ILearningWorldViewModel learningWorldVm, string name,
        ILearningContentViewModel learningContentVm, string description, string goals,
        LearningElementDifficultyEnum difficulty, ElementModel elementModel, int workload, int points,
        double positionX = 0D,
        double positionY = 0D);

    /// <summary>
    /// Asynchronously retrieves the save path for a Learning World.
    /// </summary>
    /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous operation. 
    /// The task result contains the file path for the Learning World.</returns>
    Task<string> GetWorldSavePath();

    IEnumerable<IFileInfo> GetSavedLearningWorldPaths();

    void SetSelectedLearningContentViewModel(ILearningContentViewModel content);

    void CreateMultipleChoiceSingleResponseQuestion(IAdaptivityTaskViewModel task, QuestionDifficulty difficulty,
        string questionText, ICollection<ChoiceFormModel> choices, ChoiceFormModel correctChoice,
        int expectedCompletionTime);

    void CreateMultipleChoiceMultipleResponseQuestion(IAdaptivityTaskViewModel task, QuestionDifficulty difficulty,
        string questionText, ICollection<ChoiceFormModel> choices,
        ICollection<ChoiceFormModel> correctChoices, int expectedCompletionTime);


    void EditMultipleChoiceSingleResponseQuestion(MultipleChoiceSingleResponseQuestionViewModel question,
        string questionText, ICollection<ChoiceFormModel> choices, ChoiceFormModel correctChoice,
        int expectedCompletionTime);

    void EditMultipleChoiceMultipleResponseQuestion(MultipleChoiceMultipleResponseQuestionViewModel question,
        string questionText, ICollection<ChoiceFormModel> choices,
        ICollection<ChoiceFormModel> correctChoices, int expectedCompletionTime);

    void EditMultipleChoiceQuestionWithTypeChange(IAdaptivityTaskViewModel task,
        IMultipleChoiceQuestionViewModel question, bool isSingleResponse, string text,
        ICollection<ChoiceFormModel> choices, ICollection<ChoiceFormModel> correctChoices, int expectedCompletionTime);

    void DeleteAdaptivityQuestion(IAdaptivityTaskViewModel task, IAdaptivityQuestionViewModel question);

#if DEBUG
    void ConstructDebugBackup(ILearningWorldViewModel world);
#endif

    void CreateAdaptivityRule(IAdaptivityQuestionViewModel question, IAdaptivityTriggerViewModel trigger,
        IAdaptivityActionViewModel action);

    void DeleteAdaptivityRule(IAdaptivityQuestionViewModel question, IAdaptivityRuleViewModel rule);
    void EditCommentAction(CommentActionViewModel action, string comment);

    void EditContentReferenceAction(ContentReferenceActionViewModel action, ILearningContentViewModel content,
        string comment);

    void EditElementReferenceAction(ElementReferenceActionViewModel action, Guid elementGuid, string comment);

    /// <summary>
    /// Replaces a ContentReferenceAction with an ElementReferenceAction in the specified question.
    /// </summary>
    /// <param name="question">The adaptivity question to be updated.</param>
    /// <param name="ruleVm">The ViewModel of the adaptivity rule to be removed.</param>
    /// <param name="elementReferenceActionVm">The ViewModel of the new ElementReferenceAction.</param>
    /// <param name="triggerVm">The ViewModel of the CorrectnessTrigger for the new rule.</param>
    void ReplaceContentReferenceActionByElementReferenceAction(IAdaptivityQuestionViewModel question,
        IAdaptivityRuleViewModel ruleVm, ElementReferenceActionViewModel elementReferenceActionVm,
        IAdaptivityTriggerViewModel triggerVm);

    /// <summary>
    /// Replaces an ElementReferenceAction with a ContentReferenceAction in the specified question.
    /// </summary>
    /// <param name="question">The adaptivity question to be updated.</param>
    /// <param name="ruleVm">The ViewModel of the adaptivity rule to be removed.</param>
    /// <param name="contentReferenceActionVm">The ViewModel of the new ContentReferenceAction.</param>
    /// <param name="triggerVm">The ViewModel of the CorrectnessTrigger for the new rule.</param>
    void ReplaceElementReferenceActionByContentReferenceAction(IAdaptivityQuestionViewModel question,
        IAdaptivityRuleViewModel ruleVm, ContentReferenceActionViewModel contentReferenceActionVm,
        IAdaptivityTriggerViewModel triggerVm);

    /// <summary>
    /// Asynchronously retrieves a list of LMS World view models.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, which upon completion, returns a list of LmsWorldViewModel objects.</returns>
    /// <exception cref="BackendException">Thrown if there is an issue with the HTTP request.</exception>
    Task<List<LmsWorldViewModel>> GetLmsWorldList();

    /// <summary>
    /// Asynchronously sends a request to delete a specific LMS World entity represented by a view model.
    /// </summary>
    /// <param name="worldVm">The LmsWorldViewModel object representing the world to be deleted.</param>
    /// <exception cref="BackendException">Thrown when the LMS world could not be deleted or if there is an issue with the HTTP request.</exception>
    Task DeleteLmsWorld(LmsWorldViewModel worldVm);

    Task ExportLearningWorldToZipArchiveAsync(ILearningWorldViewModel world);
    Task ExportLearningWorldToMoodleArchiveAsync(ILearningWorldViewModel world);
    Task<LearningWorldViewModel?> ImportLearningWorldFromArchiveAsync();
    IFileInfo? GetFileInfoForLearningWorld(ILearningWorldViewModel world);
    void DeleteLearningWorldByPath(string savePath);

    /// <summary>
    /// Adds a structured learning outcome to the specified learning outcomes collection.
    /// </summary>
    /// <param name="learningOutcomes">The learning outcomes collection view model to which the new learning outcome will be added.</param>
    /// <param name="taxonomyLevel">The taxonomy level of the learning outcome.</param>
    /// <param name="what">A description of the learning outcome.</param>
    /// <param name="verbOfVisibility">The verb indicating the visibility or action associated with the learning outcome.</param>
    /// <param name="whereby">The method or process by which the learning outcome can be achieved.</param>
    /// <param name="whatFor">The purpose or reason for the learning outcome.</param>
    /// <param name="language">The culture info specifying the language in which the learning outcome is described.</param>
    void AddStructuredLearningOutcome(LearningOutcomeCollectionViewModel learningOutcomes,
        TaxonomyLevel taxonomyLevel, string what, string verbOfVisibility, string whereby,
        string whatFor, CultureInfo language);

    /// <summary>
    /// Adds a manual learning outcome to the specified learning outcomes collection.
    /// </summary>
    /// <param name="learningOutcomes">The learning outcomes collection view model to which the new learning outcome will be added.</param>
    /// <param name="manualLearningOutcomeText">The text of the manual learning outcome.</param>
    void AddManualLearningOutcome(LearningOutcomeCollectionViewModel learningOutcomes,
        string manualLearningOutcomeText);


    /// <summary>
    /// Edits an existing structured learning outcome within a specified learning outcomes collection.
    /// </summary>
    /// <param name="learningOutcomes">The learning outcomes collection view model containing the learning outcome to be edited.</param>
    /// <param name="learningOutcome">The learning outcome view model to be edited.</param>
    /// <param name="taxonomyLevel">The new taxonomy level for the edited learning outcome.</param>
    /// <param name="what">The new description of the learning outcome.</param>
    /// <param name="verbOfVisibility">The new verb indicating the visibility or action associated with the learning outcome.</param>
    /// <param name="whereby">The new method or process by which the learning outcome can be achieved.</param>
    /// <param name="whatFor">The new purpose or reason for the learning outcome.</param>
    /// <param name="language">The culture info specifying the language in which the learning outcome is described.</param>
    /// <remarks>
    /// This method performs an edit operation by first deleting the existing learning outcome and then adding
    /// a new learning outcome with the updated details. It achieves this through a batch command that
    /// executes both delete and add operations atomically to ensure data consistency.
    /// </remarks>
    void EditStructuredLearningOutcome(LearningOutcomeCollectionViewModel learningOutcomes,
        StructuredLearningOutcomeViewModel learningOutcome, TaxonomyLevel taxonomyLevel,
        string what, string verbOfVisibility, string whereby, string whatFor, CultureInfo language);

    /// <summary>
    /// Edits an existing manual learning outcome within a specified learning outcomes collection.
    /// </summary>
    /// <param name="learningOutcomes">The learning outcomes collection view model containing the manual learning outcome to be edited.</param>
    /// <param name="learningOutcome">The manual learning outcome view model to be edited.</param>
    /// <param name="manualLearningOutcomeText">The new text for the edited manual learning outcome.</param>
    /// <remarks>
    /// This method updates a manual learning outcome by first deleting the existing outcome and then adding a new outcome
    /// with the updated text. The operations are encapsulated in a batch command to ensure that the changes are
    /// applied atomically, maintaining the integrity of the learning outcomes collection. The position of the updated
    /// learning outcome within the collection is preserved.
    /// </remarks>
    void EditManualLearningOutcome(LearningOutcomeCollectionViewModel learningOutcomes,
        ManualLearningOutcomeViewModel learningOutcome, string manualLearningOutcomeText);

    /// <summary>
    /// Deletes a specified learning outcome from a learning outcomes collection.
    /// </summary>
    /// <param name="learningOutcomes">The learning outcomes collection view model from which the learning outcome will be deleted.</param>
    /// <param name="learningOutcome">The learning outcome view model to be deleted.</param>
    void DeleteLearningOutcome(LearningOutcomeCollectionViewModel learningOutcomes,
        ILearningOutcomeViewModel learningOutcome);

    #region BackendAccess

    Task<bool> IsLmsConnected();
    string LoginName { get; }
    Task Login(string username, string password);
    void Logout();

    Task<UploadResponseViewModel> ConstructAndUploadBackupAsync(ILearningWorldViewModel world, IProgress<int> progress,
        CancellationToken cancellationToken);

    #endregion

    /// <summary>
    /// Validates a <see cref="LearningWorldViewModel"/> instance to determine whether it is suitable for export.
    /// </summary>
    /// <param name="worldVm">The view model representing the learning world to validate.</param>
    /// <returns>
    /// A <see cref="ValidationResult"/> containing any validation errors found.
    /// </returns>
    ValidationResult ValidateLearningWorldForExport(ILearningWorldViewModel worldVm);


    /// <summary>
    /// Validates a <see cref="LearningWorldViewModel"/> instance to determine whether it meets all requirements for generation.
    /// Validation includes structural integrity, learning space definitions, content references, and adaptivity rules.
    /// </summary>
    /// <param name="worldVm">The view model representing the learning world to validate.</param>
    /// <returns>
    /// A <see cref="ValidationResult"/> containing any validation errors found.
    /// </returns>
    ValidationResult ValidateLearningWorldForGeneration(ILearningWorldViewModel worldVm);
}