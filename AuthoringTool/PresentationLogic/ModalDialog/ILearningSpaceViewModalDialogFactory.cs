using AuthoringTool.Components.ModalDialog;
using AuthoringTool.PresentationLogic.LearningContent;
using Microsoft.AspNetCore.Components;

namespace AuthoringTool.PresentationLogic.ModalDialog;

public interface ILearningSpaceViewModalDialogFactory
{
    /// <summary>
    /// Dynamically generates a ModalDialog Render Fragment for a "Create learning element" dialog.
    /// </summary>
    /// <param name="dragAndDropLearningContent">Possibly drag-and-dropped learning content.</param>
    /// <param name="onCloseCallback">The callback that should be called upon closing the dialog.</param>
    /// <param name="spaceName">Name of the learning space.</param>
    /// <returns>A RenderFragment containing the dialog.</returns>
    RenderFragment GetCreateLearningElementFragment(LearningContentViewModel? dragAndDropLearningContent,
        ModalDialogOnClose onCloseCallback, string spaceName);

    /// <summary>
    /// Dynamically generates a ModalDialog Render Fragment for a "Edit learning space" dialog with initial values.
    /// </summary>
    /// <param name="initialInputValues">The initial values for the input fields.</param>
    /// <param name="onCloseCallback">The callback that should be called upon closing the dialog.</param>
    /// <returns>A RenderFragment containing the dialog.</returns>
    RenderFragment GetEditLearningSpaceFragment(IDictionary<string, string> initialInputValues,
        ModalDialogOnClose onCloseCallback);
    
    /// <summary>
    /// Dynamically generates a ModalDialog Render Fragment for a "Edit learning element" dialog with initial values.
    /// </summary>
    /// <param name="initialInputValues">The initial values for the input fields.</param>
    /// <param name="onCloseCallback">The callback that should be called upon closing the dialog.</param>
    /// <returns>A RenderFragment containing the dialog.</returns>
    RenderFragment GetEditLearningElementFragment(IDictionary<string, string> initialInputValues,
        ModalDialogOnClose onCloseCallback);
}