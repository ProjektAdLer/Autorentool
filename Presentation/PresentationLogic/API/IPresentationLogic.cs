using BusinessLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.ElectronNET;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
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
    /// <param name="learningWorldViewModel"></param>
    /// <exception cref="OperationCanceledException">Operation was cancelled by user.</exception>
    /// <returns>Filepath to the new backup file</returns>
    Task<string> ConstructBackupAsync(LearningWorldViewModel learningWorldViewModel);

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
    void DeleteLearningWorld(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm, LearningWorldViewModel worldVm);
    
    /// <summary>
    /// Asks user for path and saves <see cref="LearningWorldViewModel"/> to disk.
    /// </summary>
    /// <param name="learningWorldViewModel">The learning world which should be saved.</param>
    /// <returns>Task indicating completion.</returns>
    /// <exception cref="OperationCanceledException">Operation was cancelled by user.</exception>
    /// <exception cref="NotImplementedException">Thrown when we are not running in Electron.</exception>
    /// <exception cref="InvalidOperationException">Thrown when we are running in Electron but no <see cref="IElectronDialogManager"/>
    /// implementation is present in dependency injection container.</exception>
    Task SaveLearningWorldAsync(LearningWorldViewModel learningWorldViewModel);
    
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
    /// <param name="shortname"></param>
    /// <param name="authors"></param>
    /// <param name="description"></param>
    /// <param name="goals"></param>
    /// <param name="requiredPoints"></param>
    void CreateLearningSpace(ILearningWorldViewModel learningWorldVm, string name, string shortname,
        string authors, string description, string goals, int requiredPoints);

    /// <summary>
    /// Edits a given learning space in the given learning world with the corresponding command.
    /// </summary>
    /// <param name="learningSpaceVm">Learning space to edit.</param>
    /// <param name="name"></param>
    /// <param name="shortname"></param>
    /// <param name="authors"></param>
    /// <param name="description"></param>
    /// <param name="goals"></param>
    /// <param name="requiredPoints"></param>
    void EditLearningSpace(ILearningSpaceViewModel learningSpaceVm, string name,
        string shortname, string authors, string description, string goals, int requiredPoints);

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
    /// Adds a new learning element to its parent.
    /// </summary>
    /// <param name="elementParentVm">Parent of the element that can either be a world or a space.</param>
    /// <param name="learningElementVm">Learning element to add.</param>
    void AddLearningElement(ILearningElementViewModelParent elementParentVm, ILearningElementViewModel learningElementVm);
    
    /// <summary>
    /// Creates a new learning element and assigns it to the selected learning world or to a learning space in the
    /// selected learning world.
    /// </summary>
    /// <param name="name">Name of the element.</param>
    /// <param name="shortname">Shortname of the element.</param>
    /// <param name="elementParentVm">Parent of the element that can either be a world or a space.</param>
    /// <param name="elementType">Type of the element.</param>
    /// <param name="contentType">Type of the content that the element contains.</param>
    /// <param name="learningContentVm">The content of the element.</param>
    /// <param name="authors">A list of authors of the element.</param>
    /// <param name="description">A description of the element.</param>
    /// <param name="goals">The goals of the element.</param>
    /// <param name="difficulty">The difficulty of the element.</param>
    /// <param name="workload">The time required to complete the learning element.</param>
    /// <param name="points">The number of points of the learning element.</param>
    void CreateLearningElement(ILearningElementViewModelParent elementParentVm, string name, string shortname,
        ElementTypeEnum elementType, ContentTypeEnum contentType, LearningContentViewModel learningContentVm,
        string authors, string description, string goals, LearningElementDifficultyEnum difficulty, int workload, int points);

    /// <summary>
    /// Edits a given learning element either in the learning world or learning space with the corresponding command.
    /// </summary>
    /// <param name="learningElementVm">Element to edit.</param>
    /// <param name="learningElementParentVm">Parent of the element that can either be a world or a space.</param>
    /// <param name="name">Name of the element.</param>
    /// <param name="shortname">Shortname of the element.</param>
    /// <param name="authors">A list of authors of the element.</param>
    /// <param name="description">A description of the element.</param>
    /// <param name="goals">The goals of the element.</param>
    /// <param name="difficulty">The difficulty of the element.</param>
    /// <param name="workload">The time required to complete the learning element.</param>
    /// <param name="points">The number of points of the learning element.</param>
    void EditLearningElement(ILearningElementViewModelParent learningElementParentVm,
        ILearningElementViewModel learningElementVm, string name, string shortname, string authors, string description,
        string goals, LearningElementDifficultyEnum difficulty, int workload, int points);

    /// <summary>
    /// Deletes the given learning element either in the given learning world or in the given learning space.
    /// </summary>
    /// <param name="elementParentVm">Element to delete.</param>
    /// <param name="learningElementVm">Parent of the element that can either be a world or a space.</param>
    void DeleteLearningElement(ILearningElementViewModelParent elementParentVm,
        LearningElementViewModel learningElementVm);
    
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
    /// <param name="elementParentVm">Either a learning world or a learning space into which the learning space
    /// should be loaded.</param>
    /// <returns>Task containing deserialized object.</returns>
    /// <exception cref="OperationCanceledException">Operation was cancelled by user.</exception>
    /// <exception cref="NotImplementedException">Thrown when we are not running in Electron.</exception>
    /// <exception cref="InvalidOperationException">Thrown when we are running in Electron but no <see cref="IElectronDialogManager"/>
    /// implementation is present in dependency injection container.</exception>
    Task LoadLearningElementAsync(ILearningElementViewModelParent elementParentVm);
    
    /// <summary>
    /// Asks user for path, loads an image file from disk and returns a <see cref="LearningContentViewModel"/>. 
    /// </summary>
    /// <returns></returns>
    Task<LearningContentViewModel> LoadImageAsync();
    
    /// <summary>
    /// Asks user for path, loads a video file from disk and returns a <see cref="LearningContentViewModel"/>. 
    /// </summary>
    /// <returns></returns>
    Task<LearningContentViewModel> LoadVideoAsync();
    
    /// <summary>
    /// Asks user for path, loads a h5p file from disk and returns a <see cref="LearningContentViewModel"/>. 
    /// </summary>
    /// <returns></returns>
    Task<LearningContentViewModel> LoadH5PAsync();
    
    /// <summary>
    /// Asks user for path, loads a pdf file from disk and returns a <see cref="LearningContentViewModel"/>. 
    /// </summary>
    /// <returns></returns>
    Task<LearningContentViewModel> LoadPdfAsync();
    
    /// <summary>
    /// Asks user for path, loads a text file from disk and returns a <see cref="LearningContentViewModel"/>. 
    /// </summary>
    /// <returns></returns>
    Task<LearningContentViewModel> LoadTextAsync();

    LearningContentViewModel LoadLearningContentViewModel(string name, Stream stream);
    void LoadLearningWorldViewModel(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm, Stream stream);
    void LoadLearningSpaceViewModel(ILearningWorldViewModel learningWorldVm, Stream stream);
    void LoadLearningElementViewModel(ILearningElementViewModelParent parentVm, Stream stream);
}