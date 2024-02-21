using System.ComponentModel;

namespace Presentation.PresentationLogic.Mediator;

public interface IMediator : INotifyPropertyChanged
{
    /// <summary>
    /// Indicates whether the world dialog is open.
    /// </summary>
    bool WorldDialogOpen { get; }

    /// <summary>
    /// Indicates whether the space dialog is open.
    /// </summary>
    bool SpaceDialogOpen { get; }

    /// <summary>
    /// Indicates whether the element dialog is open.
    /// </summary>
    bool ElementDialogOpen { get; }

    /// <summary>
    /// Indicates whether the element dialog is open.
    /// </summary>
    bool AdaptivityElementDialogOpen { get; }

    /// <summary>
    /// Indicates whether the content dialog is open.
    /// </summary>
    bool ContentDialogOpen { get; }

    /// <summary>
    /// Indicates whether the world view is open.
    /// </summary>
    bool WorldPathwayViewOpen { get; }

    /// <summary>
    /// Indicates whether the world overview is open.
    /// </summary>
    bool WorldTreeViewOpen { get; }

    bool OverwriteElementEdit { get; set; }

    /// <summary>
    /// Requests the opening of the world dialog.
    /// </summary>
    void RequestOpenWorldDialog();

    /// <summary>
    /// Requests the opening of the space dialog.
    /// </summary>
    void RequestOpenSpaceDialog();

    /// <summary>
    /// Requests the opening of the element dialog.
    /// </summary>
    void RequestOpenElementDialog();

    /// <summary>
    /// Requests the opening of the adaptivity element dialog.
    /// </summary>
    void RequestOpenAdaptivityElementDialog();

    /// <summary>
    /// Requests the opening of the element dialog with the new element dialog being forced.
    /// </summary>
    void RequestOpenNewElementDialog();

    /// <summary>
    /// Requests the opening of the content dialog.
    /// </summary>
    void RequestOpenContentDialog();

    /// <summary>
    /// Requests the opening of the world view.
    /// </summary>
    void RequestOpenPathwayView();

    /// <summary>
    /// Requests the opening of the world overview.
    /// </summary>
    void RequestOpenWorldTreeView();

    /// <summary>
    /// Requests toggling the world dialog.
    /// </summary>
    void RequestToggleWorldDialog();

    /// <summary>
    /// Requests toggling the space dialog.
    /// </summary>
    void RequestToggleSpaceDialog();

    /// <summary>
    /// Requests toggling the element dialog.
    /// </summary>
    void RequestToggleElementDialog();

    /// <summary>
    /// Requests toggling the adaptivity element dialog.
    /// </summary>
    void RequestToggleAdaptivityElementDialog();

    /// <summary>
    /// Requests toggling the content dialog.
    /// </summary>
    void RequestToggleContentDialog();

    /// <summary>
    /// Requests toggling the world view.
    /// </summary>
    void RequestToggleWorldPathwayView();

    /// <summary>
    /// Requests toggling the world overview.
    /// </summary>
    void RequestToggleWorldTreeView();

    /// <summary>
    /// Closes every dialog and view.
    /// </summary>
    void CloseBothSides();
}