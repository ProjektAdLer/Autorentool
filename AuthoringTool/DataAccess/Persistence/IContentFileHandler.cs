using AuthoringTool.DataAccess.PersistEntities;

namespace AuthoringTool.DataAccess.Persistence;

public interface IContentFileHandler
{
    LearningContentPe LoadFromDisk (string filepath);
    LearningContentPe LoadFromStream(string name, Stream stream);
}