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
        FileSystem = fileSystem;

        _toolboxSavePath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "AdLerAuthoring", "Toolbox");
        Logger.LogDebug("_toolboxSavePath is {}", _toolboxSavePath);

        _worlds = new List<LearningWorldViewModel>();
        _spaces = new List<LearningSpaceViewModel>();
        _elements = new List<LearningElementViewModel>();
        _initialized = false;
        
        //ensure path is created
        if (FileSystem.Directory.Exists(_toolboxSavePath)) return;
        Logger.LogDebug("_toolboxSavePath does not exist, creating");
        FileSystem.Directory.CreateDirectory(_toolboxSavePath);
    }

    internal ILogger<ToolboxEntriesProvider> Logger { get; }
    internal IBusinessLogic BusinessLogic { get; }
    internal IEntityMapping EntityMapping { get; }
    internal IFileSystem FileSystem { get; }

    private List<LearningWorldViewModel> _worlds;
    private List<LearningSpaceViewModel> _spaces;
    private List<LearningElementViewModel> _elements;

    private bool _initialized;
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
        try
        {
            if (IsElementDuplicate(obj)) return false;
        }
        catch (ArgumentOutOfRangeException)
        {
            Logger.LogError("Caught ArgumentOutOfRangeException in AddEntry for object of type {}", obj.GetType().Name);
            return false;
        }
        
        //construct a file path (automatically without user input)
        //check if file at file path exists already
        //if yes, append _n where n is tries and try again
        //if no, save file
        //if file successfully saved, add object to correct collection
        //return true
        var savePath = FindSuitableSavePath(obj);
        switch (obj)
        {
                case LearningWorldViewModel learningWorldViewModel:
                    BusinessLogic.SaveLearningWorld(EntityMapping.WorldMapper.ToEntity(learningWorldViewModel), savePath);
                    _worlds.Add(learningWorldViewModel);
                    break;
                case LearningSpaceViewModel learningSpaceViewModel:
                    BusinessLogic.SaveLearningSpace(EntityMapping.SpaceMapper.ToEntity(learningSpaceViewModel), savePath);
                    _spaces.Add(learningSpaceViewModel);
                    break;
                case LearningElementViewModel learningElementViewModel:
                    BusinessLogic.SaveLearningElement(EntityMapping.ElementMapper.ToEntity(learningElementViewModel), savePath);
                    _elements.Add(learningElementViewModel);
                    break;
        }
        return true;
    }
    
    private string FindSuitableSavePath(IDisplayableLearningObject obj)
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
    private bool IsElementDuplicate(IDisplayableLearningObject obj)
    {
        EnsureEntriesPopulated();
        return obj switch
        {
            LearningWorldViewModel world => _worlds.Any(w => w.Name == world.Name),
            LearningSpaceViewModel space => _spaces.Any(s => s.Name == space.Name),
            LearningElementViewModel element => _elements.Any(e => e.Name == element.Name),
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
        
        var worldFiles = files.Where(filepath => filepath.EndsWith(LearningWorldViewModel.fileEnding));
        var spaceFiles = files.Where(filepath => filepath.EndsWith(LearningSpaceViewModel.fileEnding));
        var elementFiles = files.Where(filepath => filepath.EndsWith(LearningElementViewModel.fileEnding));
        
        _worlds = worldFiles.Select(filepath => BusinessLogic.LoadLearningWorld(filepath))
            .Select(entity => EntityMapping.WorldMapper.ToViewModel(entity)).ToList();
        _spaces = spaceFiles.Select(filepath => BusinessLogic.LoadLearningSpace(filepath))
            .Select(entity => EntityMapping.SpaceMapper.ToViewModel(entity)).ToList();
        _elements = elementFiles.Select(filepath => BusinessLogic.LoadLearningElement(filepath))
            .Select(entity => EntityMapping.ElementMapper.ToViewModel(entity)).ToList();

        _initialized = true;
    }
}