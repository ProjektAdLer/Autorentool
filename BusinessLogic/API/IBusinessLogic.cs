using BusinessLogic.Entities;
using Shared.Configuration;

namespace BusinessLogic.API;

public interface IBusinessLogic
{
    IAuthoringToolConfiguration Configuration { get; }
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
    LearningContent LoadLearningContent(string name, Stream stream);
    
    /// <inheritdoc cref="IDataAccess.FindSuitableNewSavePath"/>
    string FindSuitableNewSavePath(string targetFolder, string fileName, string fileEnding);
}