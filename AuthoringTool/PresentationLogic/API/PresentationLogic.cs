using AuthoringTool.API.Configuration;
using AuthoringTool.BusinessLogic.API;
using AuthoringTool.PresentationLogic.ElectronNET;
using AuthoringTool.PresentationLogic.EntityMapping;
using AuthoringTool.PresentationLogic.LearningWorld;

namespace AuthoringTool.PresentationLogic.API
{
    internal class PresentationLogic : IPresentationLogic
    {
        public PresentationLogic(
            IAuthoringToolConfiguration configuration,
            IBusinessLogic businessLogic,
            ILearningWorldMapper worldMapper,
            IServiceProvider serviceProvider,
            ILogger<PresentationLogic> logger)
        {
            _logger = logger;
            Configuration = configuration;
            BusinessLogic = businessLogic;
            WorldMapper = worldMapper;
            _dialogManager = serviceProvider.GetService(typeof(ElectronDialogManager)) as ElectronDialogManager;
        }

        private readonly ILogger<PresentationLogic> _logger;
        private ElectronDialogManager? _dialogManager;

        public IAuthoringToolConfiguration Configuration { get; }
        public IBusinessLogic BusinessLogic { get; }
        public ILearningWorldMapper WorldMapper { get; }

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
        
    }
}