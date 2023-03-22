using AutoMapper;
using BusinessLogic.API;
using BusinessLogic.Commands.Condition;
using BusinessLogic.Commands.Element;
using BusinessLogic.Commands.Layout;
using BusinessLogic.Commands.Pathway;
using BusinessLogic.Commands.Space;
using BusinessLogic.Commands.World;
using BusinessLogic.Entities.LearningContent;
using ElectronWrapper;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.ElectronNET;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Shared;
using Shared.Command;
using Shared.Configuration;

namespace Presentation.PresentationLogic.API;

public class PresentationLogic : IPresentationLogic
{
    public PresentationLogic(
        IAuthoringToolConfiguration configuration,
        IBusinessLogic businessLogic,
        IMapper mapper,
        ICachingMapper cMapper,
        IServiceProvider serviceProvider,
        ILogger<PresentationLogic> logger,
        IHybridSupportWrapper hybridSupportWrapper,
        IShellWrapper shellWrapper)
    {
        _logger = logger;
        Configuration = configuration;
        BusinessLogic = businessLogic;
        Mapper = mapper;
        CMapper = cMapper;
        HybridSupportWrapper = hybridSupportWrapper;
        ShellWrapper = shellWrapper;
        _dialogManager = serviceProvider.GetService(typeof(IElectronDialogManager)) as IElectronDialogManager;
    }

    private readonly ILogger<PresentationLogic> _logger;
    private readonly IElectronDialogManager? _dialogManager;

    private const string WorldFileEnding = "awf";
    private const string SpaceFileEnding = "asf";
    private const string ElementFileEnding = "aef";
    private readonly string[] _imageFileEnding = {"jpg", "png", "webp", "bmp"};
    private readonly string[] _textFileEnding =
        { "txt", "c", "h", "cpp", "cc", "c++", "py", "cs", "js", "php", "html", "css" };
    private const string VideoFileEnding = "mp4";
    private const string H5PFileEnding = "h5p";
    private const string PdfFileEnding = "pdf";
    private const string WorldFileFormatDescriptor = "AdLer World File";
    private const string SpaceFileFormatDescriptor = "AdLer Space File";
    private const string ElementFileFormatDescriptor = "AdLer Element File";

    public IAuthoringToolConfiguration Configuration { get; }
    public IBusinessLogic BusinessLogic { get; }
    internal IMapper Mapper { get; }
    internal ICachingMapper CMapper { get; }
    public bool RunningElectron => HybridSupportWrapper.IsElectronActive;
    private IHybridSupportWrapper HybridSupportWrapper { get; }
    private IShellWrapper ShellWrapper { get; }
    public bool CanUndo => BusinessLogic.CanUndo;
    public bool CanRedo => BusinessLogic.CanRedo;
    public event EventHandler<CommandUndoRedoOrExecuteArgs> OnCommandUndoRedoOrExecute
    {
        add => BusinessLogic.OnCommandUndoRedoOrExecute += value;
        remove => BusinessLogic.OnCommandUndoRedoOrExecute -= value;
    }

    public async Task<string> ConstructBackupAsync(LearningWorldViewModel learningWorldViewModel)
    {
        var entity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldViewModel);
        var filepath = await GetSaveFilepathAsync("Export learning world", "mbz", "Moodle Backup Zip");
        BusinessLogic.ConstructBackup(entity, filepath);
        return filepath;
    }
    
    public void UndoCommand()
    {
        BusinessLogic.UndoCommand();
    }
    
    public void RedoCommand()
    {
        BusinessLogic.RedoCommand();
    }

    public void AddLearningWorld(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm,
        ILearningWorldViewModel learningWorldVm)
    {
        var authoringToolWorkspaceEntity = Mapper.Map<BusinessLogic.Entities.AuthoringToolWorkspace>(authoringToolWorkspaceVm);
        var worldEntity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm);

        var command = new CreateLearningWorld(authoringToolWorkspaceEntity, worldEntity,
            workspace => CMapper.Map(workspace, authoringToolWorkspaceVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.CreateLearningWorld"/>
    public void CreateLearningWorld(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm, string name,
        string shortname, string authors, string language, string description, string goals)
    {
        var authoringToolWorkspaceEntity = Mapper.Map<BusinessLogic.Entities.AuthoringToolWorkspace>(authoringToolWorkspaceVm);

        var command = new CreateLearningWorld(authoringToolWorkspaceEntity, name, shortname, authors, language, description, goals,
            workspace => CMapper.Map(workspace, authoringToolWorkspaceVm));
        BusinessLogic.ExecuteCommand(command);
    }
    
    /// <inheritdoc cref="IPresentationLogic.EditLearningWorld"/>
    public void EditLearningWorld(ILearningWorldViewModel learningWorldVm, string name,
        string shortname, string authors, string language, string description, string goals)
    {
        var worldEntity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm);

        var command = new EditLearningWorld(worldEntity, name, shortname, authors, language, description, goals,
            world => CMapper.Map(world, learningWorldVm));
        BusinessLogic.ExecuteCommand(command);
    }
    
    /// <inheritdoc cref="IPresentationLogic.DeleteLearningWorld"/>
    public void DeleteLearningWorld(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm, LearningWorldViewModel worldVm)
    {
        var authoringToolWorkspaceEntity = Mapper.Map<BusinessLogic.Entities.AuthoringToolWorkspace>(authoringToolWorkspaceVm);
        var worldEntity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(worldVm);

        var command = new DeleteLearningWorld(authoringToolWorkspaceEntity, worldEntity,
            workspace => CMapper.Map(workspace, authoringToolWorkspaceVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.SaveLearningWorldAsync"/>
    public async Task SaveLearningWorldAsync(LearningWorldViewModel learningWorldViewModel)
    {
        ElectronCheck();
        var filepath = await GetSaveFilepathAsync("Save Learning World", WorldFileEnding, WorldFileFormatDescriptor);
        var worldEntity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldViewModel);
        var command = new SaveLearningWorld(BusinessLogic, worldEntity, filepath);
        BusinessLogic.ExecuteCommand(command);
        learningWorldViewModel.UnsavedChanges = false;
        AddSavedLearningWorldPath(new SavedLearningWorldPath()
            {Id = worldEntity.Id, Name = worldEntity.Name, Path = filepath});
    }

    /// <inheritdoc cref="IPresentationLogic.LoadLearningWorldAsync"/>
    public async Task LoadLearningWorldAsync(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm)
    {
        ElectronCheck();
        var filepath = await GetLoadFilepathAsync("Load Learning World", WorldFileEnding, WorldFileFormatDescriptor);
        var workspaceEntity = Mapper.Map<BusinessLogic.Entities.AuthoringToolWorkspace>(authoringToolWorkspaceVm);
        var command = new LoadLearningWorld(workspaceEntity, filepath, BusinessLogic,
            workspace => CMapper.Map(workspace, authoringToolWorkspaceVm));
        BusinessLogic.ExecuteCommand(command);
    }

    public async Task<string> GetWorldSavePath()
    {
        ElectronCheck();
        var filepath = await GetLoadFilepathAsync("Load Learning World", WorldFileEnding, WorldFileFormatDescriptor);
        return filepath;
    }
    
    /// <inheritdoc cref="IPresentationLogic.LoadLearningWorldFromPath"/>
    public void LoadLearningWorldFromPath(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm, string path)
    {
        ElectronCheck();
        var workspaceEntity = Mapper.Map<BusinessLogic.Entities.AuthoringToolWorkspace>(authoringToolWorkspaceVm);
        var command = new LoadLearningWorld(workspaceEntity, path, BusinessLogic,
            workspace => CMapper.Map(workspace, authoringToolWorkspaceVm));
        BusinessLogic.ExecuteCommand(command);
    }

    public IEnumerable<SavedLearningWorldPath> GetSavedLearningWorldPaths()
    {
        return BusinessLogic.GetSavedLearningWorldPaths();
    }

    public void AddSavedLearningWorldPath(SavedLearningWorldPath savedLearningWorldPath)
    {
        BusinessLogic.AddSavedLearningWorldPath(savedLearningWorldPath);
    }

    public SavedLearningWorldPath AddSavedLearningWorldPathByPathOnly(string path)
    {
        return BusinessLogic.AddSavedLearningWorldPathByPathOnly(path);
    }

    public void UpdateIdOfSavedLearningWorldPath(SavedLearningWorldPath savedLearningWorldPath, Guid id)
    {
        BusinessLogic.UpdateIdOfSavedLearningWorldPath(savedLearningWorldPath, id);
    }

    public void RemoveSavedLearningWorldPath(SavedLearningWorldPath savedLearningWorldPath)
    {
        BusinessLogic.RemoveSavedLearningWorldPath(savedLearningWorldPath);
    }

    public void AddLearningSpace(ILearningWorldViewModel learningWorldVm, ILearningSpaceViewModel learningSpaceVm)
    {
        var worldEntity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm);
        var spaceEntity = Mapper.Map<BusinessLogic.Entities.LearningSpace>(learningSpaceVm);

        var command = new CreateLearningSpace(worldEntity, spaceEntity, 
            world => CMapper.Map(world, learningWorldVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.CreateLearningSpace"/>
    public void CreateLearningSpace(ILearningWorldViewModel learningWorldVm, string name, string shortname,
        string authors, string description, string goals, int requiredPoints, double positionX, double positionY)
    {
        var worldEntity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm);

        var command = new CreateLearningSpace(worldEntity, name, shortname, authors, description, goals, requiredPoints, 
            positionX, positionY, world => CMapper.Map(world, learningWorldVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.EditLearningSpace"/>
    public void EditLearningSpace(ILearningSpaceViewModel learningSpaceVm, string name,
        string shortname, string authors, string description, string goals, int requiredPoints)
    {
        var spaceEntity = Mapper.Map<BusinessLogic.Entities.LearningSpace>(learningSpaceVm);

        var command = new EditLearningSpace(spaceEntity, name, shortname, authors, description, goals, requiredPoints,
            space => CMapper.Map(space, learningSpaceVm));
        BusinessLogic.ExecuteCommand(command);
    }
    
    /// <inheritdoc cref="IPresentationLogic.ChangeLearningSpaceLayout"/>
    public void ChangeLearningSpaceLayout(ILearningSpaceViewModel learningSpaceVm, FloorPlanEnum floorPlanName)
    {
        var spaceEntity = Mapper.Map<BusinessLogic.Entities.LearningSpace>(learningSpaceVm);

        var command = new ChangeLearningSpaceLayout(spaceEntity, floorPlanName,
            space => CMapper.Map(space, learningSpaceVm));
        BusinessLogic.ExecuteCommand(command);
    }
    
    public void DragObjectInPathWay(IObjectInPathWayViewModel objectInPathWayVm , double oldPositionX, double oldPositionY)
    {
        var objectInPathWayEntity = Mapper.Map<BusinessLogic.Entities.IObjectInPathWay>(objectInPathWayVm);
        
        var command = new DragObjectInPathWay(objectInPathWayEntity, oldPositionX, oldPositionY, objectInPathWayEntity.PositionX,
            objectInPathWayEntity.PositionY, space => CMapper.Map(space, objectInPathWayVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.DeleteLearningSpace"/>
    public void DeleteLearningSpace(ILearningWorldViewModel learningWorldVm, ILearningSpaceViewModel learningSpaceVm)
    {
        var worldEntity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm);
        var spaceEntity = Mapper.Map<BusinessLogic.Entities.LearningSpace>(learningSpaceVm);

        var command = new DeleteLearningSpace(worldEntity, spaceEntity,
            world => CMapper.Map(world, learningWorldVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.SaveLearningSpaceAsync"/>
    public async Task SaveLearningSpaceAsync(LearningSpaceViewModel learningSpaceViewModel)
    {
        ElectronCheck();
        var filepath = await GetSaveFilepathAsync("Save Learning Space", SpaceFileEnding, SpaceFileFormatDescriptor);
        var spaceEntity = Mapper.Map<BusinessLogic.Entities.LearningSpace>(learningSpaceViewModel);
        var command = new SaveLearningSpace(BusinessLogic, spaceEntity, filepath);
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.LoadLearningSpaceAsync"/>
    public async Task LoadLearningSpaceAsync(ILearningWorldViewModel learningWorldVm)
    {
        ElectronCheck();
        var filepath = await GetLoadFilepathAsync("Load Learning Space", SpaceFileEnding, SpaceFileFormatDescriptor);
        var worldEntity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm);
        var command = new LoadLearningSpace(worldEntity, filepath, BusinessLogic, 
            world => CMapper.Map(world, learningWorldVm));
        BusinessLogic.ExecuteCommand(command);
    }
    
    /// <inheritdoc cref="IPresentationLogic.CreatePathWayCondition"/>
    public void CreatePathWayCondition(ILearningWorldViewModel learningWorldVm, ConditionEnum condition, double positionX, double positionY)
    {
        var worldEntity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm);
        
        var command = new CreatePathWayCondition(worldEntity, condition, positionX, positionY, 
            world => CMapper.Map(world, learningWorldVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.CreatePathWayConditionBetweenObjects"/>
    public void CreatePathWayConditionBetweenObjects(ILearningWorldViewModel learningWorldVm, ConditionEnum condition,
        IObjectInPathWayViewModel sourceObject, ILearningSpaceViewModel targetObject)
    {
        var worldEntity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm);
        var sourceObjectEntity = Mapper.Map<BusinessLogic.Entities.IObjectInPathWay>(sourceObject);
        var targetObjectEntity = Mapper.Map<BusinessLogic.Entities.LearningSpace>(targetObject);
        
        var command = new CreatePathWayCondition(worldEntity, condition, sourceObjectEntity, targetObjectEntity, 
            world => CMapper.Map(world, learningWorldVm));
        BusinessLogic.ExecuteCommand(command);
    }
    
    /// <inheritdoc cref="IPresentationLogic.EditPathWayCondition"/>
    public void EditPathWayCondition(PathWayConditionViewModel pathWayConditionVm, ConditionEnum newCondition)
    {
        var pathWayConditionEntity = Mapper.Map<BusinessLogic.Entities.PathWayCondition>(pathWayConditionVm);
        
        var command = new EditPathWayCondition(pathWayConditionEntity, newCondition, 
            condition => CMapper.Map(condition, pathWayConditionVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.DeletePathWayCondition"/>
    public void DeletePathWayCondition(ILearningWorldViewModel learningWorldVm,
        PathWayConditionViewModel pathWayConditionVm)
    {
        var worldEntity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm);
        var pathWayConditionEntity = Mapper.Map<BusinessLogic.Entities.PathWayCondition>(pathWayConditionVm);

        var command = new DeletePathWayCondition(worldEntity, pathWayConditionEntity,
            world => CMapper.Map(world, learningWorldVm));
        BusinessLogic.ExecuteCommand(command);
    }
    
    /// <inheritdoc cref="IPresentationLogic.CreateLearningPathWay"/>
    public void CreateLearningPathWay(ILearningWorldViewModel learningWorldVm, IObjectInPathWayViewModel sourceObjectVm,
        IObjectInPathWayViewModel targetObjectVm)
    {
        var learningWorldEntity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm);
        var sourceObjectEntity = Mapper.Map<BusinessLogic.Entities.IObjectInPathWay>(sourceObjectVm);
        var targetObjectEntity = Mapper.Map<BusinessLogic.Entities.IObjectInPathWay>(targetObjectVm);

        var command = new CreateLearningPathWay(learningWorldEntity, sourceObjectEntity, targetObjectEntity,
            world => CMapper.Map(world, learningWorldVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.DeleteLearningPathWay"/>
    public void DeleteLearningPathWay(ILearningWorldViewModel learningWorldVm, ILearningPathWayViewModel learningPathWayVm)
    {
        var learningWorldEntity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm);
        var learningPathWayEntity = Mapper.Map<BusinessLogic.Entities.LearningPathway>(learningPathWayVm);

        var command = new DeleteLearningPathWay(learningWorldEntity, learningPathWayEntity,
            world => CMapper.Map(world, learningWorldVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.AddLearningElement"/>
    public void AddLearningElement(ILearningSpaceViewModel parentSpaceVm, int slotIndex, ILearningElementViewModel learningElementVm)
    {
        var parentSpaceEntity = Mapper.Map<BusinessLogic.Entities.LearningSpace>(parentSpaceVm);
        var elementEntity = Mapper.Map<BusinessLogic.Entities.LearningElement>(learningElementVm);

        var command = new CreateLearningElementInSlot(parentSpaceEntity, slotIndex, elementEntity, 
            parent => CMapper.Map(parent, parentSpaceVm));
        BusinessLogic.ExecuteCommand(command);
    }

    public void CreateUnplacedLearningElement(ILearningWorldViewModel learningWorldVm, string name, string shortname,
        LearningContentViewModel learningContentVm, string authors, string description, string goals,
        LearningElementDifficultyEnum difficulty, int workload, int points, double positionX = 0, double positionY = 0)
    {
        var learningWorldEntity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm);
        var contentEntity = Mapper.Map<BusinessLogic.Entities.LearningContent.LearningContent>(learningContentVm);
        
        var command = new CreateUnplacedLearningElement(learningWorldEntity, name, shortname, contentEntity, authors, description, goals, difficulty, workload, points, positionX, positionY, 
            world => CMapper.Map(world, learningWorldVm));
        BusinessLogic.ExecuteCommand(command);
    }
    
    /// <inheritdoc cref="IPresentationLogic.CreateLearningElementInSlot"/>
    public void CreateLearningElementInSlot(ILearningSpaceViewModel parentSpaceVm, int slotIndex, string name, string shortname,
        LearningContentViewModel learningContentVm, string authors, string description, string goals,
        LearningElementDifficultyEnum difficulty, int workload, int points, double positionX = 0, double positionY = 0)
    {
        var parentSpaceEntity = Mapper.Map<BusinessLogic.Entities.LearningSpace>(parentSpaceVm);
        var contentEntity = Mapper.Map<BusinessLogic.Entities.LearningContent.LearningContent>(learningContentVm);
        
        //TODO: temporary testing code
        

        var command = new CreateLearningElementInSlot(parentSpaceEntity, slotIndex, name, shortname, contentEntity, authors, description, goals, difficulty, workload, points, positionX, positionY, 
            parent => CMapper.Map(parent, parentSpaceVm));
        BusinessLogic.ExecuteCommand(command);
    } 
    
    /// <inheritdoc cref="IPresentationLogic.EditLearningElement"/>
    public void EditLearningElement(ILearningSpaceViewModel? parentSpaceVm,
        ILearningElementViewModel learningElementVm, string name, string shortname, string authors, string description,
        string goals, LearningElementDifficultyEnum difficulty, int workload, int points, 
        LearningContentViewModel learningContentViewModel)
    {
        var elementEntity = Mapper.Map<BusinessLogic.Entities.LearningElement>(learningElementVm);
        var contentEntity = Mapper.Map<BusinessLogic.Entities.LearningContent.LearningContent>(learningContentViewModel);
        
        BusinessLogic.Entities.LearningSpace? parentSpaceEntity = null;
        if(parentSpaceEntity != null)
        {
            parentSpaceEntity = Mapper.Map<BusinessLogic.Entities.LearningSpace>(parentSpaceVm);
        }

        var command = new EditLearningElement(elementEntity, parentSpaceEntity, name, shortname, authors, description,
            goals, difficulty, workload, points, contentEntity, element => CMapper.Map(element, learningElementVm));
        BusinessLogic.ExecuteCommand(command);
    }

    public void DragLearningElementFromUnplaced(ILearningWorldViewModel learningWorldVm,
        ILearningSpaceViewModel learningSpaceVm, ILearningElementViewModel learningElementVm, int newSlotIndex)
    {
        var worldEntity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm);
        var spaceEntity = Mapper.Map<BusinessLogic.Entities.LearningSpace>(learningSpaceVm);
        var elementEntity = Mapper.Map<BusinessLogic.Entities.LearningElement>(learningElementVm);

        var command = new PlaceLearningElementInLayoutFromUnplaced(worldEntity, spaceEntity, elementEntity, newSlotIndex,
            world => CMapper.Map(world, learningWorldVm));
        BusinessLogic.ExecuteCommand(command);
    }

    public void DragLearningElementToUnplaced(ILearningWorldViewModel learningWorldVm,
        ILearningSpaceViewModel learningSpaceVm, ILearningElementViewModel learningElementVm)
    {
        var worldEntity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm);
        var spaceEntity = Mapper.Map<BusinessLogic.Entities.LearningSpace>(learningSpaceVm);
        var elementEntity = Mapper.Map<BusinessLogic.Entities.LearningElement>(learningElementVm);

        var command = new RemoveLearningElementFromLayout(worldEntity, spaceEntity, elementEntity,
            world => CMapper.Map(world, learningWorldVm));
        BusinessLogic.ExecuteCommand(command);
    }

    public void SwitchLearningElementSlot(ILearningSpaceViewModel learningSpaceVm,
        ILearningElementViewModel learningElementVm, int newSlotIndex)
    {
        var spaceEntity = Mapper.Map<BusinessLogic.Entities.LearningSpace>(learningSpaceVm);
        var elementEntity = Mapper.Map<BusinessLogic.Entities.LearningElement>(learningElementVm);

        var command = new PlaceLearningElementInLayoutFromLayout(spaceEntity, elementEntity, newSlotIndex,
            space => CMapper.Map(space, learningSpaceVm));
        BusinessLogic.ExecuteCommand(command);
    }

    public void DragLearningElement(ILearningElementViewModel learningElementVm, double oldPositionX, double oldPositionY)
    {
        var elementEntity = Mapper.Map<BusinessLogic.Entities.LearningElement>(learningElementVm);
        
        var command = new DragLearningElement(elementEntity, oldPositionX, oldPositionY, elementEntity.PositionX,
            elementEntity.PositionY, space => CMapper.Map(space, learningElementVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.DeleteLearningElement"/>
    public void DeleteLearningElement(ILearningSpaceViewModel parentSpaceVm,
        ILearningElementViewModel learningElementVm)
    {
        var elementEntity = Mapper.Map<BusinessLogic.Entities.LearningElement>(learningElementVm);
        var parentSpaceEntity = Mapper.Map<BusinessLogic.Entities.LearningSpace>(parentSpaceVm);

        var command = new DeleteLearningElement(elementEntity, parentSpaceEntity, 
            parent => CMapper.Map(parent, parentSpaceVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.SaveLearningElementAsync"/>
    public async Task SaveLearningElementAsync(LearningElementViewModel learningElementViewModel)
    {
        ElectronCheck();
        var filepath =
            await GetSaveFilepathAsync("Save Learning Element", ElementFileEnding, ElementFileFormatDescriptor);
        var elementEntity = Mapper.Map<BusinessLogic.Entities.LearningElement>(learningElementViewModel);
        var command = new SaveLearningElement(BusinessLogic, elementEntity, filepath);
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.LoadLearningElementAsync"/>
    public async Task LoadLearningElementAsync(ILearningSpaceViewModel parentSpaceVm, int slotIndex)
    {
        ElectronCheck();
        var filepath =
            await GetLoadFilepathAsync("Load Learning Element", ElementFileEnding, ElementFileFormatDescriptor);
        var parentSpaceEntity = Mapper.Map<BusinessLogic.Entities.LearningSpace>(parentSpaceVm);
        var command = new LoadLearningElement(parentSpaceEntity, slotIndex, filepath, BusinessLogic,
            parent => CMapper.Map(parent, parentSpaceVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <inheritdoc cref="IPresentationLogic.ShowLearningElementContentAsync"/>
    public async Task ShowLearningElementContentAsync(LearningElementViewModel learningElementVm)
    {
        ElectronCheck();
        try
        {
            await ShowLearningContentAsync(learningElementVm.LearningContent);
        }
        catch (ArgumentOutOfRangeException)
        {
            throw new ArgumentOutOfRangeException(nameof(learningElementVm),
                "LearningElementViewModel.LearningContent is not of type FileContentViewModel or LinkContentViewModel");
        }
    }

    public async Task ShowLearningContentAsync(ILearningContentViewModel content)
    {
        ElectronCheck();
        var error =
            await (content switch
            {
                FileContentViewModel fileContentVm => ShellWrapper.OpenPathAsync(fileContentVm.Filepath),
                LinkContentViewModel linkContentVm => ShellWrapper.OpenExternalAsync(linkContentVm.Link),
                _ => throw new ArgumentOutOfRangeException(nameof(content),
                    "LearningContent is not of type FileContentViewModel or LinkContentViewModel")
            });
        
        if (error != "")
        {
            _logger.LogError("Could not open file in OS viewer: {Error}", error);
        }
    }

    /// <inheritdoc cref="IPresentationLogic.LoadImageAsync"/>
    public async Task<LearningContentViewModel> LoadImageAsync()
    {
        ElectronCheck();
        var fileFilter = new FileFilterProxy[] {new(" ", _imageFileEnding)};
        var filepath = await GetLoadFilepathAsync("Load image", fileFilter);
        var entity = BusinessLogic.LoadLearningContent(filepath);
        return Mapper.Map<LearningContentViewModel>(entity);
    }
        
    /// <inheritdoc cref="IPresentationLogic.LoadVideoAsync"/>
    public async Task<LearningContentViewModel> LoadVideoAsync()
    {
        ElectronCheck();
        var filepath = await GetLoadFilepathAsync("Load video", VideoFileEnding, " ");
        var entity = BusinessLogic.LoadLearningContent(filepath);
        return Mapper.Map<LearningContentViewModel>(entity);
    }
        
    /// <inheritdoc cref="IPresentationLogic.LoadH5PAsync"/>
    public async Task<LearningContentViewModel> LoadH5PAsync()
    {
        ElectronCheck();
        var filepath = await GetLoadFilepathAsync("Load h5p",H5PFileEnding, " ");
        var entity = BusinessLogic.LoadLearningContent(filepath);
        return Mapper.Map<LearningContentViewModel>(entity);
    }
        
    /// <inheritdoc cref="IPresentationLogic.LoadPdfAsync"/>
    public async Task<LearningContentViewModel> LoadPdfAsync()
    {
        ElectronCheck();
        var filepath = await GetLoadFilepathAsync("Load pdf",PdfFileEnding, " ");
        var entity = BusinessLogic.LoadLearningContent(filepath);
        return Mapper.Map<LearningContentViewModel>(entity);
    }
    
    /// <inheritdoc cref="IPresentationLogic.LoadTextAsync"/>
    public async Task<LearningContentViewModel> LoadTextAsync()
    {
        ElectronCheck();
        var fileFilter = new FileFilterProxy[] {new(" ", _textFileEnding)};
        var filepath = await GetLoadFilepathAsync("Load text", fileFilter);
        var entity = BusinessLogic.LoadLearningContent(filepath);
        return Mapper.Map<LearningContentViewModel>(entity);
    }

    /// <inheritdoc cref="IPresentationLogic.GetAllContent"/>
    public IEnumerable<LearningContentViewModel> GetAllContent() =>
        BusinessLogic.GetAllContent().Select(Mapper.Map<LearningContentViewModel>);

    /// <inheritdoc cref="IPresentationLogic.RemoveContent"/>
    public void RemoveContent(LearningContentViewModel content) =>
        BusinessLogic.RemoveContent(Mapper.Map<BusinessLogic.Entities.LearningContent.LearningContent>(content));

    /// <inheritdoc cref="IPresentationLogic.SaveLink"/>
    public void SaveLink(LinkContentViewModel linkContentVm) =>
        BusinessLogic.SaveLink(Mapper.Map<LinkContent>(linkContentVm));

    public void OpenContentFilesFolder()
    {
        ElectronCheck();
        var path = BusinessLogic.GetContentFilesFolderPath();
        ShellWrapper.OpenPathAsync(path);
    }

    public void LoadLearningWorldViewModel(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm, Stream stream)
    {
        var workspaceEntity = Mapper.Map<BusinessLogic.Entities.AuthoringToolWorkspace>(authoringToolWorkspaceVm);
        var command = new LoadLearningWorld(workspaceEntity, stream, BusinessLogic,
            workspace => CMapper.Map(workspace, authoringToolWorkspaceVm));
        BusinessLogic.ExecuteCommand(command);
    }

    public void LoadLearningSpaceViewModel(ILearningWorldViewModel learningWorldVm, Stream stream)
    {
        var worldEntity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm);
        var command = new LoadLearningSpace(worldEntity, stream, BusinessLogic,
            world => CMapper.Map(world, learningWorldVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.LoadLearningElementViewModel"/>
    public void LoadLearningElementViewModel(ILearningSpaceViewModel parentSpaceVm, int slotIndex, Stream stream)
    {
        var parentSpaceEntity = Mapper.Map<BusinessLogic.Entities.LearningSpace>(parentSpaceVm);
        var command =
            new LoadLearningElement(parentSpaceEntity, slotIndex, stream, BusinessLogic,
                parent => CMapper.Map(parent, parentSpaceVm));
        BusinessLogic.ExecuteCommand(command);
    }
    
    public LearningContentViewModel LoadLearningContentViewModel(string name, Stream stream)
    {
        var entity = BusinessLogic.LoadLearningContent(name, stream);
        return Mapper.Map<LearningContentViewModel>(entity);
    }

    /// <summary>
    /// Gets Save Filepath for saving.
    /// </summary>
    /// <param name="title">Title of the dialog.</param>
    /// <param name="fileEnding">File ending for the file.</param>
    /// <param name="fileFormatDescriptor"></param>
    /// <returns>Path to the file in which the object should be saved.</returns>
    /// <exception cref="OperationCanceledException">Operation was cancelled by user.</exception>
    private async Task<string> GetSaveFilepathAsync(string title, string fileEnding, string fileFormatDescriptor)
    {
        var filepath = await GetSaveFilepathAsync(title, new FileFilterProxy[]
        {
            new(fileFormatDescriptor, new[] {fileEnding})
        });
        if (!filepath.EndsWith($".{fileEnding}")) filepath += $".{fileEnding}";
        return filepath;
    }

    private async Task<string> GetSaveFilepathAsync(string title, FileFilterProxy[] fileFilterProxies)
    {
        try
        {
            var filepath = await _dialogManager!.ShowSaveAsDialogAsync(title, null, fileFilterProxies);
            return filepath;
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Save as dialog cancelled by user");
            throw;
        }
    }

    /// <summary>
    /// Gets Load Filepath for loading.
    /// </summary>
    /// <param name="title">Title of the dialog.</param>
    /// <param name="fileEnding">File ending for the file.</param>
    /// <param name="fileFormatDescriptor"></param>
    /// <returns>Path to the file which should be loaded.</returns>
    /// <exception cref="OperationCanceledException">Operation was cancelled by user.</exception>
    private async Task<string> GetLoadFilepathAsync(string title, string fileEnding, string fileFormatDescriptor)
    {
        var filepath = await GetLoadFilepathAsync(title, new FileFilterProxy[]
        {
            new(fileFormatDescriptor, new[] {fileEnding})
        });
        if (!filepath.EndsWith($".{fileEnding}")) filepath += $".{fileEnding}";
        return filepath;
    }

    private async Task<string> GetLoadFilepathAsync(string title, FileFilterProxy[] fileFilterProxies)
    {
        try
        {
            var filepath = await _dialogManager!.ShowOpenFileDialogAsync(title, null, fileFilterProxies);
            return filepath;
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Load dialog cancelled by user");
            throw;
        }
    }

    /// <summary>
    /// Performs sanity checks regarding Electron presence.
    /// </summary>
    /// <exception cref="NotImplementedException">Thrown when we are not running in Electron.</exception>
    /// <exception cref="InvalidOperationException">Thrown when we are running in Electron but no <see cref="IElectronDialogManager"/>
    /// implementation is present in dependency injection container.</exception>
    private void ElectronCheck()
    {
        if (!RunningElectron)
            throw new NotImplementedException("Browser upload/download not yet implemented");
        if (_dialogManager == null)
            throw new InvalidOperationException("dialogManager received from DI unexpectedly null");
    }

    public void CallExport()
    {
        BusinessLogic.CallExport();
    }
}