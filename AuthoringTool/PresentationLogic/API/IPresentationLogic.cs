using AuthoringTool.API.Configuration;
using AuthoringTool.BusinessLogic.API;
using AuthoringTool.PresentationLogic.ElectronNET;
using AuthoringTool.PresentationLogic.LearningContent;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.LearningWorld;

namespace AuthoringTool.PresentationLogic.API;

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
    Task<LearningWorldViewModel> LoadLearningWorldAsync();
    
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
    /// <returns>Task containing deserialized object.</returns>
    /// <exception cref="OperationCanceledException">Operation was cancelled by user.</exception>
    /// <exception cref="NotImplementedException">Thrown when we are not running in Electron.</exception>
    /// <exception cref="InvalidOperationException">Thrown when we are running in Electron but no <see cref="IElectronDialogManager"/>
    /// implementation is present in dependency injection container.</exception>
    Task<ILearningSpaceViewModel> LoadLearningSpaceAsync();
    
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
    /// <returns>Task containing deserialized object.</returns>
    /// <exception cref="OperationCanceledException">Operation was cancelled by user.</exception>
    /// <exception cref="NotImplementedException">Thrown when we are not running in Electron.</exception>
    /// <exception cref="InvalidOperationException">Thrown when we are running in Electron but no <see cref="IElectronDialogManager"/>
    /// implementation is present in dependency injection container.</exception>
    Task<LearningElementViewModel> LoadLearningElementAsync();
    
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
    Task<LearningContentViewModel> LoadH5pAsync();
    
    /// <summary>
    /// Asks user for path, loads a pdf file from disk and returns a <see cref="LearningContentViewModel"/>. 
    /// </summary>
    /// <returns></returns>
    Task<LearningContentViewModel> LoadPdfAsync();

    LearningContentViewModel LoadLearningContentViewModelFromStream(string name, Stream stream);
    LearningWorldViewModel LoadLearningWorldViewModelFromStream(Stream stream);
    ILearningSpaceViewModel LoadLearningSpaceViewModelFromStream(Stream stream);
    LearningElementViewModel LoadLearningElementViewModelFromStream(Stream stream);
}