using System.IO.Abstractions;
using System.Text.Json;

namespace AuthoringTool.DataAccess.DSL;

public class ReadDsl : IReadDsl
{
    public readonly List<LearningElementJson> ListH5PElements;
    private readonly List<LearningSpaceJson> _listLearningSpaces;
    private readonly List<LearningElementJson> _listDslDocument;
    private LearningWorldJson _learningWorldJson;
    private readonly IFileSystem _fileSystem;
    private DocumentRootJson _rootJson;

    public ReadDsl(IFileSystem fileSystem)
    {
        _learningWorldJson = new LearningWorldJson("Uuid",new IdentifierJson("LearningWorld", "Value"), 
            new List<int>(), new List<TopicJson>(), 
            new List<LearningSpaceJson>(), new List<LearningElementJson>());
        _rootJson = new DocumentRootJson(_learningWorldJson);
        _fileSystem = fileSystem;
        ListH5PElements = new List<LearningElementJson>();
        _listLearningSpaces = new List<LearningSpaceJson>();
        _listDslDocument = new List<LearningElementJson>();
    }

    public void ReadLearningWorld(string dslPath, DocumentRootJson? rootJsonForTest = null)
    {
        var filepathDsl = dslPath;
        
        if (rootJsonForTest != null)
        {
            _rootJson = rootJsonForTest;
        }
        else if (rootJsonForTest == null)
        {
             var jsonString = _fileSystem.File.ReadAllText(filepathDsl);
             _rootJson = JsonSerializer.Deserialize<DocumentRootJson>(jsonString) ?? throw new InvalidOperationException("Could not deserialize DSL_Document");
        }
        GetH5PElements(_rootJson);
        GetLearningSpaces(_rootJson);
        GetDslDocument(_rootJson);
        SetLearningWorld(_rootJson);
    }

    private void SetLearningWorld(DocumentRootJson? documentRootJson)
    {
        if (documentRootJson != null) _learningWorldJson = documentRootJson.LearningWorld;
    }

    public LearningWorldJson GetLearningWorld()
    {
        return _learningWorldJson;
    }
    
    private void GetH5PElements(DocumentRootJson? documentRootJson)
    {
        if (documentRootJson != null)
            foreach (var element in documentRootJson.LearningWorld.LearningElements)
            {
                if (element.ElementType == "h5p")
                {
                    if (ListH5PElements != null) ListH5PElements.Add(element);
                }
            }
    }

    private void GetLearningSpaces(DocumentRootJson? documentRootJson)
    {
        if (documentRootJson != null)
            foreach (var space in documentRootJson.LearningWorld.LearningSpaces)
            {
                _listLearningSpaces.Add(space);
            }
    }

    private void GetDslDocument(DocumentRootJson? documentRootJson)
    {
        if (documentRootJson != null)
            foreach (var element in documentRootJson.LearningWorld.LearningElements)
            {
                if (element.Identifier.Value == "DSL Dokument")
                {
                    if (_listDslDocument != null) _listDslDocument.Add(element);
                }
            }
    }

    public List<LearningElementJson> GetH5PElementsList()
    {
        return ListH5PElements;
    }

    public List<LearningSpaceJson> GetLearningSpaceList()
    {
        return _listLearningSpaces;
    }

    public List<LearningElementJson> GetDslDocumentList()
    {
        return _listDslDocument;
    }
    
    
}