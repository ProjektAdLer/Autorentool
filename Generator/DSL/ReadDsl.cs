using System.IO.Abstractions;
using System.Text.Json;


namespace Generator.DSL;

public class ReadDsl : IReadDsl
{
    private List<LearningElementJson> _listH5PElements;
    private List<LearningElementJson> _listResourceElements;
    private List<LearningElementJson> _listLabelElements;
    private List<LearningElementJson> _listUrlElements;
    private List<LearningElementJson> _listAllElementsOrdered;
    private LearningWorldJson _learningWorldJson;
    private IFileSystem _fileSystem;
    private DocumentRootJson _rootJson;
    

#pragma warning disable CS8618 //@Dimitri_Bigler Lists are always initiated, Constructor just doesnt know.
    public ReadDsl(IFileSystem fileSystem)
#pragma warning restore CS8618
    {
        Initialize();
        _fileSystem = fileSystem;
    }

    private void Initialize()
    {
        _learningWorldJson = new LearningWorldJson(new LmsElementIdentifierJson("idNumber", "Value"),
            "", new List<TopicJson>(), 
            new List<LearningSpaceJson>(), new List<LearningElementJson>());
        _rootJson = new DocumentRootJson("0.3", "0.3.2","","",_learningWorldJson);
        _listH5PElements = new List<LearningElementJson>();
        _listResourceElements = new List<LearningElementJson>();
        _listLabelElements = new List<LearningElementJson>();
        _listUrlElements = new List<LearningElementJson>();
        _listAllElementsOrdered = new List<LearningElementJson>();
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
             var options = new JsonSerializerOptions { WriteIndented = true, PropertyNameCaseInsensitive = true};
             _rootJson = JsonSerializer.Deserialize<DocumentRootJson>(jsonString, options) ?? throw new InvalidOperationException("Could not deserialize DSL_Document");
        }
        GetH5PElements(_rootJson);
        GetResourceElements(_rootJson);
        GetLabelElements(_rootJson);
        GetWorldAttributes(_rootJson);
        GetUrlElements(_rootJson);
        GetElementsOrdered(_rootJson);
        SetLearningWorld(_rootJson);
    }

    private void SetLearningWorld(DocumentRootJson? documentRootJson)
    {
        if (documentRootJson != null) _learningWorldJson = documentRootJson.World;
    }

    public LearningWorldJson GetLearningWorld()
    {
        return _learningWorldJson;
    }
    
    private void GetH5PElements(DocumentRootJson documentRootJson)
    {
        foreach (var element in documentRootJson.World.Elements)
        {
            if (element.ElementFileType == "h5p")
            {
                _listH5PElements.Add(element);
            }
        }
    }

    private void GetResourceElements(DocumentRootJson documentRootJson)
    {
        foreach (var resource in documentRootJson.World.Elements)
        {
            if (resource.ElementFileType is "pdf" or "json" or "jpg" or "png" or "webp" or "bmp" or "txt" or "c"
                or "h" or "cpp" or "cc" or "c++" or "py" or "cs" or "js" or "php" or "html" or "css")
            {
                _listResourceElements.Add(resource);
            }
        }
    }
    
    private void GetLabelElements(DocumentRootJson documentRootJson)
    {
        foreach (var label in documentRootJson.World.Elements)
        {
            if (label.ElementFileType is "label")
            {
                _listLabelElements.Add(label);
            }
        }
    }

    private void GetWorldAttributes(DocumentRootJson documentRootJson)
    {
        // World Attributes like Description & Goals are added to the label-list, as they are represented as Labels in Moodle
        if(documentRootJson.World.WorldDescription == "" && documentRootJson.World.WorldGoals[0] == "") return;

        var lastId = documentRootJson.World.Elements.Count+1;

        var worldAttributes = new LearningElementJson(lastId, 
            new LmsElementIdentifierJson("Description",documentRootJson.World.WorldDescription),"", "", 
            "World Attributes", "label", 0,
            0, documentRootJson.World.WorldDescription,
            documentRootJson.World.WorldGoals);
        
        _listAllElementsOrdered.Add(worldAttributes);
    }

    private void GetUrlElements(DocumentRootJson documentRootJson)
    {
        foreach (var url in documentRootJson.World.Elements)
        {
            if (url.ElementFileType is "url")
            {
                _listUrlElements.Add(url);
            }
        }
    }

    private void GetElementsOrdered(DocumentRootJson? documentRootJson)
    {
        if (documentRootJson != null)
        {
            foreach (var space in documentRootJson.World.Spaces)
            {
                foreach (int elementInSpace in space.SpaceContents)
                {
                    _listAllElementsOrdered.Add(documentRootJson.World.Elements[elementInSpace-1]);
                }
            }
        }
    }

    public List<LearningElementJson> GetH5PElementsList()
    {
        return _listH5PElements;
    }

    public List<LearningSpaceJson> GetSectionList()
    {
        var space = new LearningSpaceJson(0, new LmsElementIdentifierJson("identifier", "Topic 0"), 
            "",new List<int>(), 0 );
        var spaceList = new List<LearningSpaceJson> {space};
        spaceList.AddRange(_rootJson.World.Spaces);
        return spaceList;
    }

    public List<LearningElementJson> GetResourceList()
    {
        return _listResourceElements;
    }
    
    public List<LearningElementJson> GetLabelsList()
    {
        return _listLabelElements;
    }
    
    public List<LearningElementJson> GetUrlList()
    {
        return _listUrlElements;
    }
    
    //A List that contains all Elements in the correct order. 
    public List<LearningElementJson> GetElementsOrderedList()
    {
        return _listAllElementsOrdered;
    }
    
}