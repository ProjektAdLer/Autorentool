using AuthoringTool.API.Configuration;
using AuthoringTool.Entities;

namespace AuthoringTool.DataAccess.API;

public interface IDataAccess
{
    IAuthoringToolConfiguration Configuration { get; }

    void ConstructBackup();
    void SaveLearningWorldToFile(LearningWorld world, string filepath);
    LearningWorld LoadLearningWorldFromFile(string filepath);
}