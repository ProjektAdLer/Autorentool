using System.IO.Abstractions;
using System.Text.Json;


namespace Generator.DSL;

/// <summary>
/// Read the DSL file, generate a LearningWorldJson class and fill it with the DSL data.
/// </summary>
public class ReadDsl : IReadDsl
{
    private readonly List<LearningElementJson> _listH5PElements;
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
        _listH5PElements = new List<LearningElementJson>();
        _listLearningSpaces = new List<LearningSpaceJson>();
        _listDslDocument = new List<LearningElementJson>();
    }

    /// <summary>
    /// Get the DSL file text and give the data to the needed methods. 
    /// </summary>
    /// <param name="dslPath"></param>
    /// <param name="rootJsonForTest"></param>
    /// <exception cref="InvalidOperationException"></exception>
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
        CountLearningSpaces(_rootJson);
        GetDslDocument(_rootJson);
        SetLearningWorld(_rootJson);
    }

    /// <summary>
    /// Setter for the LearningWorldJson.
    /// </summary>
    /// <param name="documentRootJson"></param>
    private void SetLearningWorld(DocumentRootJson documentRootJson)
    {
         _learningWorldJson = documentRootJson.LearningWorld;
    }

    /// <summary>
    /// Getter for the LearningWorldJson. Its needed to get the LearningWorldJson to the Factories.
    /// </summary>
    /// <returns></returns>
    public LearningWorldJson GetLearningWorld()
    {
        return _learningWorldJson;
    }
    
    /// <summary>
    /// Count all H5P-Elements and add them to the _listH5PElements list.
    /// The H5PFactory need to know how many H5P-Elements are in the Learning World.
    /// </summary>
    /// <param name="documentRootJson"></param>
    private void GetH5PElements(DocumentRootJson documentRootJson)
    {
        foreach (var element in documentRootJson.LearningWorld.LearningElements)
        {
            if (element.ElementType == "h5p")
            {
                if (_listH5PElements != null) _listH5PElements.Add(element);
            }
        }
    }

    /// <summary>
    /// Count how many LearningSpaces are in the Learning World.
    /// </summary>
    /// <param name="documentRootJson"></param>
    private void CountLearningSpaces(DocumentRootJson? documentRootJson)
    {
        if (documentRootJson != null)
            foreach (var space in documentRootJson.LearningWorld.LearningSpaces)
            {
                _listLearningSpaces.Add(space);
            }
    }

    /// <summary>
    /// Find the "DSL Dokument" Element in the dsl file and add it to the _listDslDocument list.
    /// The FileFactory needs this Information to create the DSL Document in the MBZ-Structure. 
    /// </summary>
    /// <param name="documentRootJson"></param>
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
        return _listH5PElements;
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