using PersistEntities;

namespace Generator.DSL;

public interface ICreateDsl
{
    /// <summary>
    /// Reads the LearningWorld Entity and creates an DSL Document with the given information.
    /// </summary>
    /// <param name="learningWorld">The learning world to be written to the DSL document</param>
    /// <exception cref="ArgumentOutOfRangeException">The world contains an element whos content type is not supported.</exception>
    string GenerateAndExportLearningWorldJson(LearningWorldPe learningWorld);
}