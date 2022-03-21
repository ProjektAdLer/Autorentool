using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.LearningWorld;
using AuthoringTool.PresentationLogic.LearningElement;

namespace AuthoringTool.PresentationLogic.AuthoringToolWorkspace
{
    public class AuthoringToolWorkspacePresenter
    {
        public AuthoringToolWorkspacePresenter(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm,
            ILearningWorldPresenter learningWorldPresenter, ILearningSpacePresenter learningSpacePresenter, ILearningElementPresenter learningElementPresenter)
        {
            _learningSpacePresenter = learningSpacePresenter;
            _learningElementPresenter = learningElementPresenter;
            _learningWorldPresenter = learningWorldPresenter;
            _authoringToolWorkspaceVm = authoringToolWorkspaceVm;
            CreateLearningWorldDialogOpen = false;
            EditLearningWorldDialogOpen = false;
            CreateLearningSpaceDialogueOpen = false;
            EditLearningSpaceDialogOpen = false;
            CreateLearningElementDialogOpen = false;
            EditLearningElementDialogOpen = false;
        }

        private readonly IAuthoringToolWorkspaceViewModel _authoringToolWorkspaceVm;
        private readonly ILearningWorldPresenter _learningWorldPresenter;
        private readonly ILearningSpacePresenter _learningSpacePresenter;
        private readonly ILearningElementPresenter _learningElementPresenter;

        internal bool CreateLearningWorldDialogOpen { get; set; }
        internal bool EditLearningWorldDialogOpen { get; set; }

        internal bool CreateLearningSpaceDialogueOpen { get; set; }
        internal bool EditLearningSpaceDialogOpen { get; set; }
        
        internal bool CreateLearningElementDialogOpen { get; set; }
        internal bool EditLearningElementDialogOpen { get; set; }


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
        /// This event is fired when <see cref="EditCurrentLearningWorld"/> is called by the modal dialog as a callback.
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
        private void ChangeSelectedLearningWorld(LearningWorldViewModel? learningWorld)
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
        public void EditCurrentLearningWorld(string name, string shortname, string authors, string language,
            string description, string goals)
        {
            if (_authoringToolWorkspaceVm.SelectedLearningWorld == null)
                throw new ApplicationException("SelectedLearningWorld is null");
            _authoringToolWorkspaceVm.SelectedLearningWorld =
                _learningWorldPresenter.EditLearningWorld(_authoringToolWorkspaceVm.SelectedLearningWorld, name,
                    shortname, authors, language, description, goals);
            OnLearningWorldEdit?.Invoke(this, _authoringToolWorkspaceVm.SelectedLearningWorld);
        }

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

        public void CreateNewLearningElement(string name, string shortname,
            string authors, string description, string goals)
        {
            if (_authoringToolWorkspaceVm.SelectedLearningWorld == null)
                throw new ApplicationException("SelectedLearningWorld is null");
            var learningElement =
                _learningElementPresenter.CreateNewLearningElement(name, shortname, authors, description, goals);
            _authoringToolWorkspaceVm.SelectedLearningWorld.LearningElements.Add(learningElement);
            SetSelectedLearningObject(learningElement);
        }

        public void SetSelectedLearningObject(ILearningObjectViewModel learningObject)
        {
            if (_authoringToolWorkspaceVm.SelectedLearningWorld == null)
                throw new ApplicationException("SelectedLearningWorld is null");
            _authoringToolWorkspaceVm.SelectedLearningWorld.SelectedLearningObject = learningObject;
        }

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
                        _learningElementPresenter.EditLearningElement(learningElementViewModel, name, shortname, authors,
                            description, goals);
                    break;
                default:
                    throw new ApplicationException("Type of LearningObject is not implemented");
            }
        }

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
                    throw new ApplicationException("Type of LearningObject is not implemented");
            }

            _authoringToolWorkspaceVm.SelectedLearningWorld.SelectedLearningObject =
                (ILearningObjectViewModel?) _authoringToolWorkspaceVm.SelectedLearningWorld?.LearningSpaces
                    .LastOrDefault() ??
                _authoringToolWorkspaceVm.SelectedLearningWorld?.LearningElements.LastOrDefault();
        }
    }
}