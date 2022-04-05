using AuthoringTool.Components.ModalDialog;
using AuthoringTool.PresentationLogic.LearningWorld;

namespace AuthoringTool.PresentationLogic.AuthoringToolWorkspace
{
    public class AuthoringToolWorkspaceViewModel : IAuthoringToolWorkspaceViewModel
    {
        public AuthoringToolWorkspaceViewModel()
        {
            LearningWorlds = new List<LearningWorldViewModel>();
            SelectedLearningWorld = null;
            EditDialogInitialValues = null;
        }

        public List<LearningWorldViewModel> LearningWorlds { get; set; }
        public LearningWorldViewModel? SelectedLearningWorld { get; set; }

        public IDictionary<string, string>? EditDialogInitialValues { get; set; }
    }
}