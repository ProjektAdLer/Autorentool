using System.IO.Abstractions;
using System.Text.Json;


namespace Generator.DSL;

public class ReadDsl : IReadDsl
{
    public List<LearningElementJson> ListH5PElements;
    public List<LearningElementJson> ListResourceElements;
    public List<LearningElementJson> ListLabelElements;
    public List<LearningElementJson> ListUrlElements;
    public List<LearningElementJson> ListAllSpacesAndElementsOrdered;
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
        _learningWorldJson = new LearningWorldJson("Uuid",new IdentifierJson("LearningWorld", "Value"), 
            new List<int>(), new List<TopicJson>(), 
            new List<LearningSpaceJson>(), new List<LearningElementJson>());
        _rootJson = new DocumentRootJson(_learningWorldJson);
        ListH5PElements = new List<LearningElementJson>();
        ListResourceElements = new List<LearningElementJson>();
        ListLabelElements = new List<LearningElementJson>();
        ListUrlElements = new List<LearningElementJson>();
        ListAllSpacesAndElementsOrdered = new List<LearningElementJson>();
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
        GetSpacesAndElementsOrdered(_rootJson);
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
            if (resource.ElementType is "pdf" or "json" or "jpg" or "png" or "webp" or "bmp" or "txt" or "c"
                or "h" or "cpp" or "cc" or "c++" or "py" or "cs" or "js" or "php" or "html" or "css")
            {
                ListResourceElements.Add(resource);
            }
        }
    }
    
    private void GetLabelElements(DocumentRootJson documentRootJson)
    {
        foreach (var label in documentRootJson.LearningWorld.LearningElements)
        {
            if (label.ElementType is "label")
            {
                ListLabelElements.Add(label);
            }
        }
    }

    private void GetWorldAttributes(DocumentRootJson documentRootJson)
    {
        // World Attributes like Description & Goals are added to the label-list, as they are represented as Labels in Moodle
        if(documentRootJson.LearningWorld.Description == "" && documentRootJson.LearningWorld.Goals == "") return;
        
        int lastId;
        
        lastId = documentRootJson.LearningWorld.LearningElements.Count+1;

        var worldAttributes = new LearningElementJson(lastId, 
            new IdentifierJson("Description",documentRootJson.LearningWorld.Description), "", 
            "World Attributes", "label", 1, 
            new List<LearningElementValueJson>(), documentRootJson.LearningWorld.Description,
            documentRootJson.LearningWorld.Goals);
        
        ListAllSpacesAndElementsOrdered.Add(worldAttributes);
    }

    private void GetUrlElements(DocumentRootJson documentRootJson)
    {
        foreach (var url in documentRootJson.LearningWorld.LearningElements)
        {
            if (url.ElementType is "url")
            {
                ListUrlElements.Add(url);
            }
        }
    }

    private void GetSpacesAndElementsOrdered(DocumentRootJson? documentRootJson)
    {
        if (documentRootJson != null)
        {
            foreach (var space in documentRootJson.LearningWorld.LearningSpaces)
            {
                List<LearningElementValueJson> values = new List<LearningElementValueJson>{new("", "0")};
                ListAllSpacesAndElementsOrdered.Add(new LearningElementJson(space.SpaceId+1000, space.Identifier, "", "space","space", 0, values, space.Description));
                
                foreach (int elementInSpace in space.LearningSpaceContent)
                {
                    ListAllSpacesAndElementsOrdered.Add(documentRootJson.LearningWorld.LearningElements[elementInSpace-1]);
                }
            }
        }
    }

    public List<LearningElementJson> GetH5PElementsList()
    {
        return ListH5PElements;
    }

    //because sections are not supported in the authoringTool yet, a dummy section is created
    //right now this dummy is just a space, but it will be a section in the future
    public List<LearningSpaceJson> GetSectionList()
    {
        var space = new LearningSpaceJson(0, new IdentifierJson("identifier", "Topic 0"), 
            new List<int>(), 0 ,0 );
        var spaceList = new List<LearningSpaceJson> {space};
        return spaceList;
    }

    public List<LearningElementJson> GetResourceList()
    {
        return ListResourceElements;
    }
    
    public List<LearningElementJson> GetLabelsList()
    {
        return ListLabelElements;
    }
    
    public List<LearningElementJson> GetUrlList()
    {
        return ListUrlElements;
    }
    
    //A List that contains all Spaces and Elements in the correct order. 
    //First comes a Spaces followed by all his Elements until another Space appears.
    //The Spaces where transformed to LearningElementJson, so they can be used in the same List.
    public List<LearningElementJson> GetSpacesAndElementsOrderedList()
    {
        return ListAllSpacesAndElementsOrdered;
    }
    
}