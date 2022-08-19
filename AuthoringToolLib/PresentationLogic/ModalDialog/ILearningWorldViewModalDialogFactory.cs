using AuthoringToolLib.Components.ModalDialog;
using AuthoringToolLib.PresentationLogic.LearningContent;
using AuthoringToolLib.PresentationLogic.LearningSpace;
using Microsoft.AspNetCore.Components;

namespace AuthoringToolLib.PresentationLogic.ModalDialog;

public interface ILearningWorldViewModalDialogFactory
{
    /// <summary>
    /// Dynamically generates a ModalDialog Render Fragment for a "Create learning element" dialog.
    /// </summary>
    /// <param name="dragAndDropLearningContent">Possibly drag-and-dropped learning content.</param>
    /// <param name="learningSpaces">LearningSpaces that already exist in the learning world.</param>
    /// <param name="worldName">Name of the learning world.</param>
    /// <param name="onCloseCallback">The callback that should be called upon closing the dialog.</param>
    /// <returns>A RenderFragment containing the dialog.</returns>
    RenderFragment GetCreateLearningElementFragment(LearningContentViewModel? dragAndDropLearningContent,
        IEnumerable<ILearningSpaceViewModel> learningSpaces, string worldName ,ModalDialogOnClose onCloseCallback);
    
    /// <summary>
    /// Dynamically generates a ModalDialog Render Fragment for a "Create learning space" dialog.
    /// </summary>
    /// <param name="onCloseCallback">The callback that should be called upon closing the dialog.</param>
    /// <returns>A RenderFragment containing the dialog.</returns>
    RenderFragment GetCreateLearningSpaceFragment(ModalDialogOnClose onCloseCallback);

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