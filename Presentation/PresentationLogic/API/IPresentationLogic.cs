using BusinessLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.ElectronNET;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.Topic;
using Shared;
using Shared.Command;
using Shared.Configuration;

namespace Presentation.PresentationLogic.API;

public interface IPresentationLogic
{
    /// <summary>
    /// AuthoringTool configuration object
    /// </summary>
    IAuthoringToolConfiguration Configuration { get; }

    /// <summary>
    /// BusinessLogic dependency
    /// </summary>
    IBusinessLogic BusinessLogic { get; }

    bool RunningElectron { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="learningWorldViewModel"></param>
    /// <exception cref="OperationCanceledException">Operation was cancelled by user.</exception>
    /// <returns>Filepath to the new backup file</returns>
    Task<string> ConstructBackupAsync(ILearningWorldViewModel learningWorldViewModel);

    bool CanUndo { get; }
    bool CanRedo { get; }
    void UndoCommand();
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
    /// <param name="shortname"></param>
    /// <param name="authors"></param>
    /// <param name="language"></param>
    /// <param name="description"></param>
    /// <param name="goals"></param>
    /// <param name="name"></param>
    void CreateLearningWorld(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm, string name, string shortname,
        string authors, string language, string description, string goals);

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
    void EditLearningWorld(ILearningWorldViewModel learningWorldVm, string name, string shortname, string authors,
        string language, string description, string goals);

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
    Task SaveLearningWorldAsync(ILearningWorldViewModel learningWorldViewModel);

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
    /// <returns></returns>
    void LoadLearningWorldFromPath(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm, string path);

    /// <summary>
    /// Adds a new learning space in the given learning world with the corresponding command.
    /// </summary>
    /// <param name="learningWorldVm">Learning world to add the learning space in.</param>
    /// <param name="learningSpaceVm">Learning space to add.</param>
    void AddLearningSpace(ILearningWorldViewModel learningWorldVm, ILearningSpaceViewModel learningSpaceVm);

    /// <summary>
    /// Creates a new learning space in the given learning world with the corresponding command.
    /// </summary>
    /// <param name="learningWorldVm">Parent learning world of the learning space to create.</param>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="goals"></param>
    /// <param name="requiredPoints"></param>
    /// <param name="theme"></param>
    /// <param name="positionX"></param>
    /// <param name="positionY"></param>
    /// <param name="topicVm"></param>
    void CreateLearningSpace(ILearningWorldViewModel learningWorldVm, string name,
        string description, string goals, int requiredPoints, Theme theme, bool advancedMode, double positionX, double positionY,
        ITopicViewModel? topicVm = null);

    /// <summary>
    /// Edits a given learning space in the given learning world with the corresponding command.
    /// </summary>
    /// <param name="learningSpaceVm">Learning space to edit.</param>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="goals"></param>
    /// <param name="requiredPoints"></param>
    /// <param name="theme"></param>
    /// <param name="topicVm"></param>
    void EditLearningSpace(ILearningSpaceViewModel learningSpaceVm, string name,
        string description, string goals, int requiredPoints, Theme theme, ITopicViewModel? topicVm);

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
    /// Asks user for path and saves <see cref="LearningSpaceViewModel"/> to disk.
    /// </summary>
    /// <param name="learningSpaceViewModel">The learning space which should be saved.</param>
    /// <returns>Task indicating completion.</returns>
    /// <exception cref="OperationCanceledException">Operation was cancelled by user.</exception>
    /// <exception cref="NotImplementedException">Thrown when we are not running in Electron.</exception>
    /// <exception cref="InvalidOperationException">Thrown when we are running in Electron but no <see cref="IElectronDialogManager"/>
    /// implementation is present in dependency injection container.</exception>
    Task SaveLearningSpaceAsync(LearningSpaceViewModel learningSpaceViewModel);

    /// <summary>
    /// Asks user for path and loads <see cref="LearningSpaceViewModel"/> from disk.
    /// </summary>
    /// <param name="learningWorldVm">Learning world into which the learning space should be loaded.</param>
    /// <returns>Task containing deserialized object.</returns>
    /// <exception cref="OperationCanceledException">Operation was cancelled by user.</exception>
    /// <exception cref="NotImplementedException">Thrown when we are not running in Electron.</exception>
    /// <exception cref="InvalidOperationException">Thrown when we are running in Electron but no <see cref="IElectronDialogManager"/>
    /// implementation is present in dependency injection container.</exception>
    Task LoadLearningSpaceAsync(ILearningWorldViewModel learningWorldVm);

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
    /// <param name="workload">The time required to complete the learning element.</param>
    /// <param name="points">The number of points of the learning element.</param>
    /// <param name="positionX"></param>
    /// <param name="positionY"></param>
    void CreateLearningElementInSlot(ILearningSpaceViewModel parentSpaceVm, int slotIndex, string name,
        ILearningContentViewModel learningContentVm,
        string description, string goals, LearningElementDifficultyEnum difficulty, ElementModel elementModel,
        int workload, int points,
        double positionX = 0D, double positionY = 0D);

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
    /// Moves the given learning element from the learning space to unplaced elements in the learning world.
    /// </summary>
    /// <param name="learningWorldVm">Learning World with the unplaced elements.</param>
    /// <param name="learningSpaceVm">Learning space from which the element should be removed.</param>
    /// <param name="learningElementVm">Learning element to remove.</param>
    void DragLearningElementToUnplaced(ILearningWorldViewModel learningWorldVm, ILearningSpaceViewModel learningSpaceVm,
        ILearningElementViewModel learningElementVm);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="learningSpaceVm"></param>
    /// <param name="learningElementVm"></param>
    /// <param name="newSlotIndex"></param>
    void SwitchLearningElementSlot(ILearningSpaceViewModel learningSpaceVm, ILearningElementViewModel learningElementVm,
        int newSlotIndex);

    /// <summary>
    /// Deletes the given learning element in the given learning space.
    /// </summary>
    /// <param name="parentSpaceVm">Parent space of the element.</param>
    /// <param name="learningElementVm">Element to delete.</param>
    void DeleteLearningElementInSpace(ILearningSpaceViewModel parentSpaceVm,
        ILearningElementViewModel learningElementVm);

    /// <summary>
    /// Deletes the given learning element in the given learning world.
    /// </summary>
    /// <param name="learningWorldVm">Parent world of the element.</param>
    /// <param name="learningElementVm">Element to delete.</param>
    void DeleteLearningElementInWorld(ILearningWorldViewModel learningWorldVm,
        ILearningElementViewModel learningElementVm);

    /// <summary>
    /// Asks user for path and saves <see cref="LearningElementViewModel"/> to disk.
    /// </summary>
    /// <param name="learningElementViewModel">The learning element which should be saved.</param>
    /// <returns>Task indicating completion.</returns>
    /// <exception cref="OperationCanceledException">Operation was cancelled by user.</exception>
    /// <exception cref="NotImplementedException">Thrown when we are not running in Electron.</exception>
    /// <exception cref="InvalidOperationException">Thrown when we are running in Electron but no <see cref="IElectronDialogManager"/>
    /// implementation is present in dependency injection container.</exception>
    Task SaveLearningElementAsync(LearningElementViewModel learningElementViewModel);

    /// <summary>
    /// Asks user for path and loads <see cref="LearningElementViewModel"/> from disk.
    /// </summary>
    /// <param name="parentSpaceVm">Learning space into which the learning element should be loaded.</param>
    /// <param name="slotIndex">Index of the slot in which the element should be loaded..</param>
    /// <returns>Task containing deserialized object.</returns>
    /// <exception cref="OperationCanceledException">Operation was cancelled by user.</exception>
    /// <exception cref="NotImplementedException">Thrown when we are not running in Electron.</exception>
    /// <exception cref="InvalidOperationException">Thrown when we are running in Electron but no <see cref="IElectronDialogManager"/>
    /// implementation is present in dependency injection container.</exception>
    Task LoadLearningElementAsync(ILearningSpaceViewModel parentSpaceVm, int slotIndex);

    /// <summary>
    /// Open the given content file of the learning element in the desktop's default manner.
    /// </summary>
    /// <param name="learningElementVm">Element which contains the content file to be opened.</param>
    /// <returns></returns>
    Task ShowLearningElementContentAsync(LearningElementViewModel learningElementVm);

    ILearningContentViewModel LoadLearningContentViewModel(string name, Stream stream);

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

    void LoadLearningWorldViewModel(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm, Stream stream);
    void LoadLearningSpaceViewModel(ILearningWorldViewModel learningWorldVm, Stream stream);
    void LoadLearningElementViewModel(ILearningSpaceViewModel parentSpaceVm, int slotIndex, Stream stream);
    event EventHandler<CommandUndoRedoOrExecuteArgs> OnCommandUndoRedoOrExecute;
    void DragObjectInPathWay(IObjectInPathWayViewModel pathWayObjectVm, double oldPositionX, double oldPositionY);
    void DragLearningElement(ILearningElementViewModel learningElementVm, double oldPositionX, double oldPositionY);
    Task ShowLearningContentAsync(ILearningContentViewModel content);
    void SaveLink(LinkContentViewModel linkContentVm);

    /// <summary>
    /// Opens the folder containing all content files in the desktop's default manner.
    /// </summary>
    void OpenContentFilesFolder();

    void CreateUnplacedLearningElement(ILearningWorldViewModel learningWorldVm, string name,
        ILearningContentViewModel learningContentVm, string description, string goals,
        LearningElementDifficultyEnum difficulty, ElementModel elementModel, int workload, int points,
        double positionX = 0D,
        double positionY = 0D);

    Task<string> GetWorldSavePath();
    IEnumerable<SavedLearningWorldPath> GetSavedLearningWorldPaths();
    void AddSavedLearningWorldPath(SavedLearningWorldPath savedLearningWorldPath);
    SavedLearningWorldPath AddSavedLearningWorldPathByPathOnly(string path);
    void UpdateIdOfSavedLearningWorldPath(SavedLearningWorldPath savedLearningWorldPath, Guid id);
    void RemoveSavedLearningWorldPath(SavedLearningWorldPath savedLearningWorldPath);

    /// <summary>
    /// Debug method for Philipp.
    /// </summary>
    void CallExport();

    #region BackendAccess

    Task<bool> IsLmsConnected();
    string LoginName { get; }
    Task<bool> Login(string username, string password);
    void Logout();
    void UploadLearningWorldToBackend(string filepath);

    #endregion

    void SetSelectedLearningContentViewModel(ILearningContentViewModel content);
}