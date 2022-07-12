using System.IO.Abstractions;

namespace AuthoringTool.DataAccess.DSL;

public interface IReadDSL
{
    void ReadLearningWorld(string dslPath);
    List<LearningElementJson>? GetH5PElementsList();
    LearningWorldJson? GetLearningWorld();
    
    List<LearningElementJson>? GetDslDocumentList();

    List<LearningSpaceJson>? GetLearningSpaceList();
}