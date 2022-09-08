namespace Generator.DSL;

public interface IReadDsl
{
    void ReadLearningWorld(string dslPath, DocumentRootJson? rootJsonForTest = null);
    List<LearningElementJson> GetH5PElementsList();
    LearningWorldJson GetLearningWorld();

    List<LearningSpaceJson> GetLearningSpaceList();
    List<LearningElementJson> GetResourceList();
}