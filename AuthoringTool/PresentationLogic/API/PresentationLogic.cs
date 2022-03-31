using AuthoringTool.API.Configuration;
using AuthoringTool.BusinessLogic.API;
using AuthoringTool.PresentationLogic.ElectronNET;
using AuthoringTool.PresentationLogic.EntityMapping;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.LearningWorld;

namespace AuthoringTool.PresentationLogic.API
{
    internal class PresentationLogic : IPresentationLogic
    {
        public PresentationLogic(
            IAuthoringToolConfiguration configuration,
            IBusinessLogic businessLogic,
            ILearningWorldMapper worldMapper,
            ILearningSpaceMapper spaceMapper,
            ILearningElementMapper elementMapper,
            IServiceProvider serviceProvider,
            ILogger<PresentationLogic> logger)
        {
            _logger = logger;
            Configuration = configuration;
            BusinessLogic = businessLogic;
            WorldMapper = worldMapper;
            SpaceMapper = spaceMapper;
            ElementMapper = elementMapper;
            _dialogManager = serviceProvider.GetService(typeof(ElectronDialogManager)) as ElectronDialogManager;
        }

        private readonly ILogger<PresentationLogic> _logger;
        private ElectronDialogManager? _dialogManager;

        public IAuthoringToolConfiguration Configuration { get; }
        public IBusinessLogic BusinessLogic { get; }
        public ILearningWorldMapper WorldMapper { get; }
        public ILearningSpaceMapper SpaceMapper { get; }
        public ILearningElementMapper ElementMapper { get; }

        public void ConstructBackup()
        {
            BusinessLogic.ConstructBackup();
        }

        public async void SaveLearningWorld(LearningWorldViewModel learningWorldViewModel)
        {
            if (BusinessLogic.RunningElectron)
            {
                if (_dialogManager == null)
                {
                    throw new Exception("dialogManager received from DI unexpectedly null");
                }

                string filepath;
                try
                {
                    filepath = await _dialogManager.ShowSaveAsDialog("Save learning world", null, new FileFilterProxy[]
                    {
                        new("AdLer World File", new []{"awf"})
                    });
                    if (!filepath.EndsWith(".awf")) filepath += ".awf";
                }
                catch (OperationCanceledException e)
                {
                    _logger.LogInformation("SaveAs operation in SaveLearningWorld cancelled by user");
                    throw;
                }
                var worldEntity = WorldMapper.ToEntity(learningWorldViewModel);
                BusinessLogic.SaveLearningWorld(worldEntity, filepath);
            }
            else
            {
                //TODO: copy JsFileSavingService over from Electronize
                throw new NotImplementedException("Browser saving not yet implemented");
            }
        }

        public async Task<LearningWorldViewModel> LoadLearningWorld()
        {
            if (BusinessLogic.RunningElectron)
            {
                if (_dialogManager == null)
                {
                    throw new Exception("dialogManager received from DI unexpectedly null");
                }

                string filepath;
                try
                {
                    filepath = await _dialogManager.ShowOpenFileDialog("Load learning world", null,
                        new FileFilterProxy[]
                        {
                            new("AdLer World File", new[] { "awf" })
                        });
                    var worldEntity = BusinessLogic.LoadLearningWorld(filepath);
                    return WorldMapper.ToViewModel(worldEntity);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            else
            {
                //TODO: look at how to upload files in browser
                throw new NotImplementedException("Browser upload not yet implemented");
            }
        }

        public async void SaveLearningSpace(LearningSpaceViewModel learningSpaceViewModel)
        {
            if (BusinessLogic.RunningElectron)
            {
                if (_dialogManager == null)
                {
                    throw new Exception("dialogManager received from DI unexpectedly null");
                }

                string filepath;
                try
                {
                    filepath = await _dialogManager.ShowSaveAsDialog("Save learning space", null, new FileFilterProxy[]
                    {
                        new("AdLer Space File", new []{"asf"})
                    });
                    if (!filepath.EndsWith(".asf")) filepath += ".asf";
                }
                catch (OperationCanceledException e)
                {
                    _logger.LogInformation("SaveAs operation in SaveLearningSpace cancelled by user");
                    throw;
                }
                var spaceEntity = SpaceMapper.ToEntity(learningSpaceViewModel);
                BusinessLogic.SaveLearningSpace(spaceEntity, filepath);
            }
            else
            {
                //TODO: copy JsFileSavingService over from Electronize
                throw new NotImplementedException("Browser saving not yet implemented");
            }
        }

        public async Task<LearningSpaceViewModel> LoadLearningSpace()
        {
            if (BusinessLogic.RunningElectron)
            {
                if (_dialogManager == null)
                {
                    throw new Exception("dialogManager received from DI unexpectedly null");
                }

                string filepath;
                try
                {
                    filepath = await _dialogManager.ShowOpenFileDialog("Load learning space", null,
                        new FileFilterProxy[]
                        {
                            new("AdLer Space File", new[] { "asf" })
                        });
                    var spaceEntity = BusinessLogic.LoadLearningSpace(filepath);
                    return SpaceMapper.ToViewModel(spaceEntity);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            else
            {
                //TODO: look at how to upload files in browser
                throw new NotImplementedException("Browser upload not yet implemented");
            }
        }

        public async void SaveLearningElement(LearningElementViewModel learningElementViewModel)
        {
            if (BusinessLogic.RunningElectron)
            {
                if (_dialogManager == null)
                {
                    throw new Exception("dialogManager received from DI unexpectedly null");
                }

                string filepath;
                try
                {
                    filepath = await _dialogManager.ShowSaveAsDialog("Save learning element", null, new FileFilterProxy[]
                    {
                        new("AdLer Element File", new []{"aef"})
                    });
                    if (!filepath.EndsWith(".aef")) filepath += ".aef";
                }
                catch (OperationCanceledException e)
                {
                    _logger.LogInformation("SaveAs operation in SaveLearningElement cancelled by user");
                    throw;
                }
                var elementEntity = ElementMapper.ToEntity(learningElementViewModel);
                BusinessLogic.SaveLearningElement(elementEntity, filepath);
            }
            else
            {
                //TODO: copy JsFileSavingService over from Electronize
                throw new NotImplementedException("Browser saving not yet implemented");
            }
        }

        public async Task<LearningElementViewModel> LoadLearningElement()
        {
            if (BusinessLogic.RunningElectron)
            {
                if (_dialogManager == null)
                {
                    throw new Exception("dialogManager received from DI unexpectedly null");
                }

                string filepath;
                try
                {
                    filepath = await _dialogManager.ShowOpenFileDialog("Load learning element", null,
                        new FileFilterProxy[]
                        {
                            new("AdLer Element File", new[] { "aef" })
                        });
                    var elementEntity = BusinessLogic.LoadLearningElement(filepath);
                    return ElementMapper.ToViewModel(elementEntity);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            else
            {
                //TODO: look at how to upload files in browser
                throw new NotImplementedException("Browser upload not yet implemented");
            }
        }
    }
}