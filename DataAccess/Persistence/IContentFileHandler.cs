using PersistEntities;

namespace DataAccess.Persistence;

public interface IContentFileHandler
{
    /// <summary>
    /// Loads the file at the given filepath into an application data folder and returns a <see cref="ContentPe"/> object referencing it.
    /// </summary>
    /// <param name="filepath">The path to the content file.</param>
    /// <returns>A content object referencing the file.</returns>
    /// <exception cref="ArgumentException">The <paramref name="filepath"/> was null or whitespace.</exception>
    /// <exception cref="IOException">The file at <paramref name="filepath"/> has a length of 0 and is empty.</exception>
    /// <remarks>If a file of identical length and SHA256 hash already exists in the appdata folder,
    /// we don't copy the file and use the existing one instead.</remarks>
    public Task<ContentPe> LoadContentAsync(string filepath);
    /// <summary>
    /// Writes the content of the given stream into an application data folder and returns a <see cref="ContentPe"/> object referencing it.
    /// </summary>
    /// <param name="name">The name of the file which is contained in the stream.</param>
    /// <param name="stream">The stream to be written.</param>
    /// <returns>A content object referencing the file.</returns>
    /// <exception cref="IOException">The stream has a length of 0 and is empty.</exception>
    /// <remarks>If a file of identical length and SHA256 hash already exists in the appdata folder,
    /// we don't copy the stream and use the existing one instead.</remarks>
    public Task<ContentPe> LoadContentAsync(string name, MemoryStream stream);
}