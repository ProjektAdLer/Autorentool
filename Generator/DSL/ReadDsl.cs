using System.Diagnostics.CodeAnalysis;
using System.IO.Abstractions;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Shared;

namespace Generator.DSL;

public class ReadDsl : IReadDsl
{
    private readonly IFileSystem _fileSystem;
    private ILearningWorldJson _learningWorldJson;
    private List<LearningElementJson> _listAllElementsOrdered;
    private List<LearningElementJson> _listH5PElements;
    private List<LearningElementJson> _listLabelElements;
    private List<LearningElementJson> _listResourceElements;
    private List<LearningElementJson> _listUrlElements;
    private ILogger<ReadDsl> _logger;
    private DocumentRootJson _rootJson;


    public ReadDsl(IFileSystem fileSystem, ILogger<ReadDsl> logger)

    {
        Initialize();
        _fileSystem = fileSystem;
        _logger = logger;
    }

    /// <inheritdoc cref="IReadDsl.ReadLearningWorld"/>
    public void ReadLearningWorld(string dslPath, DocumentRootJson? rootJsonForTest = null)
    {
        Initialize();

        if (rootJsonForTest != null)
        {
            _rootJson = rootJsonForTest;
        }
        else
        {
            var jsonString = _fileSystem.File.ReadAllText(dslPath);
            var options = new JsonSerializerOptions { WriteIndented = true, PropertyNameCaseInsensitive = true };
            _rootJson = JsonSerializer.Deserialize<DocumentRootJson>(jsonString, options) ??
                        throw new InvalidOperationException("Could not deserialize DSL_Document");
        }

        GetH5PElements(_rootJson);
        GetResourceElements(_rootJson);
        GetLabelElements(_rootJson);
        GetWorldAttributes(_rootJson);
        GetUrlElements(_rootJson);
        GetElementsOrdered(_rootJson);
        SetLearningWorld(_rootJson);
    }

    /// <inheritdoc cref="IReadDsl.GetLearningWorld"/>
    public ILearningWorldJson GetLearningWorld()
    {
        return _learningWorldJson;
    }

    /// <inheritdoc cref="IReadDsl.GetH5PElementsList"/>
    public List<LearningElementJson> GetH5PElementsList()
    {
        return _listH5PElements;
    }

    /// <inheritdoc cref="IReadDsl.GetSectionList"/>
    public List<ILearningSpaceJson> GetSectionList()
    {
        var space = new LearningSpaceJson(0, "", "",
            new List<int?>(), -1, "", "");
        var spaceList = new List<ILearningSpaceJson> { space };
        spaceList.AddRange(_rootJson.World.Spaces);
        return spaceList;
    }

    /// <inheritdoc cref="IReadDsl.GetResourceElementList"/>
    public List<LearningElementJson> GetResourceElementList()
    {
        return _listResourceElements;
    }

    /// <inheritdoc cref="IReadDsl.GetLabelElementList"/>
    public List<LearningElementJson> GetLabelElementList()
    {
        return _listLabelElements;
    }

    /// <inheritdoc cref="IReadDsl.GetUrlElementList"/>
    public List<LearningElementJson> GetUrlElementList()
    {
        return _listUrlElements;
    }

    /// <inheritdoc cref="IReadDsl.GetElementsOrderedList"/>
    public List<LearningElementJson> GetElementsOrderedList()
    {
        return _listAllElementsOrdered;
    }

    [MemberNotNull(nameof(_learningWorldJson), nameof(_rootJson), nameof(_listH5PElements),
        nameof(_listResourceElements), nameof(_listLabelElements), nameof(_listUrlElements),
        nameof(_listAllElementsOrdered))]
    private void Initialize()
    {
        _learningWorldJson = new LearningWorldJson("Value",
            "", new List<ITopicJson>(),
            new List<ILearningSpaceJson>(), new List<IElementJson>());
        _rootJson = new DocumentRootJson("0.3", Constants.ApplicationVersion, "", "", _learningWorldJson);
        _listH5PElements = new List<LearningElementJson>();
        _listResourceElements = new List<LearningElementJson>();
        _listLabelElements = new List<LearningElementJson>();
        _listUrlElements = new List<LearningElementJson>();
        _listAllElementsOrdered = new List<LearningElementJson>();
    }

    /// <summary>
    /// Sets the learning world object using the provided DocumentRootJson object.
    /// </summary>
    private void SetLearningWorld(DocumentRootJson? documentRootJson)
    {
        if (documentRootJson != null) _learningWorldJson = documentRootJson.World;
    }

    /// <summary>
    /// Extracts H5P elements from the provided DocumentRootJson object and adds them to the H5P elements list.
    /// </summary>
    private void GetH5PElements(DocumentRootJson documentRootJson)
    {
        foreach (var element in documentRootJson.World.Elements)
        {
            if (element.ElementFileType == "h5p")
            {
                _listH5PElements.Add((LearningElementJson)element);
            }
        }

        _logger.LogTrace("Found {Count} H5P elements", _listH5PElements.Count);
    }

    /// <summary>
    /// Extracts resource elements (e.g., pdf, json, jpg, etc.) from the provided DocumentRootJson object and adds them to the resource elements list.
    /// </summary>
    private void GetResourceElements(DocumentRootJson documentRootJson)
    {
        foreach (var resource in documentRootJson.World.Elements)
        {
            if (resource.ElementFileType is "pdf" or "json" or "jpg" or "jpeg" or "png" or "webp" or "bmp" or "txt"
                or "c"
                or "h" or "cpp" or "cc" or "c++" or "py" or "cs" or "js" or "php" or "html" or "css")
            {
                _listResourceElements.Add((LearningElementJson)resource);
            }
        }

        _logger.LogTrace("Found {Count} resource elements", _listResourceElements.Count);
    }

    /// <summary>
    /// Extracts label elements from the provided DocumentRootJson object and adds them to the label elements list.
    /// </summary>
    private void GetLabelElements(DocumentRootJson documentRootJson)
    {
        foreach (var label in documentRootJson.World.Elements)
        {
            if (label.ElementFileType is "label")
            {
                _listLabelElements.Add((LearningElementJson)label);
            }
        }

        _logger.LogTrace("Found {Count} label elements", _listLabelElements.Count);
    }

    /// <summary>
    /// Retrieves world attributes from the provided DocumentRootJson object and adds them to the ordered elements list.
    /// </summary>
    private void GetWorldAttributes(DocumentRootJson documentRootJson)
    {
        // World Attributes like Description & Goals are added to the label-list, as they are represented as Labels in Moodle
        if (documentRootJson.World.WorldDescription == "" && documentRootJson.World.WorldGoals[0] == "")
        {
            _logger.LogTrace("No world description and goals found");
            return;
        }

        var lastId = documentRootJson.World.Elements.Count + 1;

        var worldAttributes = new LearningElementJson(lastId, "",
            documentRootJson.World.WorldDescription, "",
            "World Attributes", "label", 0,
            0, "", documentRootJson.World.WorldDescription,
            documentRootJson.World.WorldGoals);

        _listAllElementsOrdered.Add(worldAttributes);
    }

    /// <summary>
    /// Extracts URL elements from the provided DocumentRootJson object and adds them to the URL elements list.
    /// </summary>
    private void GetUrlElements(DocumentRootJson documentRootJson)
    {
        foreach (var url in documentRootJson.World.Elements)
        {
            if (url.ElementFileType is "url")
            {
                _listUrlElements.Add((LearningElementJson)url);
            }
        }

        _logger.LogTrace("Found {Count} URL elements", _listUrlElements.Count);
    }

    /// <summary>
    /// Extracts ordered elements from the provided DocumentRootJson object and adds them to the ordered elements list.
    /// </summary>
    private void GetElementsOrdered(DocumentRootJson? documentRootJson)
    {
        if (documentRootJson != null)
        {
            foreach (var space in documentRootJson.World.Spaces)
            {
                foreach (var elementInSpace in space.SpaceSlotContents)
                {
                    if (elementInSpace != null)
                        _listAllElementsOrdered.Add(
                            (LearningElementJson)documentRootJson.World.Elements[(int)elementInSpace - 1]);
                }
            }
        }
    }
}