using AuthoringTool.PresentationLogic.LearningWorld;

namespace AuthoringTool.PresentationLogic
{
    public class AuthoringToolWorkspacePresenter
    {
        public AuthoringToolWorkspacePresenter(AuthoringToolWorkspaceViewModel authoringToolWorkspaceVm)
        {
            AuthoringToolWorkspaceVm = authoringToolWorkspaceVm;
        }
        
        private AuthoringToolWorkspaceViewModel AuthoringToolWorkspaceVm { get; set; }
        
        internal bool CreateLearningWorldDialogueOpen { get; set; }
        
        internal void IncrementCount()
        {
            AuthoringToolWorkspaceVm.Count++;
        }

        internal void CreateNewLearningWorld(string name, string shortname, string authors, string language,
            string description, string goals)
        {
            var learningWorld = new LearningWorldViewModel(name, shortname, authors, language, description, goals);
            AuthoringToolWorkspaceVm.LearningWorlds.Add(learningWorld);
        }
        

        public void DeleteLastLearningWorld()
        {
            AuthoringToolWorkspaceVm.LearningWorlds.Remove(AuthoringToolWorkspaceVm.LearningWorlds.Last());
        }
    }
}