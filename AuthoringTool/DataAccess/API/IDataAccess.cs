using AuthoringTool.API.Configuration;
using AuthoringTool.Entities;

namespace AuthoringTool.DataAccess.API;

public interface IDataAccess
{
    IAuthoringToolConfiguration Configuration { get; }

    void ConstructBackup();
    void SaveLearningWorldToFile(LearningWorld world, string filepath);
    LearningWorld LoadLearningWorldFromFile(string filepath);
    void SaveLearningSpaceToFile(LearningSpace space, string filepath);
    LearningSpace LoadLearningSpaceFromFile(string filepath);
    void SaveLearningElementToFile(LearningElement element, string filepath);
    LearningElement LoadLearningElementFromFile(string filepath);
    LearningContent LoadLearningContentFromFile(string filepath);
}