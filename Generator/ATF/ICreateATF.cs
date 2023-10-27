using PersistEntities;

namespace Generator.ATF;

public interface ICreateAtf
{
    /// <summary>
    /// Reads the LearningWorld Entity and creates an ATF Document with the given information.
    /// </summary>
    /// <param name="learningWorld">The learning world to be written to the ATF document</param>
    /// <exception cref="ArgumentOutOfRangeException">The world contains an element whos content type is not supported.</exception>
    string GenerateAndExportLearningWorldJson(LearningWorldPe learningWorld);
}