using AuthoringTool.API.Configuration;
using AuthoringTool.DataAccess.API;
using AuthoringTool.Entities;

namespace AuthoringTool.BusinessLogic.API;

public interface IBusinessLogic
{
    IAuthoringToolConfiguration Configuration { get; }
    IDataAccess DataAccess { get; }
    bool RunningElectron { get; }
    void ConstructBackup();
    void SaveLearningWorld(LearningWorld learningWorld, string filepath);
    LearningWorld LoadLearningWorld(string filepath);
    void SaveLearningSpace(LearningSpace learningSpace, string filepath);
    LearningSpace LoadLearningSpace(string filepath);
    void SaveLearningElement(LearningElement learningElement, string filepath);
    LearningElement LoadLearningElement(string filepath);
    LearningContent LoadLearningContent(string filepath);
}