using Microsoft.AspNetCore.Components;
using Presentation.Components.ModalDialog;
using Presentation.PresentationLogic.Content;

namespace Presentation.PresentationLogic.ModalDialog;

public interface ISpaceViewModalDialogFactory
{
    /// <summary>
    /// Dynamically generates a ModalDialog Render Fragment for a "Create  element" dialog.
    /// </summary>
    /// <param name="dragAndDropContent">Possibly drag-and-dropped  content.</param>
    /// <param name="onCloseCallback">The callback that should be called upon closing the dialog.</param>
    /// <param name="spaceName">Name of the  space.</param>
    /// <returns>A RenderFragment containing the dialog.</returns>
    RenderFragment GetCreateElementFragment(ContentViewModel? dragAndDropContent,
        ModalDialogOnClose onCloseCallback);

    /// <summary>
    /// Dynamically generates a ModalDialog Render Fragment for a "Edit  space" dialog with initial values.
    /// </summary>
    /// <param name="initialInputValues">The initial values for the input fields.</param>
    /// <param name="onCloseCallback">The callback that should be called upon closing the dialog.</param>
    /// <param name="annotations"></param>
    /// <returns>A RenderFragment containing the dialog.</returns>
    RenderFragment GetEditSpaceFragment(IDictionary<string, string> initialInputValues,
        ModalDialogOnClose onCloseCallback, IDictionary<string, string>? annotations = null);
    
    /// <summary>
    /// Dynamically generates a ModalDialog Render Fragment for a "Edit element" dialog with initial values.
    /// </summary>
    /// <param name="initialInputValues">The initial values for the input fields.</param>
    /// <param name="onCloseCallback">The callback that should be called upon closing the dialog.</param>
    /// <returns>A RenderFragment containing the dialog.</returns>
    RenderFragment GetEditElementFragment(IDictionary<string, string> initialInputValues,
        ModalDialogOnClose onCloseCallback);
}