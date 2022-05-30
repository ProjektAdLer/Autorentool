using AuthoringTool.API.Configuration;
using AuthoringTool.Entities;

namespace AuthoringTool.DataAccess.API;

public interface IDataAccess
{
    IAuthoringToolConfiguration Configuration { get; }

    void ConstructBackup(LearningWorld learningWorld, string filepath);
    void SaveLearningWorldToFile(LearningWorld world, string filepath);
    LearningWorld LoadLearningWorldFromFile(string filepath);
    LearningWorld LoadLearningWorldFromStream(Stream stream);
    void SaveLearningSpaceToFile(LearningSpace space, string filepath);
    LearningSpace LoadLearningSpaceFromFile(string filepath);
    LearningSpace LoadLearningSpaceFromStream(Stream stream);
    void SaveLearningElementToFile(LearningElement element, string filepath);
    LearningElement LoadLearningElementFromFile(string filepath);
    LearningElement LoadLearningElementFromStream(Stream stream);
    LearningContent LoadLearningContentFromFile(string filepath);
}