using AuthoringTool.API.Configuration;
using AuthoringTool.DataAccess.API;
using AuthoringTool.Entities;

namespace AuthoringTool.BusinessLogic.API;

public interface IBusinessLogic
{
    IAuthoringToolConfiguration Configuration { get; }
    bool RunningElectron { get; }
    void ConstructBackup(LearningWorld learningWorld, string filepath);
    void SaveLearningWorld(LearningWorld learningWorld, string filepath);
    LearningWorld LoadLearningWorld(string filepath);
    LearningWorld LoadLearningWorldFromStream(Stream stream);
    void SaveLearningSpace(LearningSpace learningSpace, string filepath);
    LearningSpace LoadLearningSpace(string filepath);
    LearningSpace LoadLearningSpaceFromStream(Stream stream);
    void SaveLearningElement(LearningElement learningElement, string filepath);
    LearningElement LoadLearningElement(string filepath);
    LearningElement LoadLearningElementFromStream(Stream stream);
    LearningContent LoadLearningContent(string filepath);
    LearningContent LoadLearningContentFromStream(string name, Stream stream);
    
    /// <inheritdoc cref="IDataAccess.FindSuitableNewSavePath"/>
    string FindSuitableNewSavePath(string targetFolder, string fileName, string fileEnding);
}