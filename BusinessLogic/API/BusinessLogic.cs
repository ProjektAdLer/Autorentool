using BusinessLogic.Commands;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using Shared;
using Shared.Command;
using Shared.Configuration;

namespace BusinessLogic.API;

public class BusinessLogic : IBusinessLogic
{
    public BusinessLogic(
        IAuthoringToolConfiguration configuration,
        IDataAccess dataAccess,
        IWorldGenerator worldGenerator,
        ICommandStateManager commandStateManager,
        IBackendAccess backendAccess)
    {
        Configuration = configuration;
        DataAccess = dataAccess;
        WorldGenerator = worldGenerator;
        CommandStateManager = commandStateManager;
        BackendAccess = backendAccess;
    }


    internal IWorldGenerator WorldGenerator { get; }
    internal ICommandStateManager CommandStateManager { get; }
    public IBackendAccess BackendAccess { get; }
    internal IDataAccess DataAccess { get; }
    public IAuthoringToolConfiguration Configuration { get; }
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
    }

    /// <inheritdoc cref="IBusinessLogic.SaveLink" />
    public void SaveLink(LinkContent linkContent)
    {
        DataAccess.SaveLink(linkContent);
    }

    public void ExecuteCommand(ICommand command)
    {
        CommandStateManager.Execute(command);
        OnCommandUndoRedoOrExecute?.Invoke(this,
            new CommandUndoRedoOrExecuteArgs(command.Name, CommandExecutionState.Executed));
    }

    public void UndoCommand()
    {
        var command = CommandStateManager.Undo();
        OnCommandUndoRedoOrExecute?.Invoke(this,
            new CommandUndoRedoOrExecuteArgs(command.Name, CommandExecutionState.Undone));
    }

    public void RedoCommand()
    {
        var command = CommandStateManager.Redo();
        OnCommandUndoRedoOrExecute?.Invoke(this,
            new CommandUndoRedoOrExecuteArgs(command.Name, CommandExecutionState.Redone));
    }

    public void ConstructBackup(LearningWorld learningWorld, string filepath)
    {
        WorldGenerator.ConstructBackup(learningWorld, filepath);
    }

    public void SaveLearningWorld(LearningWorld learningWorld, string filepath)
    {
        DataAccess.SaveLearningWorldToFile(learningWorld, filepath);
    }

    public LearningWorld LoadLearningWorld(string filepath)
    {
        return DataAccess.LoadLearningWorld(filepath);
    }

    public void SaveLearningSpace(LearningSpace learningSpace, string filepath)
    {
        DataAccess.SaveLearningSpaceToFile(learningSpace, filepath);
    }

    public LearningSpace LoadLearningSpace(string filepath)
    {
        return DataAccess.LoadLearningSpace(filepath);
    }

    public void SaveLearningElement(LearningElement learningElement, string filepath)
    {
        DataAccess.SaveLearningElementToFile(learningElement, filepath);
    }

    public LearningElement LoadLearningElement(string filepath)
    {
        return DataAccess.LoadLearningElement(filepath);
    }

    public ILearningContent LoadLearningContent(string filepath)
    {
        return DataAccess.LoadLearningContent(filepath);
    }

    public ILearningContent LoadLearningContent(string name, Stream stream)
    {
        return DataAccess.LoadLearningContent(name, stream);
    }

    public IEnumerable<SavedLearningWorldPath> GetSavedLearningWorldPaths()
    {
        return DataAccess.GetSavedLearningWorldPaths();
    }

    public void AddSavedLearningWorldPath(SavedLearningWorldPath savedLearningWorldPath)
    {
        DataAccess.AddSavedLearningWorldPath(savedLearningWorldPath);
    }

    public SavedLearningWorldPath AddSavedLearningWorldPathByPathOnly(string path)
    {
        return DataAccess.AddSavedLearningWorldPathByPathOnly(path);
    }

    public void UpdateIdOfSavedLearningWorldPath(SavedLearningWorldPath savedLearningWorldPath, Guid id)
    {
        DataAccess.UpdateIdOfSavedLearningWorldPath(savedLearningWorldPath, id);
    }

    public void RemoveSavedLearningWorldPath(SavedLearningWorldPath savedLearningWorldPath)
    {
        DataAccess.RemoveSavedLearningWorldPath(savedLearningWorldPath);
    }

    public LearningWorld LoadLearningWorld(Stream stream)
    {
        return DataAccess.LoadLearningWorld(stream);
    }

    public LearningSpace LoadLearningSpace(Stream stream)
    {
        return DataAccess.LoadLearningSpace(stream);
    }

    public LearningElement LoadLearningElement(Stream stream)
    {
        return DataAccess.LoadLearningElement(stream);
    }

    public string FindSuitableNewSavePath(string targetFolder, string fileName, string fileEnding)
    {
        return DataAccess.FindSuitableNewSavePath(targetFolder, fileName, fileEnding);
    }

    public string GetContentFilesFolderPath()
    {
        return DataAccess.GetContentFilesFolderPath();
    }

    public async Task CallExport()
    {
        // First get the User Token
        var usertoken = await BackendAccess.GetUserTokenAsync("user", "bitnami");

        // Then we can get User Information by using the token
        var userInformation = await BackendAccess.GetUserInformationAsync(usertoken.Token);

        // Then we can use the user information to call the export
        Console.WriteLine(userInformation);
    }
}