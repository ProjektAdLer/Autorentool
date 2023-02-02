using System.IO.Abstractions;
using AutoMapper;
using BusinessLogic.API;
using Presentation.PresentationLogic.Element;
using Presentation.PresentationLogic.Space;
using Presentation.PresentationLogic.World;

namespace Presentation.PresentationLogic.Toolbox;

public class ToolboxEntriesProvider : IToolboxEntriesProviderModifiable
{
    public ToolboxEntriesProvider(ILogger<ToolboxEntriesProvider> logger, IBusinessLogic businessLogic,
        IMapper mapper) : this(logger, businessLogic, mapper, new FileSystem()) { }
    public ToolboxEntriesProvider(ILogger<ToolboxEntriesProvider> logger, IBusinessLogic businessLogic,
        IMapper mapper, IFileSystem fileSystem)
    {
        Logger = logger;
        BusinessLogic = businessLogic;
        Mapper = mapper;
        FileSystem = fileSystem;

        _toolboxSavePath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "AdLerAuthoring", "Toolbox");
        Logger.LogDebug("_toolboxSavePath is {}", _toolboxSavePath);

        _worlds = new List<WorldViewModel>();
        _spaces = new List<ISpaceViewModel>();
        _elements = new List<IElementViewModel>();
        _initialized = false;
        
        //ensure path is created
        if (FileSystem.Directory.Exists(_toolboxSavePath)) return;
        Logger.LogDebug("_toolboxSavePath does not exist, creating");
        FileSystem.Directory.CreateDirectory(_toolboxSavePath);
    }

    internal ILogger<ToolboxEntriesProvider> Logger { get; }
    internal IBusinessLogic BusinessLogic { get; }
    internal IMapper Mapper { get; }
    internal IFileSystem FileSystem { get; }

    private List<WorldViewModel> _worlds;
    private List<ISpaceViewModel> _spaces;
    private List<IElementViewModel> _elements;

    private bool _initialized;
    //TODO: remove this and instead get path from configuration 
    private string _toolboxSavePath;

    /// <inheritdoc cref="IToolboxEntriesProvider.Entries"/>
    public IEnumerable<IDisplayableObject> Entries
    {
        get
        {
            EnsureEntriesPopulated();
            return ((IEnumerable<IDisplayableObject>)_worlds).Concat(_spaces).Concat(_elements);
        }
    }

    /// <inheritdoc cref="IToolboxEntriesProviderModifiable.AddEntry"/>
    public bool AddEntry(IDisplayableObject obj)
    {
        EnsureEntriesPopulated();
        try
        {
            if (IsElementDuplicate(obj)) return false;
        }
        catch (ArgumentOutOfRangeException)
        {
            Logger.LogError("Caught ArgumentOutOfRangeException in AddEntry for object of type {}", obj.GetType().Name);
            return false;
        }
        
        //In the future, we might have to load only partial object here instead of the real deal
        //because e.g. worlds with many, many elements might become very large and we don't want to load those before we need them
        var savePath = FindSuitableSavePath(obj);
        switch (obj)
        {
                case WorldViewModel worldViewModel:
                    BusinessLogic.SaveWorld(Mapper.Map<BusinessLogic.Entities.World>(worldViewModel), savePath);
                    _worlds.Add(worldViewModel);
                    break;
                case SpaceViewModel spaceViewModel:
                    BusinessLogic.SaveSpace(Mapper.Map<BusinessLogic.Entities.Space>(spaceViewModel), savePath);
                    _spaces.Add(spaceViewModel);
                    break;
                case ElementViewModel elementViewModel:
                    BusinessLogic.SaveElement(Mapper.Map<BusinessLogic.Entities.Element>(elementViewModel), savePath);
                    _elements.Add(elementViewModel);
                    break;
        }
        return true;
    }
    
    private string FindSuitableSavePath(IDisplayableObject obj)
    {
        return BusinessLogic.FindSuitableNewSavePath(_toolboxSavePath, obj.Name, obj.FileEnding);
    }


    /// <summary>
    /// Checks whether the passed element already exists in our lists, meaning there is an element of same type with the same
    /// name in our store.
    /// </summary>
    /// <param name="obj">The object to be checked.</param>
    /// <exception cref="ApplicationException">Internal error, either <see cref="_worlds"/> or <see cref="_spaces"/>
    /// or <see cref="_elements"/> was null but shouldn't be.</exception>
    /// <returns>True if it is a duplicate, false otherwise.</returns>
    private bool IsElementDuplicate(IDisplayableObject obj)
    {
        EnsureEntriesPopulated();
        return obj switch
        {
            WorldViewModel world => _worlds.Any(w => w.Name == world.Name),
            SpaceViewModel space => _spaces.Any(s => s.Name == space.Name),
            ElementViewModel element => _elements.Any(e => e.Name == element.Name),
            _ => throw new ArgumentOutOfRangeException(nameof(obj), "object isn't valid for toolbox"),
        };
    }
    
    /// <summary>
    /// Populates <see cref="Entries"/> if they are not populated.
    /// </summary>
    private void EnsureEntriesPopulated()
    {
        if (_initialized) return;
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
        
        var worldFiles = files.Where(filepath => filepath.EndsWith(WorldViewModel.fileEnding));
        var spaceFiles = files.Where(filepath => filepath.EndsWith(SpaceViewModel.fileEnding));
        var elementFiles = files.Where(filepath => filepath.EndsWith(ElementViewModel.fileEnding));
        
        _worlds = worldFiles.Select(filepath => BusinessLogic.LoadWorld(filepath))
            .Select(entity => Mapper.Map<WorldViewModel>(entity)).ToList();
        _spaces = spaceFiles.Select(filepath => BusinessLogic.LoadSpace(filepath))
            .Select(entity => Mapper.Map<SpaceViewModel>(entity)).ToList<ISpaceViewModel>();
        _elements = elementFiles.Select(filepath => BusinessLogic.LoadElement(filepath))
            .Select(entity => Mapper.Map<ElementViewModel>(entity)).ToList<IElementViewModel>();

        _initialized = true;
    }
}