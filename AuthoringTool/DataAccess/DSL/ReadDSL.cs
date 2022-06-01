using System.Text.Json;

namespace AuthoringTool.DataAccess.DSL;

public class ReadDSL : IReadDSL
{
    private List<LearningElementJson>? _listH5PElements;
    private LearningWorldJson? _learningWorldJson;
    private string filepathDSL = "XMLFilesForExport/DSL_Document.json";

    public void ReadLearningWorld()
    {
        _listH5PElements = new List<LearningElementJson>();
        string jsonString = File.ReadAllText(filepathDSL);
        DocumentRootJson? rootJson = JsonSerializer.Deserialize<DocumentRootJson>(jsonString);
        GetH5PElements(rootJson);
        SetLearningWorld(rootJson);
    }

    private void SetLearningWorld(DocumentRootJson? documentRootJson)
    {
        if (documentRootJson != null) _learningWorldJson = documentRootJson.learningWorld;
    }

    public LearningWorldJson? GetLearningWorld()
    {
        return _learningWorldJson;
    }
    
    private void GetH5PElements(DocumentRootJson? documentRootJson)
    {
        if (documentRootJson != null && documentRootJson.learningWorld != null)
            if (documentRootJson.learningWorld.learningElements != null)
                foreach (var element in documentRootJson.learningWorld.learningElements)
                {
                    if (element.elementType == "H5P")
                    {
                        if (_listH5PElements != null) _listH5PElements.Add(element);
                    }
                }
    }

    public List<LearningElementJson>? GetH5PElementsList()
    {
        return _listH5PElements;
    }
    
    
}