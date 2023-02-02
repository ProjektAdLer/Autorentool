using Presentation.Components.ModalDialog;

namespace Presentation.PresentationLogic.ModalDialog;

public interface IAuthoringToolWorkspaceViewModalDialogInputFieldsFactory
{
    /// <summary>
    /// Get the input fields for the create world modal dialog.
    /// </summary>
    /// <returns>The input fields for the modal dialog.</returns>
    IEnumerable<ModalDialogInputField> GetCreateWorldInputFields();
    
    /// <summary>
    /// Get the input fields for the edit world modal dialog.
    /// </summary>
    /// <returns>The input fields for the modal dialog.</returns>
    IEnumerable<ModalDialogInputField> GetEditWorldInputFields();
}