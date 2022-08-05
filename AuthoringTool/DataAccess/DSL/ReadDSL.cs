using System.IO.Abstractions;
using System.Text.Json;

namespace AuthoringTool.DataAccess.DSL;

public class ReadDSL : IReadDSL
{
    public List<LearningElementJson>? ListH5PElements;
    public List<LearningSpaceJson>? ListLearningSpaces;
    public List<LearningElementJson>? ListDslDocument;
    public LearningWorldJson? LearningWorldJson;
    private string? filepathDSL;
    private readonly IFileSystem _fileSystem;

    public ReadDSL(IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
    }

    public void ReadLearningWorld(string dslPath)
    {
        filepathDSL = dslPath;
        
        ListH5PElements = new List<LearningElementJson>();
        ListLearningSpaces = new List<LearningSpaceJson>();
        ListDslDocument = new List<LearningElementJson>();
        string jsonString = _fileSystem.File.ReadAllText(filepathDSL);
        DocumentRootJson? rootJson = JsonSerializer.Deserialize<DocumentRootJson>(jsonString);
        GetH5PElements(rootJson);
        GetLearningSpaces(rootJson);
        GetDslDocument(rootJson);
        SetLearningWorld(rootJson);
    }

    private void SetLearningWorld(DocumentRootJson? documentRootJson)
    {
        if (documentRootJson != null) LearningWorldJson = documentRootJson.learningWorld;
    }

    public LearningWorldJson? GetLearningWorld()
    {
        return LearningWorldJson;
    }
    
    private void GetH5PElements(DocumentRootJson? documentRootJson)
    {
        if (documentRootJson != null && documentRootJson.learningWorld != null)
            if (documentRootJson.learningWorld.learningElements != null)
                foreach (var element in documentRootJson.learningWorld.learningElements)
                {
                    if (element.elementType == "h5p")
                    {
                        if (ListH5PElements != null) ListH5PElements.Add(element);
                    }
                }
    }

    private void GetLearningSpaces(DocumentRootJson? documentRootJson)
    {
        if (documentRootJson != null && documentRootJson.learningWorld != null)
            if (documentRootJson.learningWorld.learningSpaces != null)
                foreach (var space in documentRootJson.learningWorld.learningSpaces)
                {
                    ListLearningSpaces.Add(space);
                }
    }

    private void GetDslDocument(DocumentRootJson? documentRootJson)
    {
        if (documentRootJson != null && documentRootJson.learningWorld != null)
            if (documentRootJson.learningWorld.learningElements != null)
                foreach (var element in documentRootJson.learningWorld.learningElements)
                {
                    if (element.identifier?.value == "DSL Dokument")
                    {
                        if (ListDslDocument != null) ListDslDocument.Add(element);
                    }
                }
    }

    public List<LearningElementJson>? GetH5PElementsList()
    {
        return ListH5PElements;
    }

    public List<LearningSpaceJson>? GetLearningSpaceList()
    {
        return ListLearningSpaces;
    }

    public List<LearningElementJson>? GetDslDocumentList()
    {
        return ListDslDocument;
    }
    
    
}