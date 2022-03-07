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
        internal ILearningObjectViewModel SelectedLearningObject { get; set; }

        internal void IncrementCount()
        {
            AuthoringToolWorkspaceVm.Count++;
        }

        internal void CreateNewLearningWorld(string name, string shortname, string authors, string language,
            string description, string goals)
        {
            //TODO: check if world with that name exists already? is name our unique identifier?
            var learningWorld = new LearningWorldViewModel(name, shortname, authors, language, description, goals);
            AuthoringToolWorkspaceVm.LearningWorlds.Add(learningWorld);
        }
        

        public void DeleteSelectedLearningWorld()
        {
            AuthoringToolWorkspaceVm.LearningWorlds.Remove(AuthoringToolWorkspaceVm.LearningWorlds.Last());
        }

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
        }
        
        public void EditSelectedLearningObject(string name, string shortname, string authors, string description, string goals)
        {
            if (SelectedLearningObject == null)
                throw new ApplicationException("SelectedLearningWorld is null");
            
            SelectedLearningObject.Name = name;
            SelectedLearningObject.Shortname = shortname;
            SelectedLearningObject.Authors = authors;
            SelectedLearningObject.Description = description;
            SelectedLearningObject.Goals = goals;
        }

        public void DeleteLastLearningSpace(LearningWorldViewModel selectedLearningWorld)
        {
            if (selectedLearningWorld.LearningSpaces.Count > 0)
            {
                selectedLearningWorld.LearningSpaces.Remove(selectedLearningWorld.LearningSpaces.Last());
            }
        }
    }
}
