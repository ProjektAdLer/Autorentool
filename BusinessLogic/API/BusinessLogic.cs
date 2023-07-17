using BusinessLogic.Commands;
using BusinessLogic.Entities;
using BusinessLogic.Entities.BackendAccess;
using BusinessLogic.Entities.LearningContent;
using BusinessLogic.ErrorManagement;
using BusinessLogic.ErrorManagement.BackendAccess;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Command;
using Shared.Configuration;

namespace BusinessLogic.API;

public class BusinessLogic : IBusinessLogic
{
    public BusinessLogic(
        IApplicationConfiguration configuration,
        IDataAccess dataAccess,
        IWorldGenerator worldGenerator,
        ICommandStateManager commandStateManager,
        IBackendAccess backendAccess,
        IErrorManager errorManager,
        ILogger<BusinessLogic> logger)
    {
        Configuration = configuration;
        DataAccess = dataAccess;
        WorldGenerator = worldGenerator;
        CommandStateManager = commandStateManager;
        BackendAccess = backendAccess;
        ErrorManager = errorManager;
        Logger = logger;
    }


    internal IWorldGenerator WorldGenerator { get; }
    internal ICommandStateManager CommandStateManager { get; }
    internal IErrorManager ErrorManager { get; }
    internal ILogger<BusinessLogic> Logger { get; }
    public IBackendAccess BackendAccess { get; }
    internal IDataAccess DataAccess { get; }
    public IApplicationConfiguration Configuration { get; }
    public event EventHandler<CommandUndoRedoOrExecuteArgs> OnCommandUndoRedoOrExecute;
    public bool CanUndo => CommandStateManager.CanUndo;
    public bool CanRedo => CommandStateManager.CanRedo;

    /// <inheritdoc cref="IBusinessLogic.GetAllContent" />
    public IEnumerable<ILearningContent> GetAllContent()
    {
        return DataAccess.GetAllContent();
    }

    /// <inheritdoc cref="IBusinessLogic.RemoveContent" />
    public void RemoveContent(ILearningContent content)
    {
        DataAccess.RemoveContent(content);
        Logger.LogTrace("Removed {type} : {name}", content.GetType(), content.Name);
    }

    /// <inheritdoc cref="IBusinessLogic.SaveLink" />
    public void SaveLink(LinkContent linkContent)
    {
        DataAccess.SaveLink(linkContent);
        Logger.LogTrace("Saved link: {link} with name: {linkName}", linkContent.Link, linkContent.Name);
    }

    /// <inheritdoc cref="IBusinessLogic.ExecuteCommand" />
    public void ExecuteCommand(ICommand command)
    {
        CommandStateManager.Execute(command);
        OnCommandUndoRedoOrExecute?.Invoke(this,
            new CommandUndoRedoOrExecuteArgs(command.Name, CommandExecutionState.Executed));
        Logger.LogTrace("Executed command: {commandName}", command.Name);
    }

    /// <inheritdoc cref="IBusinessLogic.UndoCommand" />
    public void UndoCommand()
    {
        try
        {
            var command = CommandStateManager.Undo();
            OnCommandUndoRedoOrExecute?.Invoke(this,
                new CommandUndoRedoOrExecuteArgs(command.Name, CommandExecutionState.Undone));
            Logger.LogTrace("Undone command: {commandName}", command.Name);
        }
        catch (InvalidOperationException e)
        {
            ErrorManager.LogAndRethrowUndoError(e);
        }
        catch (ArgumentOutOfRangeException e)
        {
            ErrorManager.LogAndRethrowUndoError(e);
        }
    }

    /// <inheritdoc cref="IBusinessLogic.RedoCommand" />
    public void RedoCommand()
    {
        try
        {
            var command = CommandStateManager.Redo();
            OnCommandUndoRedoOrExecute?.Invoke(this,
                new CommandUndoRedoOrExecuteArgs(command.Name, CommandExecutionState.Redone));
            Logger.LogTrace("Redone command: {commandName}", command.Name);
        }
        catch (InvalidOperationException e)
        {
            ErrorManager.LogAndRethrowRedoError(e);
        }
        catch (ApplicationException e)
        {
            ErrorManager.LogAndRethrowRedoError(e);
        }
    }

    public void ConstructBackup(LearningWorld learningWorld, string filepath)
    {
        try
        {
            WorldGenerator.ConstructBackup(learningWorld, filepath);
            Logger.LogTrace("Constructed backup for learning world: {learningWorldName} at path: {filepath}",
                learningWorld.Name, filepath);
        }
        catch (ArgumentOutOfRangeException e)
        {
            ErrorManager.LogAndRethrowGeneratorError(e);
        }
        catch (InvalidOperationException e)
        {
            ErrorManager.LogAndRethrowGeneratorError(e);
        }
        catch (FileNotFoundException e)
        {
            ErrorManager.LogAndRethrowGeneratorError(e);
        }
    }

    public void SaveLearningWorld(LearningWorld learningWorld, string filepath)
    {
        DataAccess.SaveLearningWorldToFile(learningWorld, filepath);
        Logger.LogTrace("Saved learning world: {learningWorldName} at path: {filepath}", learningWorld.Name, filepath);
    }

    public LearningWorld LoadLearningWorld(string filepath)
    {
        var world = DataAccess.LoadLearningWorld(filepath);
        Logger.LogTrace("Loaded learning world {name} from path: {filepath}", world, filepath);
        return world;
    }

    public void SaveLearningSpace(LearningSpace learningSpace, string filepath)
    {
        DataAccess.SaveLearningSpaceToFile(learningSpace, filepath);
        Logger.LogTrace("Saved learning space: {learningSpaceName} at path: {filepath}", learningSpace.Name, filepath);
    }

    public LearningSpace LoadLearningSpace(string filepath)
    {
        var space = DataAccess.LoadLearningSpace(filepath);
        Logger.LogTrace("Loaded learning space {name} from path: {filepath}", space, filepath);
        return space;
    }

    public void SaveLearningElement(LearningElement learningElement, string filepath)
    {
        DataAccess.SaveLearningElementToFile(learningElement, filepath);
        Logger.LogTrace("Saved learning element: {learningElementName} at path: {filepath}", learningElement.Name,
            filepath);
    }

    public LearningElement LoadLearningElement(string filepath)
    {
        var learningElement = DataAccess.LoadLearningElement(filepath);
        Logger.LogTrace("Loaded learning element {name} from path: {filepath}", learningElement, filepath);
        return learningElement;
    }

    public ILearningContent LoadLearningContent(string filepath)
    {
        var content = DataAccess.LoadLearningContent(filepath);
        Logger.LogTrace("Loaded learning content {name} from path: {filepath}", content, filepath);
        return content;
    }

    public ILearningContent LoadLearningContent(string name, Stream stream)
    {
        var content = DataAccess.LoadLearningContent(name, stream);
        Logger.LogTrace("Loaded learning content {name} from stream: {stream}", content, stream);
        return content;
    }

    public IEnumerable<SavedLearningWorldPath> GetSavedLearningWorldPaths()
    {
        return DataAccess.GetSavedLearningWorldPaths();
    }

    public void AddSavedLearningWorldPath(SavedLearningWorldPath savedLearningWorldPath)
    {
        DataAccess.AddSavedLearningWorldPath(savedLearningWorldPath);
        Logger.LogTrace("Added saved learning world path: {name} at path: {path}", savedLearningWorldPath.Name, savedLearningWorldPath.Path);
    }

    public SavedLearningWorldPath AddSavedLearningWorldPathByPathOnly(string path)
    {
        var savedLearningWorldPath = DataAccess.AddSavedLearningWorldPathByPathOnly(path);
        Logger.LogTrace("Added saved learning world path at path: {path}", path);
        return savedLearningWorldPath;
    }

    public void UpdateIdOfSavedLearningWorldPath(SavedLearningWorldPath savedLearningWorldPath, Guid id)
    {
        DataAccess.UpdateIdOfSavedLearningWorldPath(savedLearningWorldPath, id);
        Logger.LogTrace("Updated id of saved learning world path: {name} to id: {id}", savedLearningWorldPath.Name, id);
    }

    public void RemoveSavedLearningWorldPath(SavedLearningWorldPath savedLearningWorldPath)
    {
        DataAccess.RemoveSavedLearningWorldPath(savedLearningWorldPath);
        Logger.LogTrace("Removed saved learning world path: {name} at path: {path}", savedLearningWorldPath.Name, savedLearningWorldPath.Path);
    }

    public LearningWorld LoadLearningWorld(Stream stream)
    {
        var world = DataAccess.LoadLearningWorld(stream);
        Logger.LogTrace("Loaded learning world {name} from stream: {stream}", world, stream);
        return world;
    }

    public LearningSpace LoadLearningSpace(Stream stream)
    {
        var space = DataAccess.LoadLearningSpace(stream);
        Logger.LogTrace("Loaded learning space {name} from stream: {stream}", space, stream);
        return space;
    }

    public LearningElement LoadLearningElement(Stream stream)
    {
        var learningElement = DataAccess.LoadLearningElement(stream);
        Logger.LogTrace("Loaded learning element {name} from stream: {stream}", learningElement, stream);
        return learningElement;
    }

    public string FindSuitableNewSavePath(string targetFolder, string fileName, string fileEnding)
    {
        var targetPath = DataAccess.FindSuitableNewSavePath(targetFolder, fileName, fileEnding);
        Logger.LogTrace("Found suitable new save path: {path}", targetPath);
        return targetPath;
    }

    public string GetContentFilesFolderPath()
    {
        return DataAccess.GetContentFilesFolderPath();
    }

    #region BackendAccess

    //TODO: Move this away from here
    private UserInformation _userInformation = new("", false, 0, "");

    public async Task<bool> IsLmsConnected()
    {
        await UpdateUserInformation();
        return _userInformation.LmsUsername != "";
    }

    public string LoginName => _userInformation.LmsUsername;

    private async Task UpdateUserInformation()
    {
        if (Configuration[IApplicationConfiguration.BackendToken] != "")
        {
            _userInformation =
                await BackendAccess.GetUserInformationAsync(
                    new UserToken(Configuration[IApplicationConfiguration.BackendToken]));
        }
        else
        {
            Logout();
        }
    }

    public async Task Login(string username, string password)
    {
        try
        {
            var token = await BackendAccess.GetUserTokenAsync(username, password);
            Configuration[IApplicationConfiguration.BackendToken] = token.Token;
            Logger.LogTrace("Logged in user: {username}", username);
        }
        catch (BackendInvalidLoginException e)
        {
            Logout();
            throw;
        }
        catch (BackendInvalidUrlException e)
        {
            Logout();
            throw;
        }

        await UpdateUserInformation();
    }

    public void Logout()
    {
        Configuration[IApplicationConfiguration.BackendToken] = "";
        _userInformation.LmsUsername = "";
        _userInformation.IsLmsAdmin = false;
        _userInformation.LmsId = 0;
        _userInformation.LmsEmail = "";
        Logger.LogTrace("Logged out user");
    }

    public void UploadLearningWorldToBackend(string filepath, IProgress<int>? progress = null)
    {
        var atfPath = WorldGenerator.ExtractAtfFromBackup(filepath);
        BackendAccess.UploadLearningWorldAsync(new UserToken(Configuration[IApplicationConfiguration.BackendToken]),
            filepath, atfPath, progress);
        Logger.LogTrace("Uploaded learning world to backend from backupPath: {path}", filepath);
    }

    #endregion
}