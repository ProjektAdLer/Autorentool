using AuthoringToolLib.API.Configuration;
using Generator.PersistEntities;

namespace AuthoringToolLib.DataAccess.API;

public interface IDataAccess
{
    IAuthoringToolConfiguration Configuration { get; }
    
    void SaveLearningWorldToFile(LearningWorldPe world, string filepath);
    LearningWorldPe LoadLearningWorldFromFile(string filepath);
    LearningWorldPe LoadLearningWorldFromStream(Stream stream);
    void SaveLearningSpaceToFile(LearningSpacePe space, string filepath);
    LearningSpacePe LoadLearningSpaceFromFile(string filepath);
    LearningSpacePe LoadLearningSpaceFromStream(Stream stream);
    void SaveLearningElementToFile(LearningElementPe element, string filepath);
    LearningElementPe LoadLearningElementFromFile(string filepath);
    LearningElementPe LoadLearningElementFromStream(Stream stream);
    LearningContentPe LoadLearningContentFromFile(string filepath);
    LearningContentPe LoadLearningContentFromStream(string name, Stream stream);
    
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