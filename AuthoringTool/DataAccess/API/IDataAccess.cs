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
    
    /// <summary>
    /// Finds a save path in <paramref name="targetFolder"/> containing <paramref name="fileName"/> and ending with <paramref name="fileEnding"/>,
    /// that does not yet exist.
    /// </summary>
    /// <param name="targetFolder">The parent folder which shall contain the file.</param>
    /// <param name="fileName">The name of the file.</param>
    /// <param name="fileEnding">The ending of the file.</param>
    /// <exception cref="ArgumentException"><paramref name="targetFolder"/>, <paramref name="fileName"/>
    /// or <paramref name="fileEnding"/> is null, whitespace or empty.</exception>
    /// <returns>A save path of form <code>[targetFolder]/[fileName]_n.[fileEnding]</code> that does not yet exist,
    /// where n is an integer which is incremented until the path does not yet exist.</returns>
    string FindSuitableNewSavePath(string targetFolder, string fileName, string fileEnding);
}