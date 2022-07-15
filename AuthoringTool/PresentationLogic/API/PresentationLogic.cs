using AuthoringTool.API.Configuration;
using AuthoringTool.BusinessLogic.API;
using AuthoringTool.PresentationLogic.ElectronNET;
using AuthoringTool.PresentationLogic.EntityMapping;
using AuthoringTool.PresentationLogic.EntityMapping.LearningElementMapper;
using AuthoringTool.PresentationLogic.LearningContent;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.LearningWorld;

namespace AuthoringTool.PresentationLogic.API;

internal class PresentationLogic : IPresentationLogic
{
    public PresentationLogic(
        IAuthoringToolConfiguration configuration,
        IBusinessLogic businessLogic,
        ILearningWorldMapper worldMapper,
        ILearningSpaceMapper spaceMapper,
        ILearningElementMapper elementMapper,
        ILearningContentMapper contentMapper,
        IServiceProvider serviceProvider,
        ILogger<PresentationLogic> logger)
    {
        _logger = logger;
        Configuration = configuration;
        BusinessLogic = businessLogic;
        WorldMapper = worldMapper;
        SpaceMapper = spaceMapper;
        ElementMapper = elementMapper;
        ContentMapper = contentMapper;
        _dialogManager = serviceProvider.GetService(typeof(IElectronDialogManager)) as IElectronDialogManager;
    }

    private readonly ILogger<PresentationLogic> _logger;
    private readonly IElectronDialogManager? _dialogManager;

    private const string WorldFileEnding = "awf";
    private const string SpaceFileEnding = "asf";
    private const string ElementFileEnding = "aef";
    private string[] ImageFileEnding = {"jpg", "png", "webp", "bmp"};
    private const string VideoFileEnding = "mp4";
    private const string H5PFileEnding = "h5p";
    private const string PdfFileEnding = "pdf";
    private const string WorldFileFormatDescriptor = "AdLer World File";
    private const string SpaceFileFormatDescriptor = "AdLer Space File";
    private const string ElementFileFormatDescriptor = "AdLer Element File";

    public IAuthoringToolConfiguration Configuration { get; }
    public IBusinessLogic BusinessLogic { get; }
    public bool RunningElectron => BusinessLogic.RunningElectron;
    public ILearningWorldMapper WorldMapper { get; }
    public ILearningSpaceMapper SpaceMapper { get; }
    public ILearningElementMapper ElementMapper { get; }
    public ILearningContentMapper ContentMapper { get; }

    public async Task<string> ConstructBackupAsync(LearningWorldViewModel learningWorldViewModel)
    {
        var entity = WorldMapper.ToEntity(learningWorldViewModel);
        var filepath = await GetSaveFilepathAsync("Export learning world", "mbz", "Moodle Backup Zip");
        BusinessLogic.ConstructBackup(entity, filepath);
        return filepath;
    }

    /// <inheritdoc cref="IPresentationLogic.SaveLearningWorldAsync"/>
    public async Task SaveLearningWorldAsync(LearningWorldViewModel learningWorldViewModel)
    {
        SaveOrLoadElectronCheck();
        var filepath = await GetSaveFilepathAsync("Save Learning World", WorldFileEnding, WorldFileFormatDescriptor);
        var worldEntity = WorldMapper.ToEntity(learningWorldViewModel);
        BusinessLogic.SaveLearningWorld(worldEntity, filepath);
        learningWorldViewModel.UnsavedChanges = false;
    }

    /// <inheritdoc cref="IPresentationLogic.LoadLearningWorldAsync"/>
    public async Task<LearningWorldViewModel> LoadLearningWorldAsync()
    {
        SaveOrLoadElectronCheck();
        var filepath = await GetLoadFilepathAsync("Load Learning World", WorldFileEnding, WorldFileFormatDescriptor);
        var entity = BusinessLogic.LoadLearningWorld(filepath);
        return WorldMapper.ToViewModel(entity);
    }

    /// <inheritdoc cref="IPresentationLogic.SaveLearningSpaceAsync"/>
    public async Task SaveLearningSpaceAsync(LearningSpaceViewModel learningSpaceViewModel)
    {
        SaveOrLoadElectronCheck();
        var filepath = await GetSaveFilepathAsync("Save Learning Space", SpaceFileEnding, SpaceFileFormatDescriptor);
        var spaceEntity = SpaceMapper.ToEntity(learningSpaceViewModel);
        BusinessLogic.SaveLearningSpace(spaceEntity, filepath);
    }

    /// <inheritdoc cref="IPresentationLogic.LoadLearningSpaceAsync"/>
    public async Task<ILearningSpaceViewModel> LoadLearningSpaceAsync()
    {
        SaveOrLoadElectronCheck();
        var filepath = await GetLoadFilepathAsync("Load Learning Space", SpaceFileEnding, SpaceFileFormatDescriptor);
        var entity = BusinessLogic.LoadLearningSpace(filepath);
        return SpaceMapper.ToViewModel(entity);
    }

    /// <inheritdoc cref="IPresentationLogic.SaveLearningElementAsync"/>
    public async Task SaveLearningElementAsync(LearningElementViewModel learningElementViewModel)
    {
        SaveOrLoadElectronCheck();
        var filepath =
            await GetSaveFilepathAsync("Save Learning Element", ElementFileEnding, ElementFileFormatDescriptor);
        var elementEntity = ElementMapper.ToEntity(learningElementViewModel);
        BusinessLogic.SaveLearningElement(elementEntity, filepath);
    }

    /// <inheritdoc cref="IPresentationLogic.LoadLearningElementAsync"/>
    public async Task<LearningElementViewModel> LoadLearningElementAsync()
    {
        SaveOrLoadElectronCheck();
        var filepath =
            await GetLoadFilepathAsync("Load Learning Element", ElementFileEnding, ElementFileFormatDescriptor);
        var entity = BusinessLogic.LoadLearningElement(filepath);
        return ElementMapper.ToViewModel(entity);
    }

    /// <inheritdoc cref="IPresentationLogic.LoadImageAsync"/>
    public async Task<LearningContentViewModel> LoadImageAsync()
    {
        SaveOrLoadElectronCheck();
        var fileFilter = new FileFilterProxy[] {new(" ", ImageFileEnding)};
        var filepath = await GetLoadFilepathAsync("Load image", fileFilter);
        var entity = BusinessLogic.LoadLearningContent(filepath);
        return ContentMapper.ToViewModel(entity);
    }
        
    /// <inheritdoc cref="IPresentationLogic.LoadVideoAsync"/>
    public async Task<LearningContentViewModel> LoadVideoAsync()
    {
        SaveOrLoadElectronCheck();
        var filepath = await GetLoadFilepathAsync("Load video", VideoFileEnding, " ");
        var entity = BusinessLogic.LoadLearningContent(filepath);
        return ContentMapper.ToViewModel(entity);
    }
        
    /// <inheritdoc cref="IPresentationLogic.LoadH5pAsync"/>
    public async Task<LearningContentViewModel> LoadH5pAsync()
    {
        SaveOrLoadElectronCheck();
        var filepath = await GetLoadFilepathAsync("Load h5p",H5PFileEnding, " ");
        var entity = BusinessLogic.LoadLearningContent(filepath);
        return ContentMapper.ToViewModel(entity);
    }
        
    /// <inheritdoc cref="IPresentationLogic.LoadPdfAsync"/>
    public async Task<LearningContentViewModel> LoadPdfAsync()
    {
        SaveOrLoadElectronCheck();
        var filepath = await GetLoadFilepathAsync("Load pdf",PdfFileEnding, " ");
        var entity = BusinessLogic.LoadLearningContent(filepath);
        return ContentMapper.ToViewModel(entity);
    }

    public LearningWorldViewModel LoadLearningWorldViewModelFromStream(Stream stream)
    {
        var world = BusinessLogic.LoadLearningWorldFromStream(stream);
        return WorldMapper.ToViewModel(world);
    }

    public ILearningSpaceViewModel LoadLearningSpaceViewModelFromStream(Stream stream)
    {
        var space = BusinessLogic.LoadLearningSpaceFromStream(stream);
        return SpaceMapper.ToViewModel(space);
    }

    public LearningElementViewModel LoadLearningElementViewModelFromStream(Stream stream)
    {
        var element = BusinessLogic.LoadLearningElementFromStream(stream);
        return ElementMapper.ToViewModel(element);
    }
    
    public LearningContentViewModel LoadLearningContentViewModelFromStream(string name, Stream stream)
    {
        var entity = BusinessLogic.LoadLearningContentFromStream(name, stream);
        return ContentMapper.ToViewModel(entity);
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
        if (!BusinessLogic.RunningElectron)
            throw new NotImplementedException("Browser upload/download not yet implemented");
        if (_dialogManager == null)
            throw new InvalidOperationException("dialogManager received from DI unexpectedly null");
    }
}