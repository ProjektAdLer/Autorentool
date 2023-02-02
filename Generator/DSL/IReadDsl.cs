namespace Generator.DSL;

public interface IReadDsl
{
    void ReadWorld(string dslPath, DocumentRootJson? rootJsonForTest = null);
    List<ElementJson> GetH5PElementsList();
    WorldJson GetWorld();
    List<SpaceJson> GetSectionList();
    List<ElementJson> GetResourceList();
    List<ElementJson> GetLabelsList();
    List<ElementJson> GetUrlList();
    List<ElementJson> GetSpacesAndElementsOrderedList();
}