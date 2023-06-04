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
    /// Indicates whether the content dialog is open.
    /// </summary>
    bool ContentDialogOpen { get; }
    /// <summary>
    /// Indicates whether the world view is open.
    /// </summary>
    bool WorldViewOpen { get; }
    /// <summary>
    /// Indicates whether the world overview is open.
    /// </summary>
    bool WorldOverviewOpen { get; }

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
    void RequestOpenWorldView();
    /// <summary>
    /// Requests the opening of the world overview.
    /// </summary>
    void RequestOpenWorldOverview();
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
    /// Requests toggling the content dialog.
    /// </summary>
    void RequestToggleContentDialog();
    /// <summary>
    /// Requests toggling the world view.
    /// </summary>
    void RequestToggleWorldView();
    /// <summary>
    /// Requests toggling the world overview.
    /// </summary>
    void RequestToggleWorldOverview();

    /// <summary>
    /// Closes every dialog and view.
    /// </summary>
    void CloseBothSides();
}