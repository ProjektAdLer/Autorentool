using Microsoft.AspNetCore.Components;
using Presentation.Components.ModalDialog;

namespace Presentation.PresentationLogic.ModalDialog;

public interface IAuthoringToolWorkspaceViewModalDialogFactory
{
    /// <summary>
    /// Dynamically generates a ModalDialog Render Fragment for a information messagebox.
    /// </summary>
    /// <param name="onCloseCallback">The callback that should be called upon closing the information messagebox.</param>
    /// <param name="informationMessage">The message that should be displayed.</param>
    /// <returns>A RenderFragment containing the information messagebox.</returns>
    RenderFragment GetInformationMessageFragment(ModalDialogOnClose onCloseCallback, string informationMessage);

    /// <summary>
    /// Dynamically generates a ModalDialog Render Fragment for a "Save unsaved changes?" dialog.
    /// </summary>
    /// <param name="onCloseCallback">The callback that should be called upon closing the dialog.</param>
    /// <param name="unsavedWorldName">Name of the unsaved world.</param>
    /// <returns>A RenderFragment containing the dialog.</returns>
    RenderFragment GetSaveUnsavedWorldsFragment(ModalDialogOnClose onCloseCallback, string unsavedWorldName);

    /// <summary>
    /// Dynamically generates a ModalDialog Render Fragment for a "Save deleted world?" dialog. 
    /// </summary>
    /// <param name="onCloseCallback">The callback that should be called upon closing the dialog.</param>
    /// <param name="deletedUnsavedWorldName">Name of the deleted world with unsaved changes.</param>
    /// <returns>A RenderFragment containing the dialog.</returns>
    RenderFragment GetDeleteUnsavedWorldFragment(ModalDialogOnClose onCloseCallback, string deletedUnsavedWorldName);

    /// <summary>
    /// Dynamically generates a ModalDialog Render Fragment for a "Create new world" dialog. 
    /// </summary>
    /// <param name="onCloseCallback">The callback that should be called upon closing the dialog.</param>
    /// <returns>A RenderFragment containing the dialog.</returns>
    RenderFragment GetCreateWorldFragment(ModalDialogOnClose onCloseCallback);

    /// <summary>
    /// Dynamically generates a ModalDialog Render Fragment for a "Edit existing  world" dialog.
    /// </summary>
    /// <param name="initialInputValues">The initial values for the input fields.</param>
    /// <param name="onCloseCallback">The callback that should be called upon closing the dialog.</param>
    /// <returns>A RenderFragment containing the dialog.</returns>
    RenderFragment GetEditWorldFragment(IDictionary<string, string> initialInputValues,
        ModalDialogOnClose onCloseCallback);

    /// <summary>
    /// Dynamically generates a ModalDialog Render Fragment for a errorState messagebox.
    /// </summary>
    /// <param name="onCloseCallback">The callback that should be called upon closing the dialog.</param>
    /// <param name="errorState">The error that should be displayed.</param>
    /// <returns>A RenderFragment containing the errorState messagebox.</returns>
    RenderFragment GetErrorStateFragment(ModalDialogOnClose onCloseCallback, string errorState);
}