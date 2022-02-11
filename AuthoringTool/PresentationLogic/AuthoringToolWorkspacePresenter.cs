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

        internal void CreateNewLearningWorld()
        {
            AuthoringToolWorkspaceVm.LearningWorlds.Add(new Entities.LearningWorld());
        }
        

        public void DeleteLastLearningWorld()
        {
            AuthoringToolWorkspaceVm.LearningWorlds.Remove(AuthoringToolWorkspaceVm.LearningWorlds.Last());
        }
    }
}