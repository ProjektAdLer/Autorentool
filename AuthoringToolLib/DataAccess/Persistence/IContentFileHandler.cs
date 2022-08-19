using Generator.PersistEntities;

namespace AuthoringToolLib.DataAccess.Persistence;

public interface IContentFileHandler
{
    LearningContentPe LoadFromDisk (string filepath);
    LearningContentPe LoadFromStream(string name, Stream stream);
}