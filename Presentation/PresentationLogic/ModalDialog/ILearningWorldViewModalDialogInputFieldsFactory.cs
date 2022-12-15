using Presentation.Components.ModalDialog;

namespace Presentation.PresentationLogic.ModalDialog;

public interface ILearningWorldViewModalDialogInputFieldsFactory
{
    /// <summary>
    /// Gets the input fields for the create space modal dialog.
    /// </summary>
    /// <returns>The input fields for the modal dialog.</returns>
    IEnumerable<ModalDialogInputField> GetCreateLearningSpaceInputFields(List<string> topics);
    
    /// <summary>
    /// Gets the input fields for the create pathway condition modal dialog.
    /// </summary>
    /// <returns>The input fields for the modal dialog.</returns>
    IEnumerable<ModalDialogInputField> GetCreatePathWayConditionInputFields();
    
    /// <summary>
    /// Gets the input fields for the create topic modal dialog.
    /// </summary>
    /// <returns>The input fields for the modal dialog.</returns>
    IEnumerable<ModalDialogInputField> GetCreateTopicInputFields();
    
    /// <summary>
    /// Gets the input fields for the edit space modal dialog.
    /// </summary>
    /// <returns>The input fields for the modal dialog.</returns>
    IEnumerable<ModalDialogInputField> GetEditLearningSpaceInputFields(List<string> topics);
    
    /// <summary>
    /// Gets the input fields for the edit pathway condition modal dialog.
    /// </summary>
    /// <returns>The input fields for the modal dialog.</returns>
    IEnumerable<ModalDialogInputField> GetEditPathWayConditionInputFields();
    
    /// <summary>
    /// Gets the input fields for the edit topic modal dialog.
    /// </summary>
    /// <returns>The input fields for the modal dialog.</returns>
    IEnumerable<ModalDialogInputField> GetEditTopicInputFields(List<string> topics);
    
    /// <summary>
    /// Gets the input fields for the delete topic modal dialog.
    /// </summary>
    /// <returns>The input fields for the modal dialog.</returns>
    IEnumerable<ModalDialogInputField> GetDeleteTopicInputFields(List<string> topics);
}