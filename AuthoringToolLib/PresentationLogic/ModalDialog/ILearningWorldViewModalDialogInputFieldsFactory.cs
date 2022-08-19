using AuthoringToolLib.Components.ModalDialog;
using AuthoringToolLib.PresentationLogic.LearningContent;
using AuthoringToolLib.PresentationLogic.LearningSpace;

namespace AuthoringToolLib.PresentationLogic.ModalDialog;

public interface ILearningWorldViewModalDialogInputFieldsFactory
{
    /// <summary>
    /// Gets the input fields for the create element modal dialog, optionally dependant on 
    /// dragged and dropped learning content.
    /// </summary>
    /// <param name="dragAndDropLearningContent">Possibly drag-and-dropped learning content.</param>
    /// <param name="learningSpaces">LearningSpaces that already exist in the learning world.</param>
    /// <param name="worldName">Name of the learning world.</param>
    /// <returns>The input fields for the modal dialog.</returns>
    IEnumerable<ModalDialogInputField> GetCreateLearningElementInputFields(
        LearningContentViewModel? dragAndDropLearningContent, IEnumerable<ILearningSpaceViewModel> learningSpaces, string worldName);
    
    /// <summary>
    /// Gets the input fields for the create space modal dialog.
    /// </summary>
    /// <returns>The input fields for the modal dialog.</returns>
    IEnumerable<ModalDialogInputField> GetCreateLearningSpaceInputFields();
    
    /// <summary>
    /// Gets the input fields for the edit space modal dialog.
    /// </summary>
    /// <returns>The input fields for the modal dialog.</returns>
    IEnumerable<ModalDialogInputField> GetEditLearningSpaceInputFields();
    
    /// <summary>
    /// Gets the input fields for the edit element modal dialog.
    /// </summary>
    /// <returns>The input fields for the modal dialog.</returns>
    IEnumerable<ModalDialogInputField> GetEditLearningElementInputFields();
}