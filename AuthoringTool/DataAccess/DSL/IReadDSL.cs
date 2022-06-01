namespace AuthoringTool.DataAccess.DSL;

public interface IReadDSL
{
    void ReadLearningWorld();
    List<LearningElementJson>? GetH5PElementsList();
    LearningWorldJson? GetLearningWorld();
}