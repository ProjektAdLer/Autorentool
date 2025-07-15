using System.IO.Abstractions;
using System.Runtime.Serialization;
using BusinessLogic.Commands;
using BusinessLogic.Entities;
using BusinessLogic.Entities.BackendAccess;
using BusinessLogic.Entities.LearningContent;
using BusinessLogic.Entities.LearningContent.FileContent;
using BusinessLogic.Entities.LearningContent.LinkContent;
using BusinessLogic.ErrorManagement;
using BusinessLogic.ErrorManagement.BackendAccess;
using BusinessLogic.Validation.Validators;
using Microsoft.Extensions.Logging;
using Shared.Command;
using Shared.Configuration;
using Shared;

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
        ILogger<BusinessLogic> logger,
        ILearningWorldStructureValidator learningWorldStructureValidator)
    {
        Configuration = configuration;
        DataAccess = dataAccess;
        WorldGenerator = worldGenerator;
        CommandStateManager = commandStateManager;
        BackendAccess = backendAccess;
        ErrorManager = errorManager;
        Logger = logger;
        LearningWorldStructureValidator = learningWorldStructureValidator;
    }


    internal IWorldGenerator WorldGenerator { get; }
    internal ICommandStateManager CommandStateManager { get; }
    internal IErrorManager ErrorManager { get; }
    internal ILogger<BusinessLogic> Logger { get; }
    public IBackendAccess BackendAccess { get; }
    internal IDataAccess DataAccess { get; }
    internal ILearningWorldStructureValidator LearningWorldStructureValidator { get; }
    public IApplicationConfiguration Configuration { get; }
    public event EventHandler<CommandUndoRedoOrExecuteArgs>? OnCommandUndoRedoOrExecute;
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
        try
        {
            DataAccess.RemoveContent(content);
            Logger.LogTrace("Removed {Type} : {Name}", content.GetType(), content.Name);
        }
        catch (ArgumentOutOfRangeException e)
        {
            ErrorManager.LogAndRethrowError(e);
        }
        catch (FileNotFoundException e)
        {
            ErrorManager.LogAndRethrowError(e);
        }
        catch (SerializationException e)
        {
            ErrorManager.LogAndRethrowError(e);
        }
    }

    /// <inheritdoc cref="IBusinessLogic.RemoveMultipleContents"/>
    public void RemoveMultipleContents(IEnumerable<ILearningContent> contents)
    {
        try
        {
            foreach (var content in contents)
            {
                DataAccess.RemoveContent(content);
                Logger.LogTrace("Removed {Type} : {Name}", content.GetType(), content.Name);
            }
        }
        catch (ArgumentOutOfRangeException e)
        {
            ErrorManager.LogAndRethrowError(e);
        }
        catch (FileNotFoundException e)
        {
            ErrorManager.LogAndRethrowError(e);
        }
        catch (SerializationException e)
        {
            ErrorManager.LogAndRethrowError(e);
        }
    }

    /// <inheritdoc cref="IBusinessLogic.SaveLink" />
    public void SaveLink(LinkContent linkContent)
    {
        try
        {
            DataAccess.SaveLink(linkContent);
            Logger.LogTrace("Saved link: {Link} with name: {LinkName}", linkContent.Link, linkContent.Name);
        }
        catch (SerializationException e)
        {
            ErrorManager.LogAndRethrowError(e);
        }
    }

    /// <inheritdoc cref="IBusinessLogic.ExecuteCommand" />
    public void ExecuteCommand(ICommand command)
    {
        CommandStateManager.Execute(command);
        OnCommandUndoRedoOrExecute?.Invoke(this,
            new CommandUndoRedoOrExecuteArgs(command.Name, CommandExecutionState.Executed));
    }

    /// <inheritdoc cref="IBusinessLogic.UndoCommand" />
    public void UndoCommand()
    {
        try
        {
            var command = CommandStateManager.Undo();
            OnCommandUndoRedoOrExecute?.Invoke(this,
                new CommandUndoRedoOrExecuteArgs(command.Name, CommandExecutionState.Undone));
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

    /// <inheritdoc cref="IBusinessLogic.ConstructBackup" />
    public void ConstructBackup(LearningWorld learningWorld, string filepath)
    {
        try
        {
            WorldGenerator.ConstructBackup(learningWorld, filepath);
            Logger.LogTrace(
                "Constructed backup for learning world: {LearningWorldName} with id {LearningWorldId} at path: {Filepath}",
                learningWorld.Name, learningWorld.Id, filepath);
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
        try
        {
            DataAccess.SaveLearningWorldToFile(learningWorld, filepath);
        }
        catch (SerializationException e)
        {
            ErrorManager.LogAndRethrowError(e);
        }
    }

    public LearningWorld LoadLearningWorld(string filepath)
    {
        try
        {
            var world = DataAccess.LoadLearningWorld(filepath);
            return world;
        }
        catch (SerializationException e)
        {
            ErrorManager.LogAndRethrowError(e);
            return null!;
        }
    }

    public async Task<ILearningContent> LoadLearningContentAsync(string filepath)
    {
        return await DataAccess.LoadLearningContentAsync(filepath);
    }

    /// <inheritdoc cref="IBusinessLogic.LoadLearningContentAsync(string,System.IO.Stream)"/>
    public async Task<ILearningContent> LoadLearningContentAsync(string name, Stream stream)
    {
        try
        {
            return await DataAccess.LoadLearningContentAsync(name, stream);
        }
        catch (IOException e)
        {
            ErrorManager.LogAndRethrowError(e);
            return null!;
        }
    }

    public IEnumerable<IFileInfo> GetSavedLearningWorldPaths()
    {
        return DataAccess.GetSavedLearningWorldPaths();
    }

    public string FindSuitableNewSavePath(string targetFolder, string fileName, string fileEnding, out int iterations)
    {
        var targetPath = DataAccess.FindSuitableNewSavePath(targetFolder, fileName, fileEnding, out iterations);
        return targetPath;
    }

    public string GetContentFilesFolderPath()
    {
        return DataAccess.GetContentFilesFolderPath();
    }

    public async Task ExportLearningWorldToArchiveAsync(LearningWorld world, string pathToFile)
    {
        await DataAccess.ExportLearningWorldToArchiveAsync(world, pathToFile);
    }

    public async Task<LearningWorld> ImportLearningWorldFromArchiveAsync(string pathToFile)
    {
        return await DataAccess.ImportLearningWorldFromArchiveAsync(pathToFile);
    }

    public IFileInfo GetFileInfoForPath(string savePath)
    {
        return DataAccess.GetFileInfoForPath(savePath);
    }

    public void DeleteFileByPath(string savePath)
    {
        DataAccess.DeleteFileByPath(savePath);
    }

    public void EditH5PFileContent(IFileContent fileContent)
    {
        DataAccess.EditH5PFileContent(fileContent);
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
        if (Configuration[IApplicationConfiguration.BackendToken] == "")
        {
            Logout();
            return;
        }

        if (Configuration[IApplicationConfiguration.BackendBaseUrl] == "")
        {
            Logger.LogWarning("Tried to update user information without a backend url - logging out");
            Logout();
            return;
        }

        _userInformation =
            await BackendAccess.GetUserInformationAsync(
                new UserToken(Configuration[IApplicationConfiguration.BackendToken]));
    }

    public async Task Login(string username, string password)
    {
        try
        {
            var token = await BackendAccess.GetUserTokenAsync(username, password);
            Configuration[IApplicationConfiguration.BackendToken] = token.Token;
            Logger.LogTrace("Logged in user: {Username}", username);
        }
        catch (BackendInvalidLoginException)
        {
            Logout();
            throw;
        }
        catch (BackendInvalidUrlException)
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

    public async Task<UploadResponse> UploadLearningWorldToBackendAsync(string filepath,
        IProgress<int>? progress = null,
        CancellationToken? cancellationToken = null)
    {
        var atfPath = WorldGenerator.ExtractAtfFromBackup(filepath);
        try
        {
            var response = await BackendAccess.UploadLearningWorldAsync(
                new UserToken(Configuration[IApplicationConfiguration.BackendToken]),
                filepath, atfPath, progress, cancellationToken);
            Logger.LogTrace("Uploaded learning world to backend from backupPath: {Path}", filepath);
            return response;
        }
        catch (HttpRequestException httpReqEx)
        {
            ErrorManager.LogAndRethrowError(httpReqEx);
            throw;
        }
    }

    /// <inheritdoc cref="IBusinessLogic.DeleteLmsWorld"/>
    public async Task DeleteLmsWorld(LmsWorld world)
    {
        try
        {
            var result =
                await BackendAccess.DeleteLmsWorld(new UserToken(Configuration[IApplicationConfiguration.BackendToken]),
                    world);
            if (!result)
                ErrorManager.LogAndRethrowBackendAccessError(
                    new BackendWorldDeletionException("Lms world could not be deleted."));
        }
        catch (HttpRequestException e)
        {
            ErrorManager.LogAndRethrowBackendAccessError(e);
        }
    }

    /// <inheritdoc cref="IBusinessLogic.GetLmsWorldList"/>
    public async Task<List<LmsWorld>> GetLmsWorldList()
    {
        try
        {
            return await BackendAccess.GetLmsWorldList(
                new UserToken(Configuration[IApplicationConfiguration.BackendToken]),
                _userInformation.LmsId);
        }
        catch (HttpRequestException httpReqEx)
        {
            ErrorManager.LogAndRethrowBackendAccessError(httpReqEx);
            throw;
        }
    }

    #endregion

    #region LearningWorldStructureValidator

    /// <inheritdoc cref="IBusinessLogic.ValidateLearningWorldForExport"/>
    public ValidationResult ValidateLearningWorldForExport(LearningWorld world)
    {
        var content = DataAccess.GetAllContent();
        return LearningWorldStructureValidator.ValidateForExport(world, content.ToList());
    }

    /// <inheritdoc cref="IBusinessLogic.ValidateLearningWorldForGeneration"/>
    public ValidationResult ValidateLearningWorldForGeneration(LearningWorld world)
    {
        var content = DataAccess.GetAllContent();
        return LearningWorldStructureValidator.ValidateForGeneration(world, content.ToList());
    }
    
    #endregion

   
}