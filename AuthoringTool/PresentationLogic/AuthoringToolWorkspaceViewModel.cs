using AuthoringTool.Entities;
using AuthoringTool.PresentationLogic.LearningWorld;

namespace AuthoringTool.PresentationLogic
{
    public class AuthoringToolWorkspaceViewModel
    {
        public AuthoringToolWorkspaceViewModel()
        {
            Count = 0;
            LearningWorlds = new List<LearningWorldViewModel>();
            SelectedLearningWorld = null;
        }
        
        public int Count { get; set; }
        internal List<LearningWorldViewModel> LearningWorlds { get; set; }
        internal LearningWorldViewModel? SelectedLearningWorld { get; set; }
    }
}