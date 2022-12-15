using Presentation.Components.ModalDialog;
using Presentation.PresentationLogic.LearningContent;

namespace Presentation.PresentationLogic.ModalDialog;

public interface ILearningSpaceViewModalDialogInputFieldsFactory
{ 
    /// <summary>
    /// Get the input fields for the create element modal dialog, optionally dependant on 
    /// dragged and dropped learning content.
    /// </summary>
    /// <param name="dragAndDropLearningContent">Possibly drag-and-dropped learning content.</param>
    /// <returns>The input fields for the modal dialog.</returns>
    IEnumerable<ModalDialogInputField> GetCreateLearningElementInputFields(
        LearningContentViewModel? dragAndDropLearningContent);
    
    /// <summary>
    /// Get the input fields for the edit space modal dialog.
    /// </summary>
    /// <returns>The input fields for the modal dialog.</returns>
    IEnumerable<ModalDialogInputField> GetEditLearningSpaceInputFields(List<string> topics);
    
    /// <summary>
    /// Get the input fields for the edit element modal dialog.
    /// </summary>
    /// <returns>The input fields for the modal dialog.</returns>
    IEnumerable<ModalDialogInputField> GetEditLearningElementInputFields();
}