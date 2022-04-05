using AuthoringTool.Components.ModalDialog;
using AuthoringTool.PresentationLogic.LearningWorld;

namespace AuthoringTool.PresentationLogic.AuthoringToolWorkspace;

public interface IAuthoringToolWorkspaceViewModel
{
    List<LearningWorldViewModel> LearningWorlds { get; set; }
    LearningWorldViewModel? SelectedLearningWorld { get; set; }
    IDictionary<string, string>? EditDialogInitialValues { get; set; }
}