using BusinessLogic.Commands;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using BusinessLogic.Entities.LearningContent.LinkContent;
using BusinessLogic.ErrorManagement.DataAccess;
using BusinessLogic.Validation.Validators;
using FluentValidation.Results;
using Microsoft.Extensions.Localization;
using Shared;
using Shared.Command;
using Shared.Configuration;

namespace BusinessLogic.API;

public interface IBusinessLogic
{
    IApplicationConfiguration Configuration { get; }
    bool CanUndo { get; }
    bool CanRedo { get; }

    /// <summary>
    /// Executes a given command.
    /// </summary>
    /// <param name="command">Command to be executed.</param>
    void ExecuteCommand(ICommand command);

    /// <summary>
    /// Calls the method to undo the last executed command.
    /// </summary>
    void UndoCommand();

    /// <summary>
    /// Calls the method to redo the last undone command.
    /// </summary>
    void RedoCommand();

    /// <summary>
    /// Constructs a backup for the provided LearningWorld at a given filepath.
    /// </summary>
    /// <param name="learningWorld">The LearningWorld instance for which to create a backup.</param>
    /// <param name="filepath">The file path where the backup will be stored.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when parameter values are outside the acceptable range.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the method is called in an invalid state.</exception>
    /// <exception cref="FileNotFoundException">Thrown when the specified file cannot be found.</exception>
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
    Task<ILearningContent> LoadLearningContentAsync(string filepath);

    /// <summary>
    /// Writes the content of the given stream into an application data folder and returns a <see cref="ILearningContentPe"/> object referencing it.
    /// </summary>
    /// <param name="name">The name of the file which is contained in the stream.</param>
    /// <param name="stream">The stream to be written.</param>
    /// <returns>A content object referencing the file.</returns>
    /// <exception cref="HashExistsException">There is already a file with the same hash inside the content folder.</exception>
    Task<ILearningContent> LoadLearningContentAsync(string name, Stream stream);

    /// <summary>
    ///     Gets all content files in the appdata folder.
    /// </summary>
    /// <returns>An enumerable of content files.</returns>
    IEnumerable<ILearningContent> GetAllContent();

    /// <summary>
    ///     Deletes the file referenced by the given content object.
    /// </summary>
    /// <param name="content">The content whos file shall be deleted.</param>
    /// <exception cref="FileNotFoundException">The file corresponding to <paramref name="content" /> wasn't found.</exception>
    void RemoveContent(ILearningContent content);

    /// <summary>
    ///     Adds the given <see cref="LinkContent" /> to the link file.
    /// </summary>
    /// <param name="linkContent">The link to add.</param>
    void SaveLink(LinkContent linkContent);

    IEnumerable<SavedLearningWorldPath> GetSavedLearningWorldPaths();
    void AddSavedLearningWorldPath(SavedLearningWorldPath savedLearningWorldPath);
    SavedLearningWorldPath AddSavedLearningWorldPathByPathOnly(string path);
    void UpdateIdOfSavedLearningWorldPath(SavedLearningWorldPath savedLearningWorldPath, Guid id);
    void RemoveSavedLearningWorldPath(SavedLearningWorldPath savedLearningWorldPath);

    /// <inheritdoc cref="IDataAccess.FindSuitableNewSavePath" />
    string FindSuitableNewSavePath(string targetFolder, string fileName, string fileEnding);

    event EventHandler<CommandUndoRedoOrExecuteArgs> OnCommandUndoRedoOrExecute;
    string GetContentFilesFolderPath();

    #region BackendAccess

    Task<bool> IsLmsConnected();
    string LoginName { get; }
    Task Login(string username, string password);
    void Logout();

    Task UploadLearningWorldToBackendAsync(string filepath, IProgress<int>? progress = null,
        CancellationToken? cancellationToken = null);

    #endregion

    ValidationResult ValidateLearningWorldForExport(ILearningWorld entityWorld, IStringLocalizer<LearningWorldExportValidator> validator);
}