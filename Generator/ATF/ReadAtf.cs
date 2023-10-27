using System.Diagnostics.CodeAnalysis;
using System.IO.Abstractions;
using System.Text.Json;
using Generator.ATF.AdaptivityElement;
using Microsoft.Extensions.Logging;
using Shared;

namespace Generator.ATF;

public class ReadAtf : IReadAtf
{
    private readonly IFileSystem _fileSystem;
    private ILearningWorldJson _learningWorldJson;
    private List<IAdaptivityElementJson> _listAdaptivityElements;
    private List<IElementJson> _listAllElementsOrdered;
    private List<IBaseLearningElementJson> _listBaseLearningElements;
    private List<ILearningElementJson> _listH5PElements;
    private List<ILearningElementJson> _listResourceElements;
    private List<ILearningSpaceJson> _listSpaces;
    private List<ILearningElementJson> _listUrlElements;
    private ILogger<ReadAtf> _logger;
    private DocumentRootJson _rootJson;
    private LearningElementJson _worldAttributes;


    public ReadAtf(IFileSystem fileSystem, ILogger<ReadAtf> logger)

    {
        Initialize();
        _fileSystem = fileSystem;
        _logger = logger;
    }

    /// <inheritdoc cref="IReadAtf.ReadLearningWorld"/>
    public void ReadLearningWorld(string atfPath, DocumentRootJson? rootJsonForTest = null)
    {
        Initialize();

        if (rootJsonForTest != null)
        {
            _rootJson = rootJsonForTest;
        }
        else
        {
            var jsonString = _fileSystem.File.ReadAllText(atfPath);
            var options = new JsonSerializerOptions { WriteIndented = true, PropertyNameCaseInsensitive = true };
            _rootJson = JsonSerializer.Deserialize<DocumentRootJson>(jsonString, options) ??
                        throw new InvalidOperationException("Could not deserialize ATF_Document");
        }

        GetH5PElements(_rootJson);
        GetResourceElements(_rootJson);
        GetWorldAttributes(_rootJson);
        GetSpaces(_rootJson);
        GetUrlElements(_rootJson);
        GetAdaptivityElements(_rootJson);
        GetBaseLearningElements(_rootJson);
        GetElementsOrdered(_rootJson);
        SetLearningWorld(_rootJson);
    }

    /// <inheritdoc cref="IReadAtf.GetLearningWorld"/>
    public ILearningWorldJson GetLearningWorld()
    {
        return _learningWorldJson;
    }

    /// <inheritdoc cref="IReadAtf.GetWorldAttributes"/>
    public LearningElementJson GetWorldAttributes()
    {
        return _worldAttributes;
    }

    /// <inheritdoc cref="IReadAtf.GetH5PElementsList"/>
    public List<ILearningElementJson> GetH5PElementsList()
    {
        return _listH5PElements;
    }

    /// <inheritdoc cref="IReadAtf.GetSpaceList"/>
    public List<ILearningSpaceJson> GetSpaceList()
    {
        return _listSpaces;
    }

    /// <inheritdoc cref="IReadAtf.GetResourceElementList"/>
    public List<ILearningElementJson> GetResourceElementList()
    {
        return _listResourceElements;
    }

    /// <inheritdoc cref="IReadAtf.GetUrlElementList"/>
    public List<ILearningElementJson> GetUrlElementList()
    {
        return _listUrlElements;
    }

    /// <inheritdoc cref="IReadAtf.GetAdaptivityElementsList"/>
    public List<IAdaptivityElementJson> GetAdaptivityElementsList()
    {
        return _listAdaptivityElements;
    }

    /// <inheritdoc cref="IReadAtf.GetElementsOrderedList"/>
    public List<IElementJson> GetElementsOrderedList()
    {
        return _listAllElementsOrdered;
    }

    /// <inheritdoc cref="IReadAtf.GetBaseLearningElementsList"/>
    public List<IBaseLearningElementJson> GetBaseLearningElementsList()
    {
        return _listBaseLearningElements;
    }

    [MemberNotNull(nameof(_learningWorldJson), nameof(_listSpaces), nameof(_worldAttributes),
        nameof(_rootJson), nameof(_listH5PElements), nameof(_listResourceElements), nameof(_listUrlElements),
        nameof(_listAllElementsOrdered), nameof(_listAdaptivityElements), nameof(_listBaseLearningElements))]
    private void Initialize()
    {
        _learningWorldJson = new LearningWorldJson("Value",
            "", new List<ITopicJson>(),
            new List<ILearningSpaceJson>(), new List<IElementJson>());
        _worldAttributes = new LearningElementJson(0, "", "", "", "", "", 0, 0, "");
        _rootJson = new DocumentRootJson(Constants.AtfVersion, Constants.ApplicationVersion, "", "",
            _learningWorldJson);
        _listSpaces = new List<ILearningSpaceJson>();
        _listH5PElements = new List<ILearningElementJson>();
        _listResourceElements = new List<ILearningElementJson>();
        _listUrlElements = new List<ILearningElementJson>();
        _listAdaptivityElements = new List<IAdaptivityElementJson>();
        _listAllElementsOrdered = new List<IElementJson>();
        _listBaseLearningElements = new List<IBaseLearningElementJson>();
    }

    /// <summary>
    /// Sets the learning world object using the provided DocumentRootJson object.
    /// </summary>
    private void SetLearningWorld(IDocumentRootJson? documentRootJson)
    {
        if (documentRootJson != null) _learningWorldJson = documentRootJson.World;
    }

    /// <summary>
    /// Extracts H5P elements from the provided DocumentRootJson object and adds them to the H5P elements list.
    /// </summary>
    private void GetH5PElements(IDocumentRootJson documentRootJson)
    {
        foreach (var element in documentRootJson.World.Elements)
        {
            if (element.ElementFileType == "h5p")
            {
                _listH5PElements.Add((ILearningElementJson)element);
            }
        }

        _logger.LogTrace("Found {Count} H5P elements", _listH5PElements.Count);
    }

    /// <summary>
    /// Extracts resource elements (e.g., pdf, json, jpg, etc.) from the provided DocumentRootJson object and adds them to the resource elements list.
    /// </summary>
    private void GetResourceElements(IDocumentRootJson documentRootJson)
    {
        foreach (var resource in documentRootJson.World.Elements)
        {
            if (resource.ElementFileType is "pdf" or "json" or "jpg" or "jpeg" or "png" or "webp" or "bmp" or "txt"
                or "c"
                or "h" or "cpp" or "cc" or "c++" or "py" or "cs" or "js" or "php" or "html" or "css")
            {
                _listResourceElements.Add((ILearningElementJson)resource);
            }
        }

        _logger.LogTrace("Found {Count} resource elements", _listResourceElements.Count);
    }

    /// <summary>
    /// Retrieves world attributes from the provided DocumentRootJson object and adds them to the ordered elements list.
    /// </summary>
    private void GetWorldAttributes(IDocumentRootJson documentRootJson)
    {
        // World Attributes like Description & Goals are added to the label-list, as they are represented as Labels in Moodle
        if (documentRootJson.World.WorldDescription == "" && documentRootJson.World.WorldGoals[0] == "")
        {
            _logger.LogTrace("No world description and goals found");
            return;
        }

        var lastId = documentRootJson.World.Elements.Count + 1;

        _worldAttributes = new LearningElementJson(lastId, "",
            documentRootJson.World.WorldDescription, "",
            "World Attributes", "label", 0,
            0, "", documentRootJson.World.WorldDescription,
            documentRootJson.World.WorldGoals);

        _listAllElementsOrdered.Add(_worldAttributes);
    }

    /// <summary>
    /// Extracts spaces from the provided DocumentRootJson object and adds them to the spaces list.
    /// </summary>
    private void GetSpaces(IDocumentRootJson rootJson)
    {
        _listSpaces = rootJson.World.Spaces;
    }

    /// <summary>
    /// Extracts URL elements from the provided DocumentRootJson object and adds them to the URL elements list.
    /// </summary>
    private void GetUrlElements(IDocumentRootJson documentRootJson)
    {
        foreach (var url in documentRootJson.World.Elements)
        {
            if (url.ElementFileType is "url")
            {
                _listUrlElements.Add((ILearningElementJson)url);
            }
        }

        _logger.LogTrace("Found {Count} URL elements", _listUrlElements.Count);
    }

    /// <summary>
    /// Extracts adaptivity elements from the provided DocumentRootJson object and adds them to the adaptivity elements list.
    /// </summary>
    private void GetAdaptivityElements(IDocumentRootJson documentRootJson)
    {
        foreach (var element in documentRootJson.World.Elements.Where(
                     element => element.ElementFileType is "adaptivity"))
        {
            _listAdaptivityElements.Add((IAdaptivityElementJson)element);
        }
    }

    /// <summary>
    /// Extracts ordered elements from the provided DocumentRootJson object and adds them to the ordered elements list.
    /// </summary>
    private void GetElementsOrdered(IDocumentRootJson? documentRootJson)
    {
        if (documentRootJson != null)
        {
            foreach (var space in documentRootJson.World.Spaces)
            {
                foreach (var elementInSpace in space.SpaceSlotContents)
                {
                    if (elementInSpace != null)
                        _listAllElementsOrdered.Add(documentRootJson.World.Elements[(int)elementInSpace - 1]);
                }
            }

            _listAllElementsOrdered.AddRange(_listBaseLearningElements);
        }
    }

    /// <summary>
    /// Adds all BaseLearningElementJson elements from documentRootJson to _listBaseLearningElements.
    /// </summary>
    private void GetBaseLearningElements(IDocumentRootJson? documentRootJson)
    {
        _listBaseLearningElements.AddRange(documentRootJson?.World.Elements
            .OfType<BaseLearningElementJson>() ?? Enumerable.Empty<BaseLearningElementJson>());
    }
}