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

        public ModalDialogInputField[] ModalDialogWorldInputFields { get; } =
        {
            new("Name", ModalDialogInputType.Text, true),
            new("Shortname", ModalDialogInputType.Text, true),
            new("Authors", ModalDialogInputType.Text),
            new("Language", ModalDialogInputType.Text, true),
            new("Description", ModalDialogInputType.Text, true),
            new("Goals", ModalDialogInputType.Text)
        };

        public ModalDialogInputField[] ModalDialogSpaceInputFields { get; } =
        {
            new("Name", ModalDialogInputType.Text, true),
            new("Shortname", ModalDialogInputType.Text, true),
            new("Authors", ModalDialogInputType.Text),
            new("Description", ModalDialogInputType.Text, true),
            new("Goals", ModalDialogInputType.Text)
        };

        public ModalDialogInputField[] ModalDialogElementInputFields { get; } =
        {
            new("Name", ModalDialogInputType.Text, true),
            new("Shortname", ModalDialogInputType.Text, true),
            new ModalDialogDropdownInputField("Type",
                new[] { new ModalDialogDropdownInputFieldChoiceMapping(null,
                    new[] {"Transfer", "Activation", "Interaction","Test"})}, true),
            new ModalDialogDropdownInputField("Content", 
                new[] { new ModalDialogDropdownInputFieldChoiceMapping(null,
                    new[] { "Text", "Picture", "Video" }) }, true),
            new("Authors", ModalDialogInputType.Text),
            new("Description", ModalDialogInputType.Text, true),
            new("Goals", ModalDialogInputType.Text)
        };

        public IDictionary<string, string>? EditDialogInitialValues { get; set; }
    }
}