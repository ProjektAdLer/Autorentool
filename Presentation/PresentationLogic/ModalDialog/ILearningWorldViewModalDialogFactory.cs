using Microsoft.AspNetCore.Components;
using Presentation.Components.ModalDialog;

namespace Presentation.PresentationLogic.ModalDialog;

public interface ILearningWorldViewModalDialogFactory
{
    /// <summary>
    /// Dynamically generates a ModalDialog Render Fragment for a "Create learning space" dialog.
    /// </summary>
    /// <param name="onCloseCallback">The callback that should be called upon closing the dialog.</param>
    /// <param name="initialTopics">Available topics for assignment.</param>
    /// <param name="annotations">Annotations behind input fields</param>
    /// <returns>A RenderFragment containing the dialog.</returns>
    RenderFragment GetCreateLearningSpaceFragment(ModalDialogOnClose onCloseCallback, List<string> initialTopics, IDictionary<string, string>? annotations = null);
    
    /// <summary>
    /// Dynamically generates a ModalDialog Render Fragment for a "Create pathway condition" dialog.
    /// </summary>
    /// <param name="onCloseCallback">The callback that should be called upon closing the dialog.</param>
    /// <returns>A RenderFragment containing the dialog.</returns>
    RenderFragment GetCreatePathWayConditionFragment(ModalDialogOnClose onCloseCallback);
    
    /// <summary>
    /// Dynamically generates a ModalDialog Render Fragment for a "Create Topic" dialog.
    /// </summary>
    /// <param name="onCloseCallback">The callback that should be called upon closing the dialog.</param>
    /// <returns>A RenderFragment containing the dialog.</returns>
    RenderFragment GetCreateTopicFragment(ModalDialogOnClose onCloseCallback);

    /// <summary>
    /// Dynamically generates a ModalDialog Render Fragment for a "Edit learning space" dialog with initial values.
    /// </summary>
    /// <param name="initialInputValues">The initial values for the input fields.</param>
    /// <param name="initialTopics">Available topics for assignment.</param>
    /// <param name="onCloseCallback">The callback that should be called upon closing the dialog.</param>
    /// <param name="annotations">Annotations behind input fields</param>
    /// <returns>A RenderFragment containing the dialog.</returns>
    RenderFragment GetEditLearningSpaceFragment(IDictionary<string, string> initialInputValues, List<string> initialTopics,
        ModalDialogOnClose onCloseCallback, IDictionary<string, string>? annotations = null);

    /// <summary>
    /// Dynamically generates a ModalDialog Render Fragment for a "Edit pathway condition" dialog with initial values.
    /// </summary>
    /// <param name="initialInputValues">The initial values for the input fields.</param>
    /// <param name="onCloseCallback">The callback that should be called upon closing the dialog.</param>
    /// <returns>A RenderFragment containing the dialog.</returns>
    RenderFragment GetEditPathWayConditionFragment(IDictionary<string, string> initialInputValues, ModalDialogOnClose onCloseCallback);

    /// <summary>
    /// Dynamically generates a ModalDialog Render Fragment for a "Edit Topic" dialog with initial values.
    /// </summary>
    /// <param name="initialInputValues">The initial values for the input field.</param>
    /// <param name="onCloseCallback">The callback that should be called upon closing the dialog.</param>
    /// <returns>A RenderFragment containing the dialog.</returns>
    RenderFragment GetEditTopicFragment(List<string> initialInputValues, ModalDialogOnClose onCloseCallback);

    /// <summary>
    /// Dynamically generates a ModalDialog Render Fragment for a "Delete Topic" dialog with initial values.
    /// </summary>
    /// <param name="initialInputValues">The initial values for the input field.</param>
    /// <param name="onCloseCallback">The callback that should be called upon closing the dialog.</param>
    /// <returns>A RenderFragment containing the dialog.</returns>
    RenderFragment GetDeleteTopicFragment(List<string> initialInputValues, ModalDialogOnClose onCloseCallback);
}