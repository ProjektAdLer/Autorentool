using AuthoringTool.Entities;
using AuthoringTool.PresentationLogic.LearningWorld;

namespace AuthoringTool.PresentationLogic
{
    public class AuthoringToolWorkspaceViewModel
    {
        public AuthoringToolWorkspaceViewModel()
        {
            LearningWorlds = new List<LearningWorldViewModel>();
            SelectedLearningWorld = null;
        }
        
        internal List<LearningWorldViewModel> LearningWorlds { get; set; }
        internal LearningWorldViewModel? SelectedLearningWorld { get; set; }
        internal ILearningObjectViewModel? SelectedLearningObject { get; set; }
    }
}