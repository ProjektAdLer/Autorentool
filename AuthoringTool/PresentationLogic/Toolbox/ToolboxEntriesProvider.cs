using System.IO.Abstractions;
using AuthoringTool.BusinessLogic.API;
using AuthoringTool.PresentationLogic.EntityMapping;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.LearningWorld;

namespace AuthoringTool.PresentationLogic.Toolbox;

public class ToolboxEntriesProvider : IToolboxEntriesProviderModifiable
{
    public ToolboxEntriesProvider(ILogger<ToolboxEntriesProvider> logger, IBusinessLogic businessLogic,
        IEntityMapping entityMapping) : this(logger, businessLogic, entityMapping, new FileSystem()) { }
    public ToolboxEntriesProvider(ILogger<ToolboxEntriesProvider> logger, IBusinessLogic businessLogic,
        IEntityMapping entityMapping, IFileSystem fileSystem)
    {
        Logger = logger;
        BusinessLogic = businessLogic;
        EntityMapping = entityMapping;
        FileSystem = fileSystem ?? new FileSystem();

        _toolboxSavePath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "AdLerAuthoring", "Toolbox");
        Logger.LogDebug("_toolboxSavePath is {}", _toolboxSavePath);

        _worlds = null;
        _spaces = null;
        _elements = null;
        
        //ensure path is created
        if (FileSystem.Directory.Exists(_toolboxSavePath)) return;
        Logger.LogDebug("_toolboxSavePath does not exist, creating");
        FileSystem.Directory.CreateDirectory(_toolboxSavePath);
    }

    internal ILogger<ToolboxEntriesProvider> Logger { get; }
    internal IBusinessLogic BusinessLogic { get; }
    internal IEntityMapping EntityMapping { get; }
    internal IFileSystem FileSystem { get; }

    private List<LearningWorldViewModel>? _worlds;
    private List<LearningSpaceViewModel>? _spaces;
    private List<LearningElementViewModel>? _elements;
    //TODO: remove this and instead get path from configuration 
    private string _toolboxSavePath;

    /// <inheritdoc cref="IToolboxEntriesProvider.Entries"/>
    public IEnumerable<IDisplayableLearningObject> Entries
    {
        get
        {
            EnsureEntriesPopulated();
            return ((IEnumerable<IDisplayableLearningObject>)_worlds!).Concat(_spaces!).Concat(_elements!);
        }
    }

    /// <inheritdoc cref="IToolboxEntriesProviderModifiable.AddEntry"/>
    public bool AddEntry(IDisplayableLearningObject obj)
    {
        EnsureEntriesPopulated();
        if (IsElementDuplicate(obj)) return false;
        //TODO: implement
        throw new NotImplementedException();
    }
    

    /// <summary>
    /// Checks whether the passed element already exists in our lists, meaning there is an element of same type with the same
    /// name in our store.
    /// </summary>
    /// <param name="obj">The object to be checked.</param>
    /// <exception cref="ApplicationException">Internal error, either <see cref="_worlds"/> or <see cref="_spaces"/>
    /// or <see cref="_elements"/> was null but shouldn't be.</exception>
    /// <returns>True if it is a duplicate, false otherwise.</returns>
    private bool IsElementDuplicate(IDisplayableLearningObject obj)
    {
        EnsureEntriesPopulated();
        return obj switch
        {
            LearningWorldViewModel world => _worlds!.Any(w => w.Name == world.Name),
            LearningSpaceViewModel space => _spaces!.Any(s => s.Name == space.Name),
            LearningElementViewModel element => _elements!.Any(e => e.Name == element.Name),
            _ => false,
        };
    }
    
    /// <summary>
    /// Populates <see cref="Entries"/> if they are not populated.
    /// </summary>
    private void EnsureEntriesPopulated()
    {
        if (_worlds != null && _spaces != null && _elements != null) return;
        Logger.LogTrace("Entries aren't populated, rebuilding");
        PopulateEntries();
    }
    
    /// <summary>
    /// (Re)Populates <see cref="_worlds"/>, <see cref="_spaces"/> and <see cref="_elements"/> with files
    /// from <see cref="_toolboxSavePath"/>.
    /// </summary>
    private void PopulateEntries()
    {
        Logger.LogInformation("(Re-)Building Toolbox entries from path {}", _toolboxSavePath);
        var files = FileSystem.Directory.EnumerateFiles(_toolboxSavePath).ToArray();
        
        var worldFiles = files.Where(filepath => filepath.EndsWith(".awf"));
        var spaceFiles = files.Where(filepath => filepath.EndsWith(".asf"));
        var elementFiles = files.Where(filepath => filepath.EndsWith(".aef"));
        
        _worlds = worldFiles.Select(filepath => BusinessLogic.LoadLearningWorld(filepath))
            .Select(entity => EntityMapping.WorldMapper.ToViewModel(entity)).ToList();
        _spaces = spaceFiles.Select(filepath => BusinessLogic.LoadLearningSpace(filepath))
            .Select(entity => EntityMapping.SpaceMapper.ToViewModel(entity)).ToList();
        _elements = elementFiles.Select(filepath => BusinessLogic.LoadLearningElement(filepath))
            .Select(entity => EntityMapping.ElementMapper.ToViewModel(entity)).ToList();
    }
}