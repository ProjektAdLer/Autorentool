using BusinessLogic.Commands;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
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
    void ConstructBackup(LearningWorld learningWorld, string filepath);
    void SaveLearningWorld(LearningWorld learningWorld, string filepath);
    LearningWorld LoadLearningWorld(string filepath);
    LearningWorld LoadLearningWorld(Stream stream);
    void SaveLearningSpace(LearningSpace learningSpace, string filepath);
    LearningSpace LoadLearningSpace(string filepath);
    LearningSpace LoadLearningSpace(Stream stream);
    void SaveLearningElement(LearningElement learningElement, string filepath);
    LearningElement LoadLearningElement(string filepath);
    LearningElement LoadLearningElement(Stream stream);
    LearningContent LoadLearningContent(string filepath);
    LearningContent LoadLearningContent(string name, Stream stream);
    /// <summary>
    /// Gets all content files in the appdata folder.
    /// </summary>
    /// <returns>An enumerable of content files.</returns>
    IEnumerable<LearningContent> GetAllContent();

    /// <summary>
    /// Deletes the file referenced by the given content object.
    /// </summary>
    /// <param name="content">The content whos file shall be deleted.</param>
    /// <exception cref="FileNotFoundException">The file corresponding to <paramref name="content"/> wasn't found.</exception>
    void RemoveContent(LearningContent content);
    
    /// <summary>
    /// Adds the given <see cref="LinkContent"/> to the link file.
    /// </summary>
    /// <param name="linkContent">The link to add.</param>
    void SaveLink(LinkContent linkContent);
    
    /// <inheritdoc cref="IDataAccess.FindSuitableNewSavePath"/>
    string FindSuitableNewSavePath(string targetFolder, string fileName, string fileEnding);

    event Action? OnUndoRedoPerformed;
}