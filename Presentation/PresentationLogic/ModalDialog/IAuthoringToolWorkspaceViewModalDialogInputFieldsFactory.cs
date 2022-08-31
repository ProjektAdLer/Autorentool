using Presentation.Components.ModalDialog;

namespace Presentation.PresentationLogic.ModalDialog;

public interface IAuthoringToolWorkspaceViewModalDialogInputFieldsFactory
{
    /// <summary>
    /// Get the input fields for the create learning world modal dialog.
    /// </summary>
    /// <returns>The input fields for the modal dialog.</returns>
    IEnumerable<ModalDialogInputField> GetCreateLearningWorldInputFields();
    
    /// <summary>
    /// Get the input fields for the edit world modal dialog.
    /// </summary>
    /// <returns>The input fields for the modal dialog.</returns>
    IEnumerable<ModalDialogInputField> GetEditLearningWorldInputFields();
}