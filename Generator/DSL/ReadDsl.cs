using System.IO.Abstractions;
using System.Text.Json;


namespace Generator.DSL;

public class ReadDsl : IReadDsl
{
    public List<LearningElementJson> ListH5PElements;
    public List<LearningElementJson> ListResourceElements;
    private List<LearningSpaceJson> _listLearningSpaces;
    private LearningWorldJson _learningWorldJson;
    private IFileSystem _fileSystem;
    private DocumentRootJson _rootJson;
    

#pragma warning disable CS8618 @Dimitri_Bigler Lists are always initiated, Constructor just doesnt know.
    public ReadDsl(IFileSystem fileSystem)
#pragma warning restore CS8618
    {
        Initialize();
        _fileSystem = fileSystem;
    }

    private void Initialize()
    {
        _learningWorldJson = new LearningWorldJson("Uuid",new IdentifierJson("LearningWorld", "Value"), 
            new List<int>(), new List<TopicJson>(), 
            new List<LearningSpaceJson>(), new List<LearningElementJson>());
        _rootJson = new DocumentRootJson(_learningWorldJson);
        ListH5PElements = new List<LearningElementJson>();
        ListResourceElements = new List<LearningElementJson>();
        _listLearningSpaces = new List<LearningSpaceJson>();
    }

    public void ReadLearningWorld(string dslPath, DocumentRootJson? rootJsonForTest = null)
    {
        Initialize();
        
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
        GetResourceElements(_rootJson);
        GetLearningSpaces(_rootJson);
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
    
    private void GetH5PElements(DocumentRootJson documentRootJson)
    {
        foreach (var element in documentRootJson.LearningWorld.LearningElements)
        {
            if (element.ElementType == "h5p")
            {
                ListH5PElements.Add(element);
            }
        }
    }

    private void GetResourceElements(DocumentRootJson documentRootJson)
    {
        foreach (var resource in documentRootJson.LearningWorld.LearningElements)
        {
            if (resource.ElementType is "pdf" or "json" or "jpg" or "png" or "webp" or "bmp" or "mp4")
            {
                ListResourceElements.Add(resource);
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

    public List<LearningElementJson> GetH5PElementsList()
    {
        return ListH5PElements;
    }

    public List<LearningSpaceJson> GetLearningSpaceList()
    {
        return _listLearningSpaces;
    }

    public List<LearningElementJson> GetResourceList()
    {
        return ListResourceElements;
    }
    
    
}