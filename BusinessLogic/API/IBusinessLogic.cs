using BusinessLogic.Commands;
using BusinessLogic.Entities;
using Shared.Configuration;

namespace BusinessLogic.API;

public interface IBusinessLogic
{
    IAuthoringToolConfiguration Configuration { get; }
    
    /// <summary>
    /// Executes a given command.
    /// </summary>
    /// <param name="command">Command to be executed.</param>
    void ExecuteCommand(ICommand command);
    bool CanUndo { get; }
    bool CanRedo { get; }
    void UndoCommand();
    void RedoCommand();
    void ConstructBackup(World world, string filepath);
    void SaveWorld(World world, string filepath);
    World LoadWorld(string filepath);
    World LoadWorld(Stream stream);
    void SaveSpace(Space space, string filepath);
    Space LoadSpace(string filepath);
    Space LoadSpace(Stream stream);
    void SaveElement(Element element, string filepath);
    Element LoadElement(string filepath);
    Element LoadElement(Stream stream);
    Content LoadContent(string filepath);
    Content LoadContent(string name, MemoryStream stream);
    
    /// <inheritdoc cref="IDataAccess.FindSuitableNewSavePath"/>
    string FindSuitableNewSavePath(string targetFolder, string fileName, string fileEnding);

    event Action? OnUndoRedoPerformed;
}