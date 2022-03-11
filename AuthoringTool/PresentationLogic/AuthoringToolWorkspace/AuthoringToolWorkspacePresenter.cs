using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.LearningWorld;

namespace AuthoringTool.PresentationLogic.AuthoringToolWorkspace
{
    public class AuthoringToolWorkspacePresenter
    {

        public AuthoringToolWorkspacePresenter(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm,
            ILearningWorldPresenter learningWorldPresenter)
        {
            _learningWorldPresenter = learningWorldPresenter;
            _authoringToolWorkspaceVm = authoringToolWorkspaceVm;
            CreateLearningWorldDialogOpen = false;
            EditLearningWorldDialogOpen = false;
            CreateLearningSpaceDialogueOpen = false;
            EditLearningSpaceDialogOpen = false;
        }

        private readonly IAuthoringToolWorkspaceViewModel _authoringToolWorkspaceVm;
        private readonly ILearningWorldPresenter _learningWorldPresenter;
        
        internal bool CreateLearningWorldDialogOpen { get; set; }
        internal bool EditLearningWorldDialogOpen { get; set; }

        internal bool CreateLearningSpaceDialogueOpen { get; set; }
        internal bool EditLearningSpaceDialogOpen { get; set; }
        
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
            var learningWorld = _learningWorldPresenter.CreateNewLearningWorld(name, shortname, authors, language, description, goals);
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
        public void EditCurrentLearningWorld(string name, string shortname, string authors, string language, string description, string goals)
        {
            if (_authoringToolWorkspaceVm.SelectedLearningWorld == null)
                throw new ApplicationException("SelectedLearningWorld is null");
            _authoringToolWorkspaceVm.SelectedLearningWorld =
                _learningWorldPresenter.EditLearningWorld(_authoringToolWorkspaceVm.SelectedLearningWorld, name,
                    shortname, authors, language, description, goals);
            OnLearningWorldEdit?.Invoke(this, _authoringToolWorkspaceVm.SelectedLearningWorld);
        }

        public void CreateNewLearningSpace(LearningWorldViewModel selectedLearningWorld, string name, string shortname,
            string authors, string description, string goals)
        {
            var learningSpace = new LearningSpaceViewModel(name, shortname, authors, description, goals);
            selectedLearningWorld.LearningSpaces.Add(learningSpace);
            _authoringToolWorkspaceVm.SelectedLearningObject = learningSpace;
        }
        
        public void EditSelectedLearningObject(string name, string shortname, string authors, string description, string goals)
        {
            if (_authoringToolWorkspaceVm.SelectedLearningObject == null)
                throw new ApplicationException("SelectedLearningWorld is null");
            
            _authoringToolWorkspaceVm.SelectedLearningObject.Name = name;
            _authoringToolWorkspaceVm.SelectedLearningObject.Shortname = shortname;
            _authoringToolWorkspaceVm.SelectedLearningObject.Authors = authors;
            _authoringToolWorkspaceVm.SelectedLearningObject.Description = description;
            _authoringToolWorkspaceVm.SelectedLearningObject.Goals = goals;
        }

        public void DeleteSelectedLearningSpace(LearningWorldViewModel selectedLearningWorld)
        {
            if (_authoringToolWorkspaceVm.SelectedLearningObject == null) return;
            selectedLearningWorld.LearningSpaces.Remove((LearningSpaceViewModel) _authoringToolWorkspaceVm.SelectedLearningObject);
            _authoringToolWorkspaceVm.SelectedLearningObject = selectedLearningWorld.LearningSpaces.LastOrDefault();
        }
    }
}