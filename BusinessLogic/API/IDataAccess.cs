using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using BusinessLogic.Entities.LearningContent.LinkContent;
using BusinessLogic.ErrorManagement.DataAccess;
using Shared;
using Shared.Configuration;

namespace BusinessLogic.API;

public interface IDataAccess
{
    IApplicationConfiguration Configuration { get; }

    void SaveLearningWorldToFile(LearningWorld world, string filepath);
    LearningWorld LoadLearningWorld(string filepath);
    LearningWorld LoadLearningWorld(Stream stream);
    void SaveLearningSpaceToFile(LearningSpace space, string filepath);
    LearningSpace LoadLearningSpace(string filepath);
    LearningSpace LoadLearningSpace(Stream stream);
    void SaveLearningElementToFile(LearningElement element, string filepath);
    LearningElement LoadLearningElement(string filepath);
    LearningElement LoadLearningElement(Stream stream);

    /// <summary>
    /// Loads the file at the given filepath into an application data folder and returns a <see cref="ILearningContentPe"/> object referencing it.
    /// </summary>
    /// <param name="filepath">The path to the content file.</param>
    /// <returns>A content object referencing the file.</returns>
    /// <exception cref="ArgumentException">The <paramref name="filepath"/> was null or whitespace.</exception>
    /// <exception cref="IOException">The file at <paramref name="filepath"/> has a length of 0 and is empty.</exception>
    /// <exception cref="HashExistsException">There is already a file with the same hash inside the content folder.</exception>
    Task<ILearningContent> LoadLearningContentAsync(string filepath);

    /// <summary>
    /// Writes the content of the given stream into an application data folder and returns a <see cref="ILearningContentPe"/> object referencing it.
    /// </summary>
    /// <param name="name">The name of the file which is contained in the stream.</param>
    /// <param name="stream">The stream to be written.</param>
    /// <returns>A content object referencing the file.</returns>
    /// <exception cref="IOException">The stream has a length of 0 and is empty.</exception>
    /// <exception cref="HashExistsException">There is already a file with the same hash inside the content folder.</exception>
    Task<ILearningContent> LoadLearningContentAsync(string name, Stream stream);

    /// <summary>
    /// Gets all content files in the appdata folder.
    /// </summary>
    /// <returns>An enumerable of content files.</returns>
    IEnumerable<ILearningContent> GetAllContent();

    IEnumerable<SavedLearningWorldPath> GetSavedLearningWorldPaths();
    void AddSavedLearningWorldPath(SavedLearningWorldPath savedLearningWorldPath);
    SavedLearningWorldPath AddSavedLearningWorldPathByPathOnly(string path);
    void UpdateIdOfSavedLearningWorldPath(SavedLearningWorldPath savedLearningWorldPath, Guid id);
    void RemoveSavedLearningWorldPath(SavedLearningWorldPath savedLearningWorldPath);

    /// <summary>
    /// Finds a save path in <paramref name="targetFolder"/> containing <paramref name="fileName"/> and ending with <paramref name="fileEnding"/>,
    /// that does not yet exist.
    /// </summary>
    /// <param name="targetFolder">The parent folder which shall contain the file.</param>
    /// <param name="fileName">The name of the file.</param>
    /// <param name="fileEnding">The ending of the file.</param>
    /// <exception cref="ArgumentException"><paramref name="targetFolder"/>, <paramref name="fileName"/>
    /// or <paramref name="fileEnding"/> is null, whitespace or empty.</exception>
    /// <returns>A save path of form <code>[targetFolder]/[fileName]_n.[fileEnding]</code> that does not yet exist,
    /// where n is an integer which is incremented until the path does not yet exist.</returns>
    string FindSuitableNewSavePath(string targetFolder, string fileName, string fileEnding);

    /// <summary>
    /// Deletes the file referenced by the given content object.
    /// </summary>
    /// <param name="content">The content whos file shall be deleted.</param>
    /// <exception cref="FileNotFoundException">The file corresponding to <paramref name="content"/> wasn't found.</exception>
    void RemoveContent(ILearningContent content);

    /// <summary>
    /// Adds the given <see cref="LinkContent"/> to the link file.
    /// </summary>
    /// <param name="linkContent">The link to add.</param>
    void SaveLink(LinkContent linkContent);

    string GetContentFilesFolderPath();

    /// <summary>
    /// Exports the given <see cref="LearningWorld"/> and all content it references to a zip archive at the given path.
    /// </summary>
    /// <param name="world">The world to export.</param>
    /// <param name="pathToFile">Filepath to export it to</param>
    void ExportLearningWorldToArchive(LearningWorld world, string pathToFile);
    
    /// <summary>
    /// Imports a <see cref="LearningWorld"/> and all content it references from a zip archive at the given path.
    /// </summary>
    /// <param name="pathToArchive">Filepath to the archive.</param>
    void ImportLearningWorldFromArchive(string pathToArchive);
}