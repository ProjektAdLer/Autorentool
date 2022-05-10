using AuthoringTool.Entities;

namespace AuthoringTool.DataAccess.Persistence;

public interface IContentFileHandler
{
    LearningContent LoadFromDisk (string filepath);
}