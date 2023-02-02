using Presentation.Components.ModalDialog;
using Presentation.PresentationLogic.Content;

namespace Presentation.PresentationLogic.ModalDialog;

public interface ISpaceViewModalDialogInputFieldsFactory
{ 
    /// <summary>
    /// Get the input fields for the create element modal dialog, optionally dependant on 
    /// dragged and dropped  content.
    /// </summary>
    /// <param name="dragAndDropContent">Possibly drag-and-dropped  content.</param>
    /// <returns>The input fields for the modal dialog.</returns>
    IEnumerable<ModalDialogInputField> GetCreateElementInputFields(
        ContentViewModel? dragAndDropContent);
    
    /// <summary>
    /// Get the input fields for the edit space modal dialog.
    /// </summary>
    /// <returns>The input fields for the modal dialog.</returns>
    IEnumerable<ModalDialogInputField> GetEditSpaceInputFields();
    
    /// <summary>
    /// Get the input fields for the edit element modal dialog.
    /// </summary>
    /// <returns>The input fields for the modal dialog.</returns>
    IEnumerable<ModalDialogInputField> GetEditElementInputFields();
}