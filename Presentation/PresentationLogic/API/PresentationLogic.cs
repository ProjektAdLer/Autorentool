using AutoMapper;
using BusinessLogic.API;
using BusinessLogic.Commands;
using BusinessLogic.Commands.Condition;
using BusinessLogic.Commands.Element;
using BusinessLogic.Commands.Layout;
using BusinessLogic.Commands.Pathway;
using BusinessLogic.Commands.Space;
using BusinessLogic.Commands.World;
using ElectronWrapper;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.ElectronNET;
using Presentation.PresentationLogic.Content;
using Presentation.PresentationLogic.Element;
using Presentation.PresentationLogic.PathWay;
using Presentation.PresentationLogic.Space;
using Presentation.PresentationLogic.World;
using Shared;
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
    public event Action? OnUndoRedoPerformed
    {
        add => BusinessLogic.OnUndoRedoPerformed += value;
        remove => BusinessLogic.OnUndoRedoPerformed -= value;
    }

    public async Task<string> ConstructBackupAsync(WorldViewModel worldViewModel)
    {
        var entity = Mapper.Map<BusinessLogic.Entities.World>(worldViewModel);
        var filepath = await GetSaveFilepathAsync("Export  world", "mbz", "Moodle Backup Zip");
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

    public void AddWorld(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm,
        IWorldViewModel worldVm)
    {
        var authoringToolWorkspaceEntity = Mapper.Map<BusinessLogic.Entities.AuthoringToolWorkspace>(authoringToolWorkspaceVm);
        var worldEntity = Mapper.Map<BusinessLogic.Entities.World>(worldVm);

        var command = new CreateWorld(authoringToolWorkspaceEntity, worldEntity,
            workspace => CMapper.Map(workspace, authoringToolWorkspaceVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.CreateWorld"/>
    public void CreateWorld(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm, string name,
        string shortname, string authors, string language, string description, string goals)
    {
        var authoringToolWorkspaceEntity = Mapper.Map<BusinessLogic.Entities.AuthoringToolWorkspace>(authoringToolWorkspaceVm);

        var command = new CreateWorld(authoringToolWorkspaceEntity, name, shortname, authors, language, description, goals,
            workspace => CMapper.Map(workspace, authoringToolWorkspaceVm));
        BusinessLogic.ExecuteCommand(command);
    }
    
    /// <inheritdoc cref="IPresentationLogic.EditWorld"/>
    public void EditWorld(IWorldViewModel worldVm, string name,
        string shortname, string authors, string language, string description, string goals)
    {
        var worldEntity = Mapper.Map<BusinessLogic.Entities.World>(worldVm);

        var command = new EditWorld(worldEntity, name, shortname, authors, language, description, goals,
            world => CMapper.Map(world, worldVm));
        BusinessLogic.ExecuteCommand(command);
    }
    
    /// <inheritdoc cref="IPresentationLogic.DeleteWorld"/>
    public void DeleteWorld(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm, WorldViewModel worldVm)
    {
        var authoringToolWorkspaceEntity = Mapper.Map<BusinessLogic.Entities.AuthoringToolWorkspace>(authoringToolWorkspaceVm);
        var worldEntity = Mapper.Map<BusinessLogic.Entities.World>(worldVm);

        var command = new DeleteWorld(authoringToolWorkspaceEntity, worldEntity,
            workspace => CMapper.Map(workspace, authoringToolWorkspaceVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.SaveWorldAsync"/>
    public async Task SaveWorldAsync(WorldViewModel worldViewModel)
    {
        SaveOrLoadElectronCheck();
        var filepath = await GetSaveFilepathAsync("Save World", WorldFileEnding, WorldFileFormatDescriptor);
        var worldEntity = Mapper.Map<BusinessLogic.Entities.World>(worldViewModel);
        var command = new SaveWorld(BusinessLogic, worldEntity, filepath);
        BusinessLogic.ExecuteCommand(command);
        worldViewModel.UnsavedChanges = false;
    }

    /// <inheritdoc cref="IPresentationLogic.LoadWorldAsync"/>
    public async Task LoadWorldAsync(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm)
    {
        SaveOrLoadElectronCheck();
        var filepath = await GetLoadFilepathAsync("Load World", WorldFileEnding, WorldFileFormatDescriptor);
        var workspaceEntity = Mapper.Map<BusinessLogic.Entities.AuthoringToolWorkspace>(authoringToolWorkspaceVm);
        var command = new LoadWorld(workspaceEntity, filepath, BusinessLogic,
            workspace => CMapper.Map(workspace, authoringToolWorkspaceVm));
        BusinessLogic.ExecuteCommand(command);
    }

    public void AddSpace(IWorldViewModel worldVm, ISpaceViewModel spaceVm)
    {
        var worldEntity = Mapper.Map<BusinessLogic.Entities.World>(worldVm);
        var spaceEntity = Mapper.Map<BusinessLogic.Entities.Space>(spaceVm);

        var command = new CreateSpace(worldEntity, spaceEntity, 
            world => CMapper.Map(world, worldVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.CreateSpace"/>
    public void CreateSpace(IWorldViewModel worldVm, string name, string shortname,
        string authors, string description, string goals, int requiredPoints, double positionX, double positionY)
    {
        var worldEntity = Mapper.Map<BusinessLogic.Entities.World>(worldVm);

        var command = new CreateSpace(worldEntity, name, shortname, authors, description, goals, requiredPoints, 
            positionX, positionY, world => CMapper.Map(world, worldVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.EditSpace"/>
    public void EditSpace(ISpaceViewModel spaceVm, string name,
        string shortname, string authors, string description, string goals, int requiredPoints)
    {
        var spaceEntity = Mapper.Map<BusinessLogic.Entities.Space>(spaceVm);

        var command = new EditSpace(spaceEntity, name, shortname, authors, description, goals, requiredPoints,
            space => CMapper.Map(space, spaceVm));
        BusinessLogic.ExecuteCommand(command);
    }
    
    /// <inheritdoc cref="IPresentationLogic.ChangeSpaceLayout"/>
    public void ChangeSpaceLayout(ISpaceViewModel spaceVm, FloorPlanEnum floorPlanName)
    {
        var spaceEntity = Mapper.Map<BusinessLogic.Entities.Space>(spaceVm);

        var command = new ChangeSpaceLayout(spaceEntity, floorPlanName,
            space => CMapper.Map(space, spaceVm));
        BusinessLogic.ExecuteCommand(command);
    }
    
    public void DragObjectInPathWay(IObjectInPathWayViewModel objectInPathWayVm , double oldPositionX, double oldPositionY)
    {
        var objectInPathWayEntity = Mapper.Map<BusinessLogic.Entities.IObjectInPathWay>(objectInPathWayVm);
        
        var command = new DragObjectInPathWay(objectInPathWayEntity, oldPositionX, oldPositionY, objectInPathWayEntity.PositionX,
            objectInPathWayEntity.PositionY, space => CMapper.Map(space, objectInPathWayVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.DeleteSpace"/>
    public void DeleteSpace(IWorldViewModel worldVm, ISpaceViewModel spaceVm)
    {
        var worldEntity = Mapper.Map<BusinessLogic.Entities.World>(worldVm);
        var spaceEntity = Mapper.Map<BusinessLogic.Entities.Space>(spaceVm);

        var command = new DeleteSpace(worldEntity, spaceEntity,
            world => CMapper.Map(world, worldVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.SaveSpaceAsync"/>
    public async Task SaveSpaceAsync(SpaceViewModel spaceViewModel)
    {
        SaveOrLoadElectronCheck();
        var filepath = await GetSaveFilepathAsync("Save Space", SpaceFileEnding, SpaceFileFormatDescriptor);
        var spaceEntity = Mapper.Map<BusinessLogic.Entities.Space>(spaceViewModel);
        var command = new SaveSpace(BusinessLogic, spaceEntity, filepath);
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.LoadSpaceAsync"/>
    public async Task LoadSpaceAsync(IWorldViewModel worldVm)
    {
        SaveOrLoadElectronCheck();
        var filepath = await GetLoadFilepathAsync("Load Space", SpaceFileEnding, SpaceFileFormatDescriptor);
        var worldEntity = Mapper.Map<BusinessLogic.Entities.World>(worldVm);
        var command = new LoadSpace(worldEntity, filepath, BusinessLogic, 
            world => CMapper.Map(world, worldVm));
        BusinessLogic.ExecuteCommand(command);
    }
    
    /// <inheritdoc cref="IPresentationLogic.CreatePathWayCondition"/>
    public void CreatePathWayCondition(IWorldViewModel worldVm, ConditionEnum condition, double positionX, double positionY)
    {
        var worldEntity = Mapper.Map<BusinessLogic.Entities.World>(worldVm);
        
        var command = new CreatePathWayCondition(worldEntity, condition, positionX, positionY, 
            world => CMapper.Map(world, worldVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.CreatePathWayConditionBetweenObjects"/>
    public void CreatePathWayConditionBetweenObjects(IWorldViewModel worldVm, ConditionEnum condition,
        IObjectInPathWayViewModel sourceObject, ISpaceViewModel targetObject)
    {
        var worldEntity = Mapper.Map<BusinessLogic.Entities.World>(worldVm);
        var sourceObjectEntity = Mapper.Map<BusinessLogic.Entities.IObjectInPathWay>(sourceObject);
        var targetObjectEntity = Mapper.Map<BusinessLogic.Entities.Space>(targetObject);
        
        var command = new CreatePathWayCondition(worldEntity, condition, sourceObjectEntity, targetObjectEntity, 
            world => CMapper.Map(world, worldVm));
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
    public void DeletePathWayCondition(IWorldViewModel worldVm,
        PathWayConditionViewModel pathWayConditionVm)
    {
        var worldEntity = Mapper.Map<BusinessLogic.Entities.World>(worldVm);
        var pathWayConditionEntity = Mapper.Map<BusinessLogic.Entities.PathWayCondition>(pathWayConditionVm);

        var command = new DeletePathWayCondition(worldEntity, pathWayConditionEntity,
            world => CMapper.Map(world, worldVm));
        BusinessLogic.ExecuteCommand(command);
    }
    
    /// <inheritdoc cref="IPresentationLogic.CreatePathWay"/>
    public void CreatePathWay(IWorldViewModel worldVm, IObjectInPathWayViewModel sourceObjectVm,
        IObjectInPathWayViewModel targetObjectVm)
    {
        var worldEntity = Mapper.Map<BusinessLogic.Entities.World>(worldVm);
        var sourceObjectEntity = Mapper.Map<BusinessLogic.Entities.IObjectInPathWay>(sourceObjectVm);
        var targetObjectEntity = Mapper.Map<BusinessLogic.Entities.IObjectInPathWay>(targetObjectVm);

        var command = new CreatePathWay(worldEntity, sourceObjectEntity, targetObjectEntity,
            world => CMapper.Map(world, worldVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.DeletePathWay"/>
    public void DeletePathWay(IWorldViewModel worldVm, IPathWayViewModel pathWayVm)
    {
        var worldEntity = Mapper.Map<BusinessLogic.Entities.World>(worldVm);
        var pathWayEntity = Mapper.Map<BusinessLogic.Entities.Pathway>(pathWayVm);

        var command = new DeletePathWay(worldEntity, pathWayEntity,
            world => CMapper.Map(world, worldVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.AddElement"/>
    public void AddElement(ISpaceViewModel parentSpaceVm, int slotIndex, IElementViewModel elementVm)
    {
        var parentSpaceEntity = Mapper.Map<BusinessLogic.Entities.Space>(parentSpaceVm);
        var elementEntity = Mapper.Map<BusinessLogic.Entities.Element>(elementVm);

        var command = new CreateElement(parentSpaceEntity, slotIndex, elementEntity, 
            parent => CMapper.Map(parent, parentSpaceVm));
        BusinessLogic.ExecuteCommand(command);
    }
    
    /// <inheritdoc cref="IPresentationLogic.CreateElement"/>
    public void CreateElement(ISpaceViewModel parentSpaceVm, int slotIndex, string name, string shortname,
        ElementTypeEnum elementType, ContentTypeEnum contentType, ContentViewModel contentVm,
        string url, string authors, string description, string goals, ElementDifficultyEnum difficulty,
        int workload, int points, double positionX = 0, double positionY = 0)
    {
        var parentSpaceEntity = Mapper.Map<BusinessLogic.Entities.Space>(parentSpaceVm);
        var contentEntity = Mapper.Map<BusinessLogic.Entities.Content>(contentVm);
        
        //TODO: temporary testing code
        

        var command = new CreateElement(parentSpaceEntity, slotIndex, name, shortname, elementType, contentType, contentEntity,
            url, authors, description, goals, difficulty, workload, points, positionX, positionY, 
            parent => CMapper.Map(parent, parentSpaceVm));
        BusinessLogic.ExecuteCommand(command);
    } 
    
    /// <inheritdoc cref="IPresentationLogic.EditElement"/>
    public void EditElement(ISpaceViewModel parentSpaceVm,
        IElementViewModel elementVm, string name, string shortname, string url, string authors,
        string description, string goals, ElementDifficultyEnum difficulty, int workload, int points)
    {
        var elementEntity = Mapper.Map<BusinessLogic.Entities.Element>(elementVm);
        var parentSpaceEntity = Mapper.Map<BusinessLogic.Entities.Space>(parentSpaceVm);

        var command = new EditElement(elementEntity, parentSpaceEntity, name, shortname, url, authors, description,
            goals, difficulty, workload, points, element => CMapper.Map(element, elementVm));
        BusinessLogic.ExecuteCommand(command);
    }

    public void DragElementFromUnplaced(IWorldViewModel worldVm,
        ISpaceViewModel spaceVm, IElementViewModel elementVm, int newSlotIndex)
    {
        var worldEntity = Mapper.Map<BusinessLogic.Entities.World>(worldVm);
        var spaceEntity = Mapper.Map<BusinessLogic.Entities.Space>(spaceVm);
        var elementEntity = Mapper.Map<BusinessLogic.Entities.Element>(elementVm);

        var command = new PlaceElementInLayoutFromUnplaced(worldEntity, spaceEntity, elementEntity, newSlotIndex,
            world => CMapper.Map(world, worldVm));
        BusinessLogic.ExecuteCommand(command);
    }

    public void DragElementToUnplaced(IWorldViewModel worldVm,
        ISpaceViewModel spaceVm, IElementViewModel elementVm)
    {
        var worldEntity = Mapper.Map<BusinessLogic.Entities.World>(worldVm);
        var spaceEntity = Mapper.Map<BusinessLogic.Entities.Space>(spaceVm);
        var elementEntity = Mapper.Map<BusinessLogic.Entities.Element>(elementVm);

        var command = new RemoveElementFromLayout(worldEntity, spaceEntity, elementEntity,
            world => CMapper.Map(world, worldVm));
        BusinessLogic.ExecuteCommand(command);
    }

    public void SwitchElementSlot(ISpaceViewModel spaceVm,
        IElementViewModel elementVm, int newSlotIndex)
    {
        var spaceEntity = Mapper.Map<BusinessLogic.Entities.Space>(spaceVm);
        var elementEntity = Mapper.Map<BusinessLogic.Entities.Element>(elementVm);

        var command = new PlaceElementInLayoutFromLayout(spaceEntity, elementEntity, newSlotIndex,
            space => CMapper.Map(space, spaceVm));
        BusinessLogic.ExecuteCommand(command);
    }

    public void DragElement(IElementViewModel elementVm, double oldPositionX, double oldPositionY)
    {
        var elementEntity = Mapper.Map<BusinessLogic.Entities.Element>(elementVm);
        
        var command = new DragElement(elementEntity, oldPositionX, oldPositionY, elementEntity.PositionX,
            elementEntity.PositionY, space => CMapper.Map(space, elementVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.DeleteElement"/>
    public void DeleteElement(ISpaceViewModel parentSpaceVm,
        IElementViewModel elementVm)
    {
        var elementEntity = Mapper.Map<BusinessLogic.Entities.Element>(elementVm);
        var parentSpaceEntity = Mapper.Map<BusinessLogic.Entities.Space>(parentSpaceVm);

        var command = new DeleteElement(elementEntity, parentSpaceEntity, 
            parent => CMapper.Map(parent, parentSpaceVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.SaveElementAsync"/>
    public async Task SaveElementAsync(ElementViewModel elementViewModel)
    {
        SaveOrLoadElectronCheck();
        var filepath =
            await GetSaveFilepathAsync("Save Element", ElementFileEnding, ElementFileFormatDescriptor);
        var elementEntity = Mapper.Map<BusinessLogic.Entities.Element>(elementViewModel);
        var command = new SaveElement(BusinessLogic, elementEntity, filepath);
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.LoadElementAsync"/>
    public async Task LoadElementAsync(ISpaceViewModel parentSpaceVm, int slotIndex)
    {
        SaveOrLoadElectronCheck();
        var filepath =
            await GetLoadFilepathAsync("Load Element", ElementFileEnding, ElementFileFormatDescriptor);
        var parentSpaceEntity = Mapper.Map<BusinessLogic.Entities.Space>(parentSpaceVm);
        var command = new LoadElement(parentSpaceEntity, slotIndex, filepath, BusinessLogic,
            parent => CMapper.Map(parent, parentSpaceVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.ShowElementContentAsync"/>
    public Task ShowElementContentAsync(ElementViewModel elementVm)
    {
        SaveOrLoadElectronCheck();
        var filepath = elementVm.Content.Filepath;
        var error = ShellWrapper.OpenPathAsync(filepath).Result;
        if (error != "")
        {
            _logger.LogError(error);
        }
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="IPresentationLogic.LoadImageAsync"/>
    public async Task<ContentViewModel> LoadImageAsync()
    {
        SaveOrLoadElectronCheck();
        var fileFilter = new FileFilterProxy[] {new(" ", _imageFileEnding)};
        var filepath = await GetLoadFilepathAsync("Load image", fileFilter);
        var entity = BusinessLogic.LoadContent(filepath);
        return Mapper.Map<ContentViewModel>(entity);
    }
        
    /// <inheritdoc cref="IPresentationLogic.LoadVideoAsync"/>
    public async Task<ContentViewModel> LoadVideoAsync()
    {
        SaveOrLoadElectronCheck();
        var filepath = await GetLoadFilepathAsync("Load video", VideoFileEnding, " ");
        var entity = BusinessLogic.LoadContent(filepath);
        return Mapper.Map<ContentViewModel>(entity);
    }
        
    /// <inheritdoc cref="IPresentationLogic.LoadH5PAsync"/>
    public async Task<ContentViewModel> LoadH5PAsync()
    {
        SaveOrLoadElectronCheck();
        var filepath = await GetLoadFilepathAsync("Load h5p",H5PFileEnding, " ");
        var entity = BusinessLogic.LoadContent(filepath);
        return Mapper.Map<ContentViewModel>(entity);
    }
        
    /// <inheritdoc cref="IPresentationLogic.LoadPdfAsync"/>
    public async Task<ContentViewModel> LoadPdfAsync()
    {
        SaveOrLoadElectronCheck();
        var filepath = await GetLoadFilepathAsync("Load pdf",PdfFileEnding, " ");
        var entity = BusinessLogic.LoadContent(filepath);
        return Mapper.Map<ContentViewModel>(entity);
    }
    
    /// <inheritdoc cref="IPresentationLogic.LoadTextAsync"/>
    public async Task<ContentViewModel> LoadTextAsync()
    {
        SaveOrLoadElectronCheck();
        var fileFilter = new FileFilterProxy[] {new(" ", _textFileEnding)};
        var filepath = await GetLoadFilepathAsync("Load text", fileFilter);
        var entity = BusinessLogic.LoadContent(filepath);
        return Mapper.Map<ContentViewModel>(entity);
    }

    public void LoadWorldViewModel(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm, Stream stream)
    {
        var workspaceEntity = Mapper.Map<BusinessLogic.Entities.AuthoringToolWorkspace>(authoringToolWorkspaceVm);
        var command = new LoadWorld(workspaceEntity, stream, BusinessLogic,
            workspace => CMapper.Map(workspace, authoringToolWorkspaceVm));
        BusinessLogic.ExecuteCommand(command);
    }

    public void LoadSpaceViewModel(IWorldViewModel worldVm, Stream stream)
    {
        var worldEntity = Mapper.Map<BusinessLogic.Entities.World>(worldVm);
        var command = new LoadSpace(worldEntity, stream, BusinessLogic,
            world => CMapper.Map(world, worldVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.LoadElementViewModel"/>
    public void LoadElementViewModel(ISpaceViewModel parentSpaceVm, int slotIndex, Stream stream)
    {
        var parentSpaceEntity = Mapper.Map<BusinessLogic.Entities.Space>(parentSpaceVm);
        var command =
            new LoadElement(parentSpaceEntity, slotIndex, stream, BusinessLogic,
                parent => CMapper.Map(parent, parentSpaceVm));
        BusinessLogic.ExecuteCommand(command);
    }
    
    public ContentViewModel LoadContentViewModel(string name, MemoryStream stream)
    {
        var entity = BusinessLogic.LoadContent(name, stream);
        return Mapper.Map<ContentViewModel>(entity);
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
    private void SaveOrLoadElectronCheck()
    {
        if (!RunningElectron)
            throw new NotImplementedException("Browser upload/download not yet implemented");
        if (_dialogManager == null)
            throw new InvalidOperationException("dialogManager received from DI unexpectedly null");
    }
}