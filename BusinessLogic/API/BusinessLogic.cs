using BusinessLogic.Commands;
using BusinessLogic.Entities;
using Shared.Configuration;

namespace BusinessLogic.API;

public class BusinessLogic : IBusinessLogic
{
    public BusinessLogic(
        IAuthoringToolConfiguration configuration,
        IDataAccess dataAccess,
        IWorldGenerator worldGenerator,
        ICommandStateManager commandStateManager)
    {
        Configuration = configuration;
        DataAccess = dataAccess;
        WorldGenerator = worldGenerator;
        CommandStateManager = commandStateManager;
    }
    
    
    internal IWorldGenerator WorldGenerator { get; }
    internal ICommandStateManager CommandStateManager { get; }
    internal IDataAccess DataAccess { get;  }
    public event Action? OnUndoRedoPerformed;

    public void ExecuteCommand(ICommand command)
    {
        CommandStateManager.Execute(command);
        OnUndoRedoPerformed?.Invoke();
    }
    public bool CanUndo => CommandStateManager.CanUndo;
    public bool CanRedo => CommandStateManager.CanRedo;
    
    public void UndoCommand()
    {
        CommandStateManager.Undo();
        OnUndoRedoPerformed?.Invoke();
    }
    
    public void RedoCommand()
    {
        CommandStateManager.Redo();
        OnUndoRedoPerformed?.Invoke();
    }

    public void ConstructBackup(World world, string filepath)
    {
        WorldGenerator.ConstructBackup(world, filepath);
    }

    public void SaveWorld(World world, string filepath)
    {
        DataAccess.SaveWorldToFile(world, filepath);
    }

    public World LoadWorld(string filepath)
    {
        return DataAccess.LoadWorld(filepath);
    }
    
    public void SaveSpace(Space space, string filepath)
    {
        DataAccess.SaveSpaceToFile(space, filepath);
    }

    public Space LoadSpace(string filepath)
    {
        return DataAccess.LoadSpace(filepath);
    }
    
    public void SaveElement(Element element, string filepath)
    {
        DataAccess.SaveElementToFile(element, filepath);
    }

    public Element LoadElement(string filepath)
    {
        return DataAccess.LoadElement(filepath);
    }

    public Content LoadContent(string filepath)
    {
        return DataAccess.LoadContent(filepath);
    }

    public Content LoadContent(string name, MemoryStream stream)
    {
        return DataAccess.LoadContent(name, stream);
    }

    public World LoadWorld(Stream stream)
    {
        return DataAccess.LoadWorld(stream);
    }

    public Space LoadSpace(Stream stream)
    {
        return DataAccess.LoadSpace(stream);
    }

    public Element LoadElement(Stream stream)
    {
        return DataAccess.LoadElement(stream);
    }

    public string FindSuitableNewSavePath(string targetFolder, string fileName, string fileEnding)
    {
        return DataAccess.FindSuitableNewSavePath(targetFolder, fileName, fileEnding);
    }
    public IAuthoringToolConfiguration Configuration { get; }
  
}