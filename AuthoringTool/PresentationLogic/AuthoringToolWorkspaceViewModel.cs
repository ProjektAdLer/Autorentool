using AuthoringTool.Entities;

namespace AuthoringTool.PresentationLogic
{
    public class AuthoringToolWorkspaceViewModel
    {
        public AuthoringToolWorkspaceViewModel()
        {
            Count = 0;
            LearningWorlds = new List<ILearningWorld>();
        }
        
        public int Count { get; set; }
        internal List<ILearningWorld> LearningWorlds { get; set; }
    }
}