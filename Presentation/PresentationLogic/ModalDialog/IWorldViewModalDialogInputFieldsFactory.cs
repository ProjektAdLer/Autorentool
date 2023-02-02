using Presentation.Components.ModalDialog;
using Presentation.PresentationLogic.Content;

namespace Presentation.PresentationLogic.ModalDialog;

public interface IWorldViewModalDialogInputFieldsFactory
{
    /// <summary>
    /// Gets the input fields for the create space modal dialog.
    /// </summary>
    /// <returns>The input fields for the modal dialog.</returns>
    IEnumerable<ModalDialogInputField> GetCreateSpaceInputFields();
    
    /// <summary>
    /// Gets the input fields for the create pathway condition modal dialog.
    /// </summary>
    /// <returns>The input fields for the modal dialog.</returns>
    IEnumerable<ModalDialogInputField> GetCreatePathWayConditionInputFields();
    
    /// <summary>
    /// Gets the input fields for the edit space modal dialog.
    /// </summary>
    /// <returns>The input fields for the modal dialog.</returns>
    IEnumerable<ModalDialogInputField> GetEditSpaceInputFields();
    
    /// <summary>
    /// Gets the input fields for the edit pathway condition modal dialog.
    /// </summary>
    /// <returns>The input fields for the modal dialog.</returns>
    IEnumerable<ModalDialogInputField> GetEditPathWayConditionInputFields();
}