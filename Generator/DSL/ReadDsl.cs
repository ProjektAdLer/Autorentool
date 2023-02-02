using System.IO.Abstractions;
using System.Text.Json;


namespace Generator.DSL;

public class ReadDsl : IReadDsl
{
    public List<ElementJson> ListH5PElements;
    public List<ElementJson> ListResourceElements;
    public List<ElementJson> ListLabelElements;
    public List<ElementJson> ListUrlElements;
    public List<ElementJson> ListAllSpacesAndElementsOrdered;
    private WorldJson _worldJson;
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
        _worldJson = new WorldJson("Uuid",new IdentifierJson("World", "Value"), 
            new List<int>(), new List<TopicJson>(), 
            new List<SpaceJson>(), new List<ElementJson>());
        _rootJson = new DocumentRootJson(_worldJson);
        ListH5PElements = new List<ElementJson>();
        ListResourceElements = new List<ElementJson>();
        ListLabelElements = new List<ElementJson>();
        ListUrlElements = new List<ElementJson>();
        ListAllSpacesAndElementsOrdered = new List<ElementJson>();
    }

    public void ReadWorld(string dslPath, DocumentRootJson? rootJsonForTest = null)
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
        SetWorld(_rootJson);
    }

    private void SetWorld(DocumentRootJson? documentRootJson)
    {
        if (documentRootJson != null) _worldJson = documentRootJson.World;
    }

    public WorldJson GetWorld()
    {
        return _worldJson;
    }
    
    private void GetH5PElements(DocumentRootJson documentRootJson)
    {
        foreach (var element in documentRootJson.World.Elements)
        {
            if (element.ElementType == "h5p")
            {
                ListH5PElements.Add(element);
            }
        }
    }

    private void GetResourceElements(DocumentRootJson documentRootJson)
    {
        foreach (var resource in documentRootJson.World.Elements)
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
        foreach (var label in documentRootJson.World.Elements)
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
        if(documentRootJson.World.Description == "" && documentRootJson.World.Goals == "") return;
        
        int lastId;
        
        lastId = documentRootJson.World.Elements.Count+1;

        var worldAttributes = new ElementJson(lastId, 
            new IdentifierJson("Description",documentRootJson.World.Description), "", 
            "World Attributes", "label", 1, 
            new List<ElementValueJson>(), documentRootJson.World.Description,
            documentRootJson.World.Goals);
        
        ListAllSpacesAndElementsOrdered.Add(worldAttributes);
    }

    private void GetUrlElements(DocumentRootJson documentRootJson)
    {
        foreach (var url in documentRootJson.World.Elements)
        {
            if (url.ElementType is "url")
            {
                ListUrlElements.Add(url);
            }
        }
    }

    //Because spaces are represented as labels in Moodle, they are added as a ElementJson to the List.
    //If the User can somehow create more than 10000 Lelements, this will break. (But that´s unlikely)
    //Spaces are also added as "Elements", because we need a list containing both spaces and Elements.
    private void GetSpacesAndElementsOrdered(DocumentRootJson? documentRootJson)
    {
        if (documentRootJson != null)
        {
            foreach (var space in documentRootJson.World.Spaces)
            {
                List<ElementValueJson> values = new List<ElementValueJson>{new("", "0")};
                ListAllSpacesAndElementsOrdered.Add(new ElementJson(space.SpaceId+10000, space.Identifier, "", "space","space", 0, values, space.Description));
                
                foreach (int elementInSpace in space.SpaceContent)
                {
                    ListAllSpacesAndElementsOrdered.Add(documentRootJson.World.Elements[elementInSpace-1]);
                }
            }
        }
    }

    public List<ElementJson> GetH5PElementsList()
    {
        return ListH5PElements;
    }

    //because sections are not supported in the authoringTool yet, a dummy section is created
    //right now this dummy is just a space, but it will be a section in the future
    public List<SpaceJson> GetSectionList()
    {
        var space = new SpaceJson(0, new IdentifierJson("identifier", "Topic 0"), 
            new List<int>(), 0 ,0 );
        var spaceList = new List<SpaceJson> {space};
        return spaceList;
    }

    public List<ElementJson> GetResourceList()
    {
        return ListResourceElements;
    }
    
    public List<ElementJson> GetLabelsList()
    {
        return ListLabelElements;
    }
    
    public List<ElementJson> GetUrlList()
    {
        return ListUrlElements;
    }
    
    //A List that contains all Spaces and Elements in the correct order. 
    //First comes a Spaces followed by all his Elements until another Space appears.
    //The Spaces where transformed to ElementJson, so they can be used in the same List.
    public List<ElementJson> GetSpacesAndElementsOrderedList()
    {
        return ListAllSpacesAndElementsOrdered;
    }
    
}