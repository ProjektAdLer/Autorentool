using AuthoringTool.API.Configuration;
using AuthoringTool.BusinessLogic.API;
using AuthoringTool.PresentationLogic.ElectronNET;
using AuthoringTool.PresentationLogic.EntityMapping;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.LearningWorld;
using AuthoringTool.View.LearningWorld;

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
            _dialogManager = serviceProvider.GetService(typeof(IElectronDialogManager)) as IElectronDialogManager;
        }

        private readonly ILogger<PresentationLogic> _logger;
        private readonly IElectronDialogManager? _dialogManager;
        
        private const string WorldFileEnding = "awf";
        private const string SpaceFileEnding = "asf";
        private const string ElementFileEnding = "aef";
        private const string WorldFileFormatDescriptor = "AdLer World File";
        private const string SpaceFileFormatDescriptor = "AdLer Space File";
        private const string ElementFileFormatDescriptor = "AdLer Element File";

        public IAuthoringToolConfiguration Configuration { get; }
        public IBusinessLogic BusinessLogic { get; }
        public ILearningWorldMapper WorldMapper { get; }
        public ILearningSpaceMapper SpaceMapper { get; }
        public ILearningElementMapper ElementMapper { get; }

        public void ConstructBackup()
        {
            BusinessLogic.ConstructBackup();
        }

        /// <inheritdoc cref="IPresentationLogic.SaveLearningWorldAsync"/>
        public async Task SaveLearningWorldAsync(LearningWorldViewModel learningWorldViewModel)
        {
            await SaveGenericAsync(learningWorldViewModel);
        }

        /// <inheritdoc cref="IPresentationLogic.LoadLearningWorldAsync"/>
        public async Task<LearningWorldViewModel> LoadLearningWorldAsync()
        {
            return await LoadGenericAsync<LearningWorldViewModel>();
        }

        /// <inheritdoc cref="IPresentationLogic.SaveLearningSpaceAsync"/>
        public async void SaveLearningSpaceAsync(LearningSpaceViewModel learningSpaceViewModel)
        {
            await SaveGenericAsync(learningSpaceViewModel);
        }

        /// <inheritdoc cref="IPresentationLogic.LoadLearningSpaceAsync"/>
        public async Task<LearningSpaceViewModel> LoadLearningSpaceAsync()
        {
            return await LoadGenericAsync<LearningSpaceViewModel>();
        }

        /// <inheritdoc cref="IPresentationLogic.SaveLearningElementAsync"/>
        public async void SaveLearningElementAsync(LearningElementViewModel learningElementViewModel)
        {
            await SaveGenericAsync(learningElementViewModel);
        }

        /// <inheritdoc cref="IPresentationLogic.LoadLearningElementAsync"/>
        public async Task<LearningElementViewModel> LoadLearningElementAsync()
        {
            return await LoadGenericAsync<LearningElementViewModel>();
        }
        
        /// <summary>
        /// Generically asks user for path and saves object <see cref="obj"/> of type T
        /// </summary>
        /// <param name="obj">The object which should be saved.</param>
        /// <typeparam name="T">Type of the object <see cref="obj"/></typeparam>
        /// <exception cref="ArgumentException">The given type T is not supported.</exception>
        /// <exception cref="OperationCanceledException">Operation was cancelled by user.</exception>
        /// <exception cref="NotImplementedException">Thrown when we are not running in Electron.</exception>
        /// <exception cref="InvalidOperationException">Thrown when we are running in Electron but no <see cref="IElectronDialogManager"/>
        /// implementation is present in dependency injection container.</exception>
        private async Task SaveGenericAsync<T>(T obj) where T : ISerializableViewModel
        {
            SaveOrLoadElectronCheck();
            var filepath = await GetSaveFilepathAsync(obj);

            switch (obj)
            {
                case LearningWorldViewModel world:
                {
                    var worldEntity = WorldMapper.ToEntity(world);
                    BusinessLogic.SaveLearningWorld(worldEntity, filepath);
                    break;
                }
                case LearningSpaceViewModel space:
                {
                    var spaceEntity = SpaceMapper.ToEntity(space);
                    BusinessLogic.SaveLearningSpace(spaceEntity, filepath);
                    break;
                }
                case LearningElementViewModel element:
                {
                    var elementEntity = ElementMapper.ToEntity(element);
                    BusinessLogic.SaveLearningElement(elementEntity, filepath);
                    break;
                }
            }
        }
        /// <summary>
        /// Generically asks user for path and loads object of type T
        /// </summary>
        /// <typeparam name="T">The type of the object which should be loaded.</typeparam>
        /// <returns>A deserialized object of type T.</returns>
        /// <exception cref="ArgumentException">The given type T is not supported.</exception>
        /// <exception cref="OperationCanceledException">Operation was cancelled by user.</exception>
        /// <exception cref="NotImplementedException">Thrown when we are not running in Electron.</exception>
        /// <exception cref="InvalidOperationException">Thrown when we are running in Electron but no <see cref="IElectronDialogManager"/>
        /// implementation is present in dependency injection container.</exception>
        private async Task<T> LoadGenericAsync<T>() where T : class, ISerializableViewModel
        {
            SaveOrLoadElectronCheck();
            var filepath = await GetLoadFilepathAsync<T>();

            if (typeof(T) == typeof(LearningWorldViewModel))
            {
                var entity = BusinessLogic.LoadLearningWorld(filepath);
                var viewModel = WorldMapper.ToViewModel(entity);
                return (viewModel as T)!;
            }

            if (typeof(T) == typeof(LearningSpaceViewModel))
            {
                var entity = BusinessLogic.LoadLearningSpace(filepath);
                var viewModel = SpaceMapper.ToViewModel(entity);
                return (viewModel as T)!;
            }

            if (typeof(T) == typeof(LearningElementViewModel))
            {
                var entity = BusinessLogic.LoadLearningElement(filepath);
                var viewModel = ElementMapper.ToViewModel(entity);
                return (viewModel as T)!;
            }

            throw new ArgumentException($"{typeof(T)} not allowed for LoadGenericAsync");
        }

        /// <summary>
        /// Generically gets Save Filepath for new File corresponding to type T and object <see cref="obj"/>.
        /// </summary>
        /// <param name="obj">The object for which a filepath should be gotten.</param>
        /// <typeparam name="T">The type of the object <see cref="obj"/>.</typeparam>
        /// <returns>Path to the file in which the object should be saved.</returns>
        /// <exception cref="ArgumentException">The given type T is not supported.</exception>
        /// <exception cref="OperationCanceledException">Operation was cancelled by user.</exception>
        private async Task<string> GetSaveFilepathAsync<T>(T obj) where T : ISerializableViewModel
        {
            try
            {
                string UnallowedTypeException()
                {
                    throw new ArgumentException($"Type {typeof(T).Name} not allowed for GetLoadFilepathAsync.");
                }

                var title = obj switch
                {
                    LearningWorldViewModel => "Save learning world",
                    LearningSpaceViewModel => "Save learning space",
                    LearningElementViewModel => "Save learning element",
                    _ => UnallowedTypeException()
                };
                var fileEnding = obj switch
                {
                    LearningWorldViewModel => WorldFileEnding,
                    LearningSpaceViewModel => SpaceFileEnding,
                    LearningElementViewModel => ElementFileEnding,
                    _ => UnallowedTypeException()
                };
                var fileFormatDescriptor = obj switch
                {
                    LearningWorldViewModel => WorldFileFormatDescriptor,
                    LearningSpaceViewModel => SpaceFileFormatDescriptor,
                    LearningElementViewModel => ElementFileFormatDescriptor,
                    _ => UnallowedTypeException()
                };
                var filepath = await _dialogManager!.ShowSaveAsDialog(title, null, new FileFilterProxy[]
                {
                    new(fileFormatDescriptor, new[] { fileEnding })
                });
                if (!filepath.EndsWith($".{fileEnding}")) filepath += $".{fileEnding}";
                return filepath;
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("SaveAs operation in SaveLearningWorld cancelled by user");
                throw;
            }
            catch (ArgumentException)
            {
                _logger.LogCritical("Unknown parameter type {TypeName} in GetSaveFilepathAsync", typeof(T).Name);
                throw;
            }
        }

        /// <summary>
        /// Generically gets Load Filepath for File corresponding to type T.
        /// </summary>
        /// <typeparam name="T">The type for which a Load File dialog should be opened.</typeparam>
        /// <returns>Path to the file which should be loaded.</returns>
        /// <exception cref="ArgumentException">The given type T is not supported.</exception>
        /// <exception cref="OperationCanceledException">Operation was cancelled by user.</exception>
        private async Task<string> GetLoadFilepathAsync<T>() where T : ISerializableViewModel
        {
            try
            {
                var title = "";
                var fileEnding = "";
                var fileFormatDescriptor = "";
                if (typeof(T) == typeof(LearningWorldViewModel))
                {
                    title = "Load Learning World";
                    fileEnding = WorldFileEnding;
                    fileFormatDescriptor = WorldFileFormatDescriptor;
                }
                else if (typeof(T) == typeof(LearningSpaceViewModel))
                {
                    title = "Load Learning Space";
                    fileEnding = SpaceFileEnding;
                    fileFormatDescriptor = SpaceFileFormatDescriptor;
                }
                else if (typeof(T) == typeof(LearningElementViewModel))
                {
                    title = "Load Learning Element";
                    fileEnding = ElementFileEnding;
                    fileFormatDescriptor = ElementFileFormatDescriptor;
                }
                else
                {
                    _logger.LogCritical("Unknown parameter type {TypeName} in GetLoadFilepathAsync", typeof(T).Name);
                    throw new ArgumentException($"Type {typeof(T).Name} not allowed for GetLoadFilepathAsync.");
                }
                var filepath = await _dialogManager!.ShowOpenFileDialog(title, null, new FileFilterProxy[]
                {
                    new(fileFormatDescriptor, new[] { fileEnding })
                });
                if (!filepath.EndsWith($".{fileEnding}")) filepath += $".{fileEnding}";
                return filepath;
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("SaveAs operation in SaveLearningWorld cancelled by user");
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
            if (!BusinessLogic.RunningElectron) throw new NotImplementedException("Browser upload/download not yet implemented");
            if (_dialogManager == null) throw new InvalidOperationException("dialogManager received from DI unexpectedly null");
        }
    }
}