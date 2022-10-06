using PersistEntities;

namespace DataAccess.Persistence;

public interface IContentFileHandler
{
    public Task<LearningContentPe> LoadContentAsync(string filepath);
    public Task<LearningContentPe> LoadContentAsync(string name, Stream stream);
}