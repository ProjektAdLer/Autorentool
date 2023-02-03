namespace Generator.DSL;

public interface IReadDsl
{
    void ReadLearningWorld(string dslPath, DocumentRootJson? rootJsonForTest = null);
    List<LearningElementJson> GetH5PElementsList();
    LearningWorldJson GetLearningWorld();
    List<LearningSpaceJson> GetSectionList();
    List<LearningElementJson> GetResourceList();
    List<LearningElementJson> GetLabelsList();
    List<LearningElementJson> GetUrlList();
    List<LearningElementJson> GetSpacesAndElementsOrderedList();
}