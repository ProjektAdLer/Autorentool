using AuthoringTool.PresentationLogic.LearningWorld;
using AuthoringTool.PresentationLogic.LearningSpace;

namespace AuthoringTool.PresentationLogic
{
    public class AuthoringToolWorkspacePresenter
    {
        public AuthoringToolWorkspacePresenter(AuthoringToolWorkspaceViewModel authoringToolWorkspaceVm)
        {
            AuthoringToolWorkspaceVm = authoringToolWorkspaceVm;
        }
        
        private AuthoringToolWorkspaceViewModel AuthoringToolWorkspaceVm { get; set; }
        
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
            var learningWorld = new LearningWorldViewModel(name, shortname, authors, language, description, goals);
            AuthoringToolWorkspaceVm.LearningWorlds.Add(learningWorld);
            OnLearningWorldCreate?.Invoke(this, learningWorld);
        }
        
        /// <summary>
        /// Changes the selected <see cref="LearningWorldViewModel"/> in the view model.
        /// </summary>
        /// <param name="worldName">The name of the world that should be selected.</param>
        /// <exception cref="ArgumentException">Thrown when no world with that name is registered in the view model.</exception>
        internal void ChangeSelectedLearningWorld(string worldName)
        {
            var world = AuthoringToolWorkspaceVm.LearningWorlds.FirstOrDefault(world => world.Name == worldName);
            if (world == null) throw new ArgumentException("no world with that name in viewmodel");
            AuthoringToolWorkspaceVm.SelectedLearningWorld = world;
            OnLearningWorldSelect?.Invoke(this, AuthoringToolWorkspaceVm.SelectedLearningWorld);
        }

        private void ChangeSelectedLearningWorld(LearningWorldViewModel? learningWorld)
        {
            AuthoringToolWorkspaceVm.SelectedLearningWorld = learningWorld;
            OnLearningWorldSelect?.Invoke(this, AuthoringToolWorkspaceVm.SelectedLearningWorld);
        }
        
        /// <summary>
        /// Deletes the currently selected learning world from the view model and selects the last learning world in the
        /// collection, if any remain.
        /// </summary>
        public void DeleteSelectedLearningWorld()
        {
            var learningWorld = AuthoringToolWorkspaceVm.SelectedLearningWorld;
            if (learningWorld == null) return;
            AuthoringToolWorkspaceVm.LearningWorlds.Remove(learningWorld);
            ChangeSelectedLearningWorld(AuthoringToolWorkspaceVm.LearningWorlds.LastOrDefault());
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
            if (AuthoringToolWorkspaceVm.SelectedLearningWorld == null)
                throw new ApplicationException("SelectedLearningWorld is null");
            
            AuthoringToolWorkspaceVm.SelectedLearningWorld.Name = name;
            AuthoringToolWorkspaceVm.SelectedLearningWorld.Shortname = shortname;
            AuthoringToolWorkspaceVm.SelectedLearningWorld.Authors = authors;
            AuthoringToolWorkspaceVm.SelectedLearningWorld.Language = language;
            AuthoringToolWorkspaceVm.SelectedLearningWorld.Description = description;
            AuthoringToolWorkspaceVm.SelectedLearningWorld.Goals = goals;
        }

        public void CreateNewLearningSpace(LearningWorldViewModel selectedLearningWorld, string name, string shortname,
            string authors, string description, string goals)
        {
            var learningSpace = new LearningSpaceViewModel(name, shortname, authors, description, goals);
            selectedLearningWorld.LearningSpaces.Add(learningSpace);
            AuthoringToolWorkspaceVm.SelectedLearningObject = learningSpace;
        }
        
        public void EditSelectedLearningObject(string name, string shortname, string authors, string description, string goals)
        {
            if (AuthoringToolWorkspaceVm.SelectedLearningObject == null)
                throw new ApplicationException("SelectedLearningWorld is null");
            
            AuthoringToolWorkspaceVm.SelectedLearningObject.Name = name;
            AuthoringToolWorkspaceVm.SelectedLearningObject.Shortname = shortname;
            AuthoringToolWorkspaceVm.SelectedLearningObject.Authors = authors;
            AuthoringToolWorkspaceVm.SelectedLearningObject.Description = description;
            AuthoringToolWorkspaceVm.SelectedLearningObject.Goals = goals;
        }

        public void DeleteSelectedLearningSpace(LearningWorldViewModel selectedLearningWorld)
        {
            if (AuthoringToolWorkspaceVm.SelectedLearningObject == null) return;
            selectedLearningWorld.LearningSpaces.Remove((LearningSpaceViewModel) AuthoringToolWorkspaceVm.SelectedLearningObject);
            AuthoringToolWorkspaceVm.SelectedLearningObject = selectedLearningWorld.LearningSpaces.LastOrDefault();
        }
    }
}