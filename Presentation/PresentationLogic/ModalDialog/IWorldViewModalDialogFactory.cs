using Microsoft.AspNetCore.Components;
using Presentation.Components.ModalDialog;

namespace Presentation.PresentationLogic.ModalDialog;

public interface IWorldViewModalDialogFactory
{
    /// <summary>
    /// Dynamically generates a ModalDialog Render Fragment for a "Create  space" dialog.
    /// </summary>
    /// <param name="onCloseCallback">The callback that should be called upon closing the dialog.</param>
    /// <param name="annotations">Annotations behind input fields</param>
    /// <returns>A RenderFragment containing the dialog.</returns>
    RenderFragment GetCreateSpaceFragment(ModalDialogOnClose onCloseCallback, IDictionary<string, string>? annotations = null);
    
    /// <summary>
    /// Dynamically generates a ModalDialog Render Fragment for a "Create pathway condition" dialog.
    /// </summary>
    /// <param name="onCloseCallback">The callback that should be called upon closing the dialog.</param>
    /// <returns>A RenderFragment containing the dialog.</returns>
    RenderFragment GetCreatePathWayConditionFragment(ModalDialogOnClose onCloseCallback);

    /// <summary>
    /// Dynamically generates a ModalDialog Render Fragment for a "Edit  space" dialog with initial values.
    /// </summary>
    /// <param name="initialInputValues">The initial values for the input fields.</param>
    /// <param name="onCloseCallback">The callback that should be called upon closing the dialog.</param>
    /// <param name="annotations">Annotations behind input fields</param>
    /// <returns>A RenderFragment containing the dialog.</returns>
    RenderFragment GetEditSpaceFragment(IDictionary<string, string> initialInputValues,
        ModalDialogOnClose onCloseCallback, IDictionary<string, string>? annotations = null);

    /// <summary>
    /// Dynamically generates a ModalDialog Render Fragment for a "Edit pathway condition" dialog with initial values.
    /// </summary>
    /// <param name="initialInputValues">The initial values for the input fields.</param>
    /// <param name="onCloseCallback">The callback that should be called upon closing the dialog.</param>
    /// <returns>A RenderFragment containing the dialog.</returns>
    RenderFragment GetEditPathWayConditionFragment(IDictionary<string, string> initialInputValues, ModalDialogOnClose onCloseCallback);
}