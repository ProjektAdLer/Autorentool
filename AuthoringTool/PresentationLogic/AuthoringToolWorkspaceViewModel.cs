using AuthoringTool.Entities;

namespace AuthoringTool.PresentationLogic
{
    public class AuthoringToolWorkspaceViewModel
    {
        public AuthoringToolWorkspaceViewModel()
        {
            Count = 0;
            LearningWorlds = new List<ILearningWorld>();
            SelectedLearningWorld = null;
        }
        
        public int Count { get; set; }
        internal List<ILearningWorld> LearningWorlds { get; set; }
        internal ILearningWorld? SelectedLearningWorld { get; set; }
    }
}