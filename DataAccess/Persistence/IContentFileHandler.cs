using BusinessLogic.ErrorManagement.DataAccess;
using PersistEntities.LearningContent;

namespace DataAccess.Persistence;

public interface IContentFileHandler
{
    /// <summary>
    /// The file path to the folder containing all content files.
    /// </summary>
    string ContentFilesFolderPath { get; }

    /// <summary>
    /// Loads the file at the given filepath into an application data folder and returns a <see cref="ILearningContentPe"/> object referencing it.
    /// </summary>
    /// <param name="filepath">The path to the content file.</param>
    /// <returns>A content object referencing the file.</returns>
    /// <exception cref="ArgumentException">The <paramref name="filepath"/> was null or whitespace.</exception>
    /// <exception cref="IOException">The file at <paramref name="filepath"/> has a length of 0 and is empty.</exception>
    /// <exception cref="HashExistsException">There is already a file with the same hash inside the content folder.</exception>
    /// <remarks>If a file of identical length and SHA256 hash already exists in the appdata folder,
    /// we don't copy the file and use the existing one instead.</remarks>
    public Task<ILearningContentPe> LoadContentAsync(string filepath);

    /// <summary>
    /// Writes the content of the given stream into an application data folder and returns a <see cref="ILearningContentPe"/> object referencing it.
    /// </summary>
    /// <param name="name">The name of the file which is contained in the stream.</param>
    /// <param name="stream">The stream to be written.</param>
    /// <returns>A content object referencing the file.</returns>
    /// <exception cref="IOException">The stream has a length of 0 and is empty.</exception>
    /// <exception cref="HashExistsException">There is already a file with the same hash inside the content folder.</exception>
    /// <remarks>If a file of identical length and SHA256 hash already exists in the appdata folder,
    /// we don't copy the stream and use the existing one instead.</remarks>
    public Task<ILearningContentPe> LoadContentAsync(string name, Stream stream);

    /// <summary>
    /// Gets all content files in the appdata folder.
    /// </summary>
    /// <returns>An enumerable of content files.</returns>
    public IEnumerable<ILearningContentPe> GetAllContent();

    /// <summary>
    /// Deletes the file referenced by the given content object.
    /// </summary>
    /// <param name="content">The content whos file shall be deleted.</param>
    /// <exception cref="FileNotFoundException">The file corresponding to <paramref name="content"/> wasn't found.</exception>
    void RemoveContent(ILearningContentPe content);

    /// <summary>
    /// Adds the given <see cref="LinkContentPe"/> to the link file.
    /// </summary>
    /// <param name="linkContent">The link to add.</param>
    void SaveLink(LinkContentPe linkContent);
}