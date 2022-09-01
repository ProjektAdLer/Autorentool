using System.IO.Abstractions;
using AutoMapper;
using BusinessLogic.API;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;

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

        _worlds = new List<LearningWorldViewModel>();
        _spaces = new List<ILearningSpaceViewModel>();
        _elements = new List<ILearningElementViewModel>();
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

    private List<LearningWorldViewModel> _worlds;
    private List<ILearningSpaceViewModel> _spaces;
    private List<ILearningElementViewModel> _elements;

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
        
        //In the future, we might have to load only partial object here instead of the real deal
        //because e.g. worlds with many, many elements might become very large and we don't want to load those before we need them
        var savePath = FindSuitableSavePath(obj);
        switch (obj)
        {
                case LearningWorldViewModel learningWorldViewModel:
                    BusinessLogic.SaveLearningWorld(Mapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldViewModel), savePath);
                    _worlds.Add(learningWorldViewModel);
                    break;
                case LearningSpaceViewModel learningSpaceViewModel:
                    BusinessLogic.SaveLearningSpace(Mapper.Map<BusinessLogic.Entities.LearningSpace>(learningSpaceViewModel), savePath);
                    _spaces.Add(learningSpaceViewModel);
                    break;
                case LearningElementViewModel learningElementViewModel:
                    BusinessLogic.SaveLearningElement(Mapper.Map<BusinessLogic.Entities.LearningElement>(learningElementViewModel), savePath);
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
            .Select(entity => Mapper.Map<LearningWorldViewModel>(entity)).ToList();
        _spaces = spaceFiles.Select(filepath => BusinessLogic.LoadLearningSpace(filepath))
            .Select(entity => Mapper.Map<LearningSpaceViewModel>(entity)).ToList<ILearningSpaceViewModel>();
        _elements = elementFiles.Select(filepath => BusinessLogic.LoadLearningElement(filepath))
            .Select(entity => Mapper.Map<LearningElementViewModel>(entity)).ToList<ILearningElementViewModel>();

        _initialized = true;
    }
}