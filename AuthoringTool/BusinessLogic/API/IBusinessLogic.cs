using AuthoringTool.API.Configuration;
using AuthoringTool.DataAccess.API;
using AuthoringTool.Entities;

namespace AuthoringTool.BusinessLogic.API;

public interface IBusinessLogic
{
    IAuthoringToolConfiguration Configuration { get; }
    IDataAccess DataAccess { get; }
    bool RunningElectron { get; }
    void ConstructBackup(LearningWorld learningWorld, string filepath);
    void SaveLearningWorld(LearningWorld learningWorld, string filepath);
    LearningWorld LoadLearningWorld(string filepath);
    LearningWorld LoadLearningWorld(Stream stream);
    void SaveLearningSpace(LearningSpace learningSpace, string filepath);
    LearningSpace LoadLearningSpace(string filepath);
    LearningSpace LoadLearningSpace(Stream stream);
    void SaveLearningElement(LearningElement learningElement, string filepath);
    LearningElement LoadLearningElement(string filepath);
    LearningElement LoadLearningElement(Stream stream);
    LearningContent LoadLearningContent(string filepath);
    
    /// <inheritdoc cref="IDataAccess.FindSuitableNewSavePath"/>
    string FindSuitableNewSavePath(string targetFolder, string fileName, string fileEnding);
}