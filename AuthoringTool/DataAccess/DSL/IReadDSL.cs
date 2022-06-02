using System.IO.Abstractions;

namespace AuthoringTool.DataAccess.DSL;

public interface IReadDSL
{
    void ReadLearningWorld(string dslPath, IFileSystem? fileSystem = null);
    List<LearningElementJson>? GetH5PElementsList();
    LearningWorldJson? GetLearningWorld();
}