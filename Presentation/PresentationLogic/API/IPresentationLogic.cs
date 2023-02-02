using BusinessLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.ElectronNET;
using Presentation.PresentationLogic.Content;
using Presentation.PresentationLogic.Element;
using Presentation.PresentationLogic.PathWay;
using Presentation.PresentationLogic.Space;
using Presentation.PresentationLogic.World;
using Shared;
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
    /// <param name="worldViewModel"></param>
    /// <exception cref="OperationCanceledException">Operation was cancelled by user.</exception>
    /// <returns>Filepath to the new backup file</returns>
    Task<string> ConstructBackupAsync(WorldViewModel worldViewModel);
    
    bool CanUndo { get; }
    bool CanRedo { get; }
    void UndoCommand();
    void RedoCommand();

    /// <summary>
    /// Adds a new  world in the authoring tool workspace with the corresponding command.
    /// </summary>
    /// <param name="authoringToolWorkspaceVm">Authoring Tool Workspace View Model to add the  world in.</param>
    /// <param name="worldVm"> world to add.</param>
    void AddWorld(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm,
        IWorldViewModel worldVm);
    
    /// <summary>
    /// Creates a new  world in the authoring tool workspace with the corresponding command.
    /// </summary>
    /// <param name="authoringToolWorkspaceVm">Authoring Tool Workspace View Model to create the  world in.</param>
    /// <param name="shortname"></param>
    /// <param name="authors"></param>
    /// <param name="language"></param>
    /// <param name="description"></param>
    /// <param name="goals"></param>
    /// <param name="name"></param>
    void CreateWorld(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm, string name, string shortname,
        string authors, string language, string description, string goals);
    
    /// <summary>
    /// Edits a given  world in the authoring tool workspace with the corresponding command.
    /// </summary>
    /// <param name="worldVm"> world to edit.</param>
    /// <param name="name"></param>
    /// <param name="shortname"></param>
    /// <param name="authors"></param>
    /// <param name="language"></param>
    /// <param name="description"></param>
    /// <param name="goals"></param>
    void EditWorld(IWorldViewModel worldVm, string name, string shortname, string authors, 
        string language, string description, string goals);

    /// <summary>
    /// Deletes the given  world in the authoring tool workspace.
    /// </summary>
    /// <param name="authoringToolWorkspaceVm"></param>
    /// <param name="worldVm">The  world to delete.</param>
    void DeleteWorld(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm, WorldViewModel worldVm);
    
    /// <summary>
    /// Asks user for path and saves <see cref="WorldViewModel"/> to disk.
    /// </summary>
    /// <param name="worldViewModel">The  world which should be saved.</param>
    /// <returns>Task indicating completion.</returns>
    /// <exception cref="OperationCanceledException">Operation was cancelled by user.</exception>
    /// <exception cref="NotImplementedException">Thrown when we are not running in Electron.</exception>
    /// <exception cref="InvalidOperationException">Thrown when we are running in Electron but no <see cref="IElectronDialogManager"/>
    /// implementation is present in dependency injection container.</exception>
    Task SaveWorldAsync(WorldViewModel worldViewModel);
    
    /// <summary>
    /// Asks user for path and loads <see cref="WorldViewModel"/> from disk.
    /// </summary>
    /// <returns>Task containing deserialized object.</returns>
    /// <exception cref="OperationCanceledException">Operation was cancelled by user.</exception>
    /// <exception cref="NotImplementedException">Thrown when we are not running in Electron.</exception>
    /// <exception cref="InvalidOperationException">Thrown when we are running in Electron but no <see cref="IElectronDialogManager"/>
    /// implementation is present in dependency injection container.</exception>
    Task LoadWorldAsync(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm);

    /// <summary>
    /// Adds a new  space in the given  world with the corresponding command.
    /// </summary>
    /// <param name="worldVm">World to add the  space in.</param>
    /// <param name="spaceVm">Space to add.</param>
    void AddSpace(IWorldViewModel worldVm, ISpaceViewModel spaceVm);

    /// <summary>
    /// Creates a new  space in the given  world with the corresponding command.
    /// </summary>
    /// <param name="worldVm">Parent  world of the  space to create.</param>
    /// <param name="name"></param>
    /// <param name="shortname"></param>
    /// <param name="authors"></param>
    /// <param name="description"></param>
    /// <param name="goals"></param>
    /// <param name="requiredPoints"></param>
    /// <param name="positionX"></param>
    /// <param name="positionY"></param>
    void CreateSpace(IWorldViewModel worldVm, string name, string shortname,
        string authors, string description, string goals, int requiredPoints, double positionX, double positionY);

    /// <summary>
    /// Edits a given space in the given  world with the corresponding command.
    /// </summary>
    /// <param name="spaceVm">Space to edit.</param>
    /// <param name="name"></param>
    /// <param name="shortname"></param>
    /// <param name="authors"></param>
    /// <param name="description"></param>
    /// <param name="goals"></param>
    /// <param name="requiredPoints"></param>
    void EditSpace(ISpaceViewModel spaceVm, string name,
        string shortname, string authors, string description, string goals, int requiredPoints);

    /// <summary>
    /// Changes the layout of the given space to the given layout.
    /// </summary>
    /// <param name="spaceVm">Space to edit.</param>
    /// <param name="floorPlanName">Enum of the FloorPlan to change the layout to.</param>
    void ChangeSpaceLayout(ISpaceViewModel spaceVm, FloorPlanEnum floorPlanName);
    
    /// <summary>
    /// Deletes the given space in the given world.
    /// </summary>
    /// <param name="worldVm">Parent world of the space.</param>
    /// <param name="spaceVm">Space to delete.</param>
    void DeleteSpace(IWorldViewModel worldVm, ISpaceViewModel spaceVm);
    
    /// <summary>
    /// Asks user for path and saves <see cref="SpaceViewModel"/> to disk.
    /// </summary>
    /// <param name="spaceViewModel">The space which should be saved.</param>
    /// <returns>Task indicating completion.</returns>
    /// <exception cref="OperationCanceledException">Operation was cancelled by user.</exception>
    /// <exception cref="NotImplementedException">Thrown when we are not running in Electron.</exception>
    /// <exception cref="InvalidOperationException">Thrown when we are running in Electron but no <see cref="IElectronDialogManager"/>
    /// implementation is present in dependency injection container.</exception>
    Task SaveSpaceAsync(SpaceViewModel spaceViewModel);
    
    /// <summary>
    /// Asks user for path and loads <see cref="SpaceViewModel"/> from disk.
    /// </summary>
    /// <param name="worldVm">World into which the  space should be loaded.</param>
    /// <returns>Task containing deserialized object.</returns>
    /// <exception cref="OperationCanceledException">Operation was cancelled by user.</exception>
    /// <exception cref="NotImplementedException">Thrown when we are not running in Electron.</exception>
    /// <exception cref="InvalidOperationException">Thrown when we are running in Electron but no <see cref="IElectronDialogManager"/>
    /// implementation is present in dependency injection container.</exception>
    Task LoadSpaceAsync(IWorldViewModel worldVm);
    
    /// <summary>
    /// Creates a new pathway condition in the given  world with the corresponding command.
    /// </summary>
    /// <param name="worldVm">Parent  world of the condition to create.</param>
    /// <param name="condition">Enum that can either be an AND or an OR condition.</param>
    /// <param name="positionX">X-coordinate of the condition to create. </param>
    /// <param name="positionY">Y-coordinate of the condition to create.</param>
    void CreatePathWayCondition(IWorldViewModel worldVm, ConditionEnum condition, double positionX,
        double positionY);

    /// <summary>
    /// Creates a new pathway condition between two given objects in the given  world.
    /// </summary>
    /// <param name="worldVm">Parent  world of the condition to create.</param>
    /// <param name="condition">Enum that can either be an AND or an OR condition.</param>
    /// <param name="sourceObject">Inbound object of the new pathway condition.</param>
    /// <param name="targetObject">Outbound object of the new pathway condition.</param>
    void CreatePathWayConditionBetweenObjects(IWorldViewModel worldVm, ConditionEnum condition,
        IObjectInPathWayViewModel sourceObject, ISpaceViewModel targetObject);

    /// <summary>
    /// Edits the given pathway condition in the given  world with the corresponding command.
    /// </summary>
    /// <param name="pathWayConditionVm">The path way condition to be edited.</param>
    /// <param name="newCondition">The new condition to be set.</param>
    void EditPathWayCondition(PathWayConditionViewModel pathWayConditionVm, ConditionEnum newCondition);

    /// <summary>
    /// Deletes the given pathway condition in the given  world.
    /// </summary>
    /// <param name="worldVm">Parent  world.</param>
    /// <param name="pathWayConditionVm">Pathway condition to be deleted.</param>
    void DeletePathWayCondition(IWorldViewModel worldVm, PathWayConditionViewModel pathWayConditionVm);

    /// <summary>
    /// Adds a new  pathway between two objects ( space or pathway condition) in the given  world.
    /// </summary>
    /// <param name="worldVm">World into which the pathway gets created.</param>
    /// <param name="sourceObjectVm">Object from which the path starts.</param>
    /// <param name="targetObjectVm">Object where the path ends.</param>
    void CreatePathWay(IWorldViewModel worldVm, IObjectInPathWayViewModel sourceObjectVm,
        IObjectInPathWayViewModel targetObjectVm);

    /// <summary>
    /// Deletes the last pathway that was created to the targetSpace.
    /// </summary>
    /// <param name="worldVm">World in which the pathway gets deleted.</param>
    /// <param name="pathWayVm">PathWay to delete.</param>
    void DeletePathWay(IWorldViewModel worldVm, IPathWayViewModel pathWayVm);

    /// <summary>
    /// Adds a new element to its parent space.
    /// </summary>
    /// <param name="parentSpaceVm">Parent space of the element.</param>
    /// <param name="slotIndex">Index of the slot in which the element should be created.</param>
    /// <param name="elementVm">Element to add.</param>
    void AddElement(ISpaceViewModel parentSpaceVm, int slotIndex, IElementViewModel elementVm);

    /// <summary>
    /// Creates a new element and assigns it to the opened space in the
    /// selected world.
    /// </summary>
    /// <param name="parentSpaceVm">Parent space of the element.</param>
    /// <param name="slotIndex">Index of the slot in which the element should be created.</param>
    /// <param name="name">Name of the element.</param>
    /// <param name="shortname">Shortname of the element.</param>
    /// <param name="elementType">Type of the element.</param>
    /// <param name="contentType">Type of the content that the element contains.</param>
    /// <param name="contentVm">The content of the element.</param>
    /// <param name="url"></param>
    /// <param name="authors">A list of authors of the element.</param>
    /// <param name="description">A description of the element.</param>
    /// <param name="goals">The goals of the element.</param>
    /// <param name="difficulty">The difficulty of the element.</param>
    /// <param name="workload">The time required to complete the element.</param>
    /// <param name="points">The number of points of the element.</param>
    /// <param name="positionX"></param>
    /// <param name="positionY"></param>
    void CreateElement(ISpaceViewModel parentSpaceVm, int slotIndex, string name, string shortname,
        ElementTypeEnum elementType, ContentTypeEnum contentType, ContentViewModel contentVm,
        string url, string authors, string description, string goals, ElementDifficultyEnum difficulty, int workload, int points,
        double positionX = 0, double positionY = 0);

    /// <summary>
    /// Edits a given element in the opened space with the corresponding command.
    /// </summary>
    /// <param name="elementVm">Element to edit.</param>
    /// <param name="parentSpaceVm">Parent space of the element.</param>
    /// <param name="name">Name of the element.</param>
    /// <param name="shortname">Shortname of the element.</param>
    /// <param name="authors">A list of authors of the element.</param>
    /// <param name="description">A description of the element.</param>
    /// <param name="goals">The goals of the element.</param>
    /// <param name="difficulty">The difficulty of the element.</param>
    /// <param name="workload">The time required to complete the element.</param>
    /// <param name="points">The number of points of the element.</param>
    void EditElement(ISpaceViewModel parentSpaceVm,
        IElementViewModel elementVm, string name, string shortname, string url, string authors, string description,
        string goals, ElementDifficultyEnum difficulty, int workload, int points);

    /// <summary>
    /// Moves the given element from unplaced elements to the given slot index in the given space.
    /// </summary>
    /// <param name="worldVm">World with the unplaced elements.</param>
    /// <param name="spaceVm">Space to place the element in.</param>
    /// <param name="elementVm">Element to place.</param>
    /// <param name="newSlotIndex">Index of the slot in the space to place the element in.</param>
    void DragElementFromUnplaced(IWorldViewModel worldVm,
        ISpaceViewModel spaceVm, IElementViewModel elementVm, int newSlotIndex);

    /// <summary>
    /// Moves the given element from the space to unplaced elements in the world.
    /// </summary>
    /// <param name="worldVm">World with the unplaced elements.</param>
    /// <param name="spaceVm">Space from which the element should be removed.</param>
    /// <param name="elementVm">Element to remove.</param>
    void DragElementToUnplaced(IWorldViewModel worldVm, ISpaceViewModel spaceVm,
        IElementViewModel elementVm);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="spaceVm"></param>
    /// <param name="elementVm"></param>
    /// <param name="newSlotIndex"></param>
    void SwitchElementSlot(ISpaceViewModel spaceVm, IElementViewModel elementVm,
        int newSlotIndex);

    /// <summary>
    /// Deletes the given element in the given space.
    /// </summary>
    /// <param name="parentSpaceVm">Parent space of the element.</param>
    /// <param name="elementVm">Element to delete.</param>
    void DeleteElement(ISpaceViewModel parentSpaceVm,
        IElementViewModel elementVm);
    
    /// <summary>
    /// Asks user for path and saves <see cref="ElementViewModel"/> to disk.
    /// </summary>
    /// <param name="elementViewModel">The element which should be saved.</param>
    /// <returns>Task indicating completion.</returns>
    /// <exception cref="OperationCanceledException">Operation was cancelled by user.</exception>
    /// <exception cref="NotImplementedException">Thrown when we are not running in Electron.</exception>
    /// <exception cref="InvalidOperationException">Thrown when we are running in Electron but no <see cref="IElectronDialogManager"/>
    /// implementation is present in dependency injection container.</exception>
    Task SaveElementAsync(ElementViewModel elementViewModel);

    /// <summary>
    /// Asks user for path and loads <see cref="ElementViewModel"/> from disk.
    /// </summary>
    /// <param name="parentSpaceVm">Space into which the element should be loaded.</param>
    /// <param name="slotIndex">Index of the slot in which the element should be loaded..</param>
    /// <returns>Task containing deserialized object.</returns>
    /// <exception cref="OperationCanceledException">Operation was cancelled by user.</exception>
    /// <exception cref="NotImplementedException">Thrown when we are not running in Electron.</exception>
    /// <exception cref="InvalidOperationException">Thrown when we are running in Electron but no <see cref="IElectronDialogManager"/>
    /// implementation is present in dependency injection container.</exception>
    Task LoadElementAsync(ISpaceViewModel parentSpaceVm, int slotIndex);
    
    /// <summary>
    /// Open the given content file of the element in the desktop's default manner.
    /// </summary>
    /// <param name="elementVm">Element which contains the content file to be opened.</param>
    /// <returns></returns>
    Task ShowElementContentAsync(ElementViewModel elementVm);
    
    /// <summary>
    /// Asks user for path, loads an image file from disk and returns a <see cref="ContentViewModel"/>. 
    /// </summary>
    /// <returns></returns>
    Task<ContentViewModel> LoadImageAsync();
    
    /// <summary>
    /// Asks user for path, loads a video file from disk and returns a <see cref="ContentViewModel"/>. 
    /// </summary>
    /// <returns></returns>
    Task<ContentViewModel> LoadVideoAsync();
    
    /// <summary>
    /// Asks user for path, loads a h5p file from disk and returns a <see cref="ContentViewModel"/>. 
    /// </summary>
    /// <returns></returns>
    Task<ContentViewModel> LoadH5PAsync();
    
    /// <summary>
    /// Asks user for path, loads a pdf file from disk and returns a <see cref="ContentViewModel"/>. 
    /// </summary>
    /// <returns></returns>
    Task<ContentViewModel> LoadPdfAsync();
    
    /// <summary>
    /// Asks user for path, loads a text file from disk and returns a <see cref="ContentViewModel"/>. 
    /// </summary>
    /// <returns></returns>
    Task<ContentViewModel> LoadTextAsync();

    ContentViewModel LoadContentViewModel(string name, MemoryStream stream);
    void LoadWorldViewModel(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm, Stream stream);
    void LoadSpaceViewModel(IWorldViewModel worldVm, Stream stream);
    void LoadElementViewModel(ISpaceViewModel parentSpaceVm, int slotIndex, Stream stream);
    event Action? OnUndoRedoPerformed;
    void DragObjectInPathWay(IObjectInPathWayViewModel pathWayObjectVm, double oldPositionX, double oldPositionY);
    void DragElement(IElementViewModel elementVm, double oldPositionX, double oldPositionY);
}