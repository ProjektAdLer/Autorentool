using AuthoringTool.Components.ModalDialog;
using AuthoringTool.PresentationLogic.API;
using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.LearningWorld;
using AuthoringTool.PresentationLogic.LearningElement;

namespace AuthoringTool.PresentationLogic.AuthoringToolWorkspace
{
    public class AuthoringToolWorkspacePresenter
    {
        public AuthoringToolWorkspacePresenter(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm,
            IPresentationLogic presentationLogic,
            ILearningWorldPresenter learningWorldPresenter, ILearningSpacePresenter learningSpacePresenter,
            ILearningElementPresenter learningElementPresenter, ILogger<AuthoringToolWorkspacePresenter> logger)
        {
            _learningSpacePresenter = learningSpacePresenter;
            _learningElementPresenter = learningElementPresenter;
            _learningWorldPresenter = learningWorldPresenter;
            _authoringToolWorkspaceVm = authoringToolWorkspaceVm;
            _presentationLogic = presentationLogic;
            _logger = logger;
            CreateLearningWorldDialogOpen = false;
            EditLearningWorldDialogOpen = false;
            CreateLearningSpaceDialogueOpen = false;
            EditLearningSpaceDialogOpen = false;
            CreateLearningElementDialogOpen = false;
            EditLearningElementDialogOpen = false;
        }

        private readonly IAuthoringToolWorkspaceViewModel _authoringToolWorkspaceVm;
        private readonly IPresentationLogic _presentationLogic;
        private readonly ILearningWorldPresenter _learningWorldPresenter;
        private readonly ILearningSpacePresenter _learningSpacePresenter;
        private readonly ILearningElementPresenter _learningElementPresenter;
        private readonly ILogger<AuthoringToolWorkspacePresenter> _logger;

        internal bool CreateLearningWorldDialogOpen { get; set; }
        internal bool EditLearningWorldDialogOpen { get; set; }

        internal bool CreateLearningSpaceDialogueOpen { get; set; }
        internal bool EditLearningSpaceDialogOpen { get; set; }

        internal bool CreateLearningElementDialogOpen { get; set; }
        internal bool EditLearningElementDialogOpen { get; set; }

        internal bool LearningWorldSelected => _authoringToolWorkspaceVm.SelectedLearningWorld != null;


        /// <summary>
        /// This event is fired when <see cref="CreateNewLearningWorld"/> is called successfully and the newly created
        /// world is passed.
        /// </summary>
        internal event EventHandler<LearningWorldViewModel?>? OnLearningWorldCreate;

        /// <summary>
        /// This event is fired when <see cref="ChangeSelectedLearningWorld"/> is called successfully and the new
        /// selection is passed.
        /// </summary>
        internal event EventHandler<LearningWorldViewModel?>? OnLearningWorldSelect;

        /// <summary>
        /// This event is fired when <see cref="DeleteSelectedLearningWorld"/> is called successfully and the deleted
        /// world is passed.
        /// </summary>
        internal event EventHandler<LearningWorldViewModel?>? OnLearningWorldDelete;

        /// <summary>
        /// This event is fired when <see cref="EditSelectedLearningWorld(string,string,string,string,string,string)"/> is called by the modal dialog as a callback.
        /// The newly edited learning world is passed.
        /// </summary>
        internal event EventHandler<LearningWorldViewModel?>? OnLearningWorldEdit;

        /// <summary>
        /// Creates a new LearningWorld in our ViewModel.
        /// </summary>
        /// <param name="name">The name of the world.</param>
        /// <param name="shortname">The short name of the world.</param>
        /// <param name="authors">A list of authors of the world.</param>
        /// <param name="language">The primary language of the world.</param>
        /// <param name="description">A description of the world.</param>
        /// <param name="goals">The goals of the world.</param>
        internal void CreateNewLearningWorld(string name, string shortname, string authors, string language,
            string description, string goals)
        {
            //TODO: check if world with that name exists already? is name our unique identifier?
            var learningWorld =
                _learningWorldPresenter.CreateNewLearningWorld(name, shortname, authors, language, description, goals);
            _authoringToolWorkspaceVm.LearningWorlds.Add(learningWorld);
            OnLearningWorldCreate?.Invoke(this, learningWorld);
        }

        /// <summary>
        /// Changes the selected <see cref="LearningWorldViewModel"/> in the view model.
        /// </summary>
        /// <param name="worldName">The name of the world that should be selected.</param>
        /// <exception cref="ArgumentException">Thrown when no world with that name is registered in the view model.</exception>
        internal void ChangeSelectedLearningWorld(string worldName)
        {
            var world = _authoringToolWorkspaceVm.LearningWorlds.FirstOrDefault(world => world.Name == worldName);
            if (world == null) throw new ArgumentException("no world with that name in viewmodel");
            _authoringToolWorkspaceVm.SelectedLearningWorld = world;
            OnLearningWorldSelect?.Invoke(this, _authoringToolWorkspaceVm.SelectedLearningWorld);
        }

        /// <summary>
        /// Changes the selected <see cref="LearningWorldViewModel"/> in the view model.
        /// </summary>
        /// <param name="learningWorld">The learning world that should be set as selected</param>
        internal void ChangeSelectedLearningWorld(LearningWorldViewModel? learningWorld)
        {
            _authoringToolWorkspaceVm.SelectedLearningWorld = learningWorld;
            OnLearningWorldSelect?.Invoke(this, _authoringToolWorkspaceVm.SelectedLearningWorld);
        }

        /// <summary>
        /// Deletes the currently selected learning world from the view model and selects the last learning world in the
        /// collection, if any remain.
        /// </summary>
        public void DeleteSelectedLearningWorld()
        {
            var learningWorld = _authoringToolWorkspaceVm.SelectedLearningWorld;
            if (learningWorld == null) return;
            _authoringToolWorkspaceVm.LearningWorlds.Remove(learningWorld);
            ChangeSelectedLearningWorld(_authoringToolWorkspaceVm.LearningWorlds.LastOrDefault());
            OnLearningWorldDelete?.Invoke(this, learningWorld);
        }

        /// <summary>
        /// Edits the currently selected learning world to have the passed values.
        /// </summary>
        /// <param name="name">The name of the world.</param>
        /// <param name="shortname">The short name of the world.</param>
        /// <param name="authors">A list of authors of the world.</param>
        /// <param name="language">The primary language of the world.</param>
        /// <param name="description">A description of the world.</param>
        /// <param name="goals">The goals of the world.</param>
        /// <exception cref="ApplicationException">Thrown if now learning world is currently selected.</exception>
        public void EditSelectedLearningWorld(string name, string shortname, string authors, string language,
            string description, string goals)
        {
            if (_authoringToolWorkspaceVm.SelectedLearningWorld == null)
                throw new ApplicationException("SelectedLearningWorld is null");
            _authoringToolWorkspaceVm.SelectedLearningWorld =
                _learningWorldPresenter.EditLearningWorld(_authoringToolWorkspaceVm.SelectedLearningWorld, name,
                    shortname, authors, language, description, goals);
            OnLearningWorldEdit?.Invoke(this, _authoringToolWorkspaceVm.SelectedLearningWorld);
        }

        public async Task LoadLearningWorld()
        {
            var learningWorld = await _presentationLogic.LoadLearningWorld();
            _authoringToolWorkspaceVm.LearningWorlds.Add(learningWorld);
        }

        public void SaveSelectedLearningWorld()
        {
            if (_authoringToolWorkspaceVm.SelectedLearningWorld == null)
                throw new ApplicationException("SelectedLearningWorld is null");
            _presentationLogic.SaveLearningWorld(_authoringToolWorkspaceVm.SelectedLearningWorld);
        }

        /// <summary>
        /// Creates a new learning space in the currently selected learning world.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="shortname"></param>
        /// <param name="authors"></param>
        /// <param name="description"></param>
        /// <param name="goals"></param>
        /// <exception cref="ApplicationException">Thrown if no learning world is currently selected.</exception>
        public void CreateNewLearningSpace(string name, string shortname,
            string authors, string description, string goals)
        {
            if (_authoringToolWorkspaceVm.SelectedLearningWorld == null)
                throw new ApplicationException("SelectedLearningWorld is null");
            var learningSpace =
                _learningSpacePresenter.CreateNewLearningSpace(name, shortname, authors, description, goals);
            _authoringToolWorkspaceVm.SelectedLearningWorld.LearningSpaces.Add(learningSpace);
            SetSelectedLearningObject(learningSpace);
        }

        public void CreateNewLearningElement(string name, string shortname, string type, string content,
            string authors, string description, string goals)
        {
            if (_authoringToolWorkspaceVm.SelectedLearningWorld == null)
                throw new ApplicationException("SelectedLearningWorld is null");
            var learningElement =
                _learningElementPresenter.CreateNewLearningElement(name, shortname, type,
                    content, authors, description, goals);
            _authoringToolWorkspaceVm.SelectedLearningWorld.LearningElements.Add(learningElement);
            SetSelectedLearningObject(learningElement);
        }

        /// <summary>
        /// Changes the selected <see cref="ILearningObjectViewModel"/> in the currently selected learning world.
        /// </summary>
        /// <param name="learningObject">The learning object that should be set as selected</param>
        /// <exception cref="ApplicationException">Thrown if no learning world is currently selected.</exception>
        private void SetSelectedLearningObject(ILearningObjectViewModel learningObject)
        {
            if (_authoringToolWorkspaceVm.SelectedLearningWorld == null)
                throw new ApplicationException("SelectedLearningWorld is null");
            _authoringToolWorkspaceVm.SelectedLearningWorld.SelectedLearningObject = learningObject;
        }
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="shortname"></param>
        /// <param name="authors"></param>
        /// <param name="description"></param>
        /// <param name="goals"></param>
        /// <exception cref="ApplicationException">Thrown if no learning world is currently selected.</exception>
        /// <exception cref="NotImplementedException">Thrown if the selected learning object is of an other type as space or element.</exception>
        public void EditSelectedLearningObject(string name, string shortname, string authors, string description,
            string goals)
        {
            if (_authoringToolWorkspaceVm.SelectedLearningWorld == null)
                throw new ApplicationException("SelectedLearningWorld is null");
            switch (_authoringToolWorkspaceVm.SelectedLearningWorld.SelectedLearningObject)
            {
                case null:
                    throw new ApplicationException("SelectedLearningObject is null");
                case LearningSpaceViewModel learningSpaceViewModel:
                    _authoringToolWorkspaceVm.SelectedLearningWorld.SelectedLearningObject =
                        _learningSpacePresenter.EditLearningSpace(learningSpaceViewModel, name, shortname, authors,
                            description, goals);
                    break;
                case LearningElementViewModel learningElementViewModel:
                    _authoringToolWorkspaceVm.SelectedLearningWorld.SelectedLearningObject =
                        _learningElementPresenter.EditLearningElement(learningElementViewModel, name, shortname,
                            authors,
                            description, goals);
                    break;
                default:
                    throw new NotImplementedException("Type of LearningObject is not implemented");
            }
        }

        /// <summary>
        /// Deletes the selected learning object in the currently selected learning world and sets an other space or element as selected learning object.
        /// </summary>
        /// <exception cref="ApplicationException">Thrown if no learning world is currently selected.</exception>
        /// <exception cref="NotImplementedException">Thrown if the selected learning object is of an other type as space or element.</exception>
       public void DeleteSelectedLearningObject()
        {
            if (_authoringToolWorkspaceVm.SelectedLearningWorld == null)
                throw new ApplicationException("SelectedLearningWorld is null");
            switch (_authoringToolWorkspaceVm.SelectedLearningWorld.SelectedLearningObject)
            {
                case null:
                    return;
                case LearningSpaceViewModel learningSpace:
                    _authoringToolWorkspaceVm.SelectedLearningWorld.LearningSpaces.Remove(learningSpace);
                    break;
                case LearningElementViewModel learningElement:
                    _authoringToolWorkspaceVm.SelectedLearningWorld.LearningElements.Remove(learningElement);
                    break;
                default:
                    throw new NotImplementedException("Type of LearningObject is not implemented");
            }

            _authoringToolWorkspaceVm.SelectedLearningWorld.SelectedLearningObject =
                (ILearningObjectViewModel?) _authoringToolWorkspaceVm.SelectedLearningWorld?.LearningSpaces
                    .LastOrDefault() ??
                _authoringToolWorkspaceVm.SelectedLearningWorld?.LearningElements.LastOrDefault();
        }

        public void EditSelectedLearningWorld()
        {
            if (_authoringToolWorkspaceVm.SelectedLearningWorld == null)
            {
                //TODO: show error message?
                return;
            }

            //prepare dictionary property to pass to dialog
            _authoringToolWorkspaceVm.EditDialogInitialValues = new Dictionary<string, string>
            {
                {"Name", _authoringToolWorkspaceVm.SelectedLearningWorld.Name},
                {"Shortname", _authoringToolWorkspaceVm.SelectedLearningWorld.Shortname},
                {"Authors", _authoringToolWorkspaceVm.SelectedLearningWorld.Authors},
                {"Language", _authoringToolWorkspaceVm.SelectedLearningWorld.Language},
                {"Description", _authoringToolWorkspaceVm.SelectedLearningWorld.Description},
                {"Goals", _authoringToolWorkspaceVm.SelectedLearningWorld.Goals},
            };
            EditLearningWorldDialogOpen = true;
        }

        public Task OnCreateWorldDialogClose(
            Tuple<ModalDialogReturnValue, IDictionary<string, string>?> returnValueTuple)
        {
            var (response, data) = returnValueTuple;
            CreateLearningWorldDialogOpen = false;

            if (response == ModalDialogReturnValue.Cancel) return Task.CompletedTask;
            if (data == null) throw new ApplicationException("dialog data unexectedly null after Ok return value");

            foreach (var pair in data)
            {
                _logger.LogTrace($"{pair.Key}:{pair.Value}\n");
            }

            //required arguments
            var name = data["Name"];
            var shortname = data["Shortname"];
            var language = data["Language"];
            var description = data["Description"];
            var authors = data.ContainsKey("Authors") ? data["Authors"] : "";
            var goals = data.ContainsKey("Goals") ? data["Goals"] : "";
            CreateNewLearningWorld(name, shortname, authors, language, description, goals);
            return Task.CompletedTask;
        }

        public Task OnEditWorldDialogClose(Tuple<ModalDialogReturnValue, IDictionary<string, string>?> returnValueTuple)
        {
            var (response, data) = returnValueTuple;
            EditLearningWorldDialogOpen = false;

            if (response == ModalDialogReturnValue.Cancel) return Task.CompletedTask;
            if (data == null) throw new ApplicationException("dialog data unexpectedly null after Ok return value");

            //TODO: change this into a trace ILogger call
            foreach (var pair in data)
            {
                _logger.LogTrace($"{pair.Key}:{pair.Value}\n");
            }

            //required arguments
            var name = data["Name"];
            var shortname = data["Shortname"];
            var language = data["Language"];
            var description = data["Description"];
            //optional arguments
            var authors = data.ContainsKey("Authors") ? data["Authors"] : "";
            var goals = data.ContainsKey("Goals") ? data["Goals"] : "";

            EditSelectedLearningWorld(name, shortname, authors, language, description, goals);
            return Task.CompletedTask;
        }

        public Task OnCreateSpaceDialogClose(
            Tuple<ModalDialogReturnValue, IDictionary<string, string>?> returnValueTuple)
        {
            var (response, data) = returnValueTuple;
            CreateLearningSpaceDialogueOpen = false;

            if (response == ModalDialogReturnValue.Cancel) return Task.CompletedTask;
            if (data == null) throw new ApplicationException("dialog data unexectedly null after Ok return value");

            foreach (var pair in data)
            {
                Console.Write($"{pair.Key}:{pair.Value}\n");
            }

            //required arguments
            var name = data["Name"];
            var shortname = data["Shortname"];
            var description = data["Description"];
            //optional arguments
            var authors = data.ContainsKey("Authors") ? data["Authors"] : "";
            var goals = data.ContainsKey("Goals") ? data["Goals"] : "";
            CreateNewLearningSpace(name, shortname, authors, description, goals);
            return Task.CompletedTask;
        }

        public Task OnCreateElementDialogClose(
            Tuple<ModalDialogReturnValue, IDictionary<string, string>?> returnValueTuple)
        {
            var (response, data) = returnValueTuple;
            CreateLearningElementDialogOpen = false;

            if (response == ModalDialogReturnValue.Cancel) return Task.CompletedTask;
            if (data == null) throw new ApplicationException("dialog data unexectedly null after Ok return value");

            foreach (var pair in data)
            {
                Console.Write($"{pair.Key}:{pair.Value}\n");
            }

            //required arguments
            var name = data["Name"];
            var shortname = data["Shortname"];
            var type = data["Type"];
            var content = data["Content"];
            var description = data["Description"];
            //optional arguments
            var authors = data.ContainsKey("Authors") ? data["Authors"] : "";
            var goals = data.ContainsKey("Goals") ? data["Goals"] : "";
            CreateNewLearningElement(name, shortname, type, content, authors, description, goals);
            return Task.CompletedTask;
        }

        public void EditLearningObject()
        {
            if (_authoringToolWorkspaceVm.SelectedLearningWorld == null)
                throw new ApplicationException("SelectedLearningWorld is null");
            switch (_authoringToolWorkspaceVm.SelectedLearningWorld.SelectedLearningObject)
            {
                case null:
                    return;
                case LearningSpaceViewModel:
                    EditLearningSpace();
                    break;
                case LearningElementViewModel:
                    EditLearningElement();
                    break;
                default:
                    throw new ApplicationException("Type of LearningObject is not implemented");
            }
        }

        private void EditLearningSpace()
        {
            if (_authoringToolWorkspaceVm.SelectedLearningWorld?.SelectedLearningObject is not LearningSpaceViewModel
                space) throw new ApplicationException("Type of LearningObject is not implemented");
            //prepare dictionary property to pass to dialog
            _authoringToolWorkspaceVm.EditDialogInitialValues = new Dictionary<string, string>
            {
                {"Name", space.Name},
                {"Shortname", space.Shortname},
                {"Authors", space.Authors},
                {"Description", space.Description},
                {"Goals", space.Goals},
            };
            EditLearningSpaceDialogOpen = true;
        }

        private void EditLearningElement()
        {
            if (_authoringToolWorkspaceVm.SelectedLearningWorld?.SelectedLearningObject is not LearningElementViewModel
                element) throw new ApplicationException("Type of LearningObject is not implemented");
            //prepare dictionary property to pass to dialog
            _authoringToolWorkspaceVm.EditDialogInitialValues = new Dictionary<string, string>
            {
                {"Name", element.Name},
                {"Shortname", element.Shortname},
                {"Type", element.Type},
                {"Content", element.Content},
                {"Authors", element.Authors},
                {"Description", element.Description},
                {"Goals", element.Goals},
            };
            EditLearningElementDialogOpen = true;
        }

        public Task OnEditSpaceDialogClose(Tuple<ModalDialogReturnValue, IDictionary<string, string>?> returnValueTuple)
        {
            var (response, data) = returnValueTuple;
            EditLearningSpaceDialogOpen = false;

            if (response == ModalDialogReturnValue.Cancel) return Task.CompletedTask;
            if (data == null) throw new ApplicationException("dialog data unexpectedly null after Ok return value");

            //TODO: change this into a trace ILogger call
            foreach (var pair in data)
            {
                Console.Write($"{pair.Key}:{pair.Value}\n");
            }

            //required arguments
            var name = data["Name"];
            var shortname = data["Shortname"];
            var description = data["Description"];
            //optional arguments
            var authors = data.ContainsKey("Authors") ? data["Authors"] : "";
            var goals = data.ContainsKey("Goals") ? data["Goals"] : "";

            if (_authoringToolWorkspaceVm.SelectedLearningWorld == null)
                throw new ApplicationException("LearningWorld is null");
            if (_authoringToolWorkspaceVm.SelectedLearningWorld.SelectedLearningObject is not LearningSpaceViewModel
                learningSpaceViewModel) throw new ApplicationException("LearningObject is not a LearningSpace");
            _learningSpacePresenter.EditLearningSpace(learningSpaceViewModel, name, shortname, authors,
                description, goals);
            return Task.CompletedTask;
        }

        public Task OnEditElementDialogClose(
            Tuple<ModalDialogReturnValue, IDictionary<string, string>?> returnValueTuple)
        {
            var (response, data) = returnValueTuple;
            EditLearningElementDialogOpen = false;

            if (response == ModalDialogReturnValue.Cancel) return Task.CompletedTask;
            if (data == null) throw new ApplicationException("dialog data unexpectedly null after Ok return value");

            //TODO: change this into a trace ILogger call
            foreach (var pair in data)
            {
                Console.Write($"{pair.Key}:{pair.Value}\n");
            }

            //required arguments
            var name = data["Name"];
            var shortname = data["Shortname"];
            var type = data["Type"];
            var content = data["Content"];
            var description = data["Description"];
            //optional arguments
            var authors = data.ContainsKey("Authors") ? data["Authors"] : "";
            var goals = data.ContainsKey("Goals") ? data["Goals"] : "";

            if (_authoringToolWorkspaceVm.SelectedLearningWorld == null)
                throw new ApplicationException("LearningWorld is null");
            if (_authoringToolWorkspaceVm.SelectedLearningWorld.SelectedLearningObject is not LearningElementViewModel
                learningElementViewModel) throw new ApplicationException("LearningObject is not a LearningElement");
            _learningElementPresenter.EditLearningElement(learningElementViewModel, name, shortname, type,
                content, authors, description, goals);
            return Task.CompletedTask;
        }

        public Task OnLearningObjectSelect(ILearningObjectViewModel learningObject)
        {
            SetSelectedLearningObject(learningObject);
            return Task.CompletedTask;
        }
    }
}