using AuthoringTool.Components.ModalDialog;
using AuthoringTool.PresentationLogic.LearningContent;

namespace AuthoringTool.PresentationLogic.ModalDialog;

public interface ILearningSpaceViewModalDialogInputFieldsFactory
{ 
    /// <summary>
    /// Get the input fields for the create element modal dialog, optionally dependant on 
    /// dragged and dropped learning content.
    /// </summary>
    /// <param name="dragAndDropLearningContent">Possibly drag-and-dropped learning content.</param>
    /// <param name="spaceName">Name of the learning space.</param>
    /// <returns>The input fields for the modal dialog.</returns>
    IEnumerable<ModalDialogInputField> GetCreateLearningElementInputFields(
        LearningContentViewModel? dragAndDropLearningContent, string spaceName);
    
    /// <summary>
    /// Get the input fields for the edit space modal dialog.
    /// </summary>
    /// <returns>The input fields for the modal dialog.</returns>
    IEnumerable<ModalDialogInputField> GetEditLearningSpaceInputFields();
    
    /// <summary>
    /// Get the input fields for the edit element modal dialog.
    /// </summary>
    /// <returns>The input fields for the modal dialog.</returns>
    IEnumerable<ModalDialogInputField> GetEditLearningElementInputFields();
}