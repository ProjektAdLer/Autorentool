using System.Collections.Generic;
using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;
using System.Threading;

namespace ElectronWrapper;

/// <summary>
/// Control your app in the macOS dock.
/// </summary>
class DockWrapper : IDockWrapper
{
    private Dock dock;
    public DockWrapper()
    {
        dock = Electron.Dock;
    }

    /// <summary>
    /// When <see cref="DockBounceType.Critical"/> is passed, the dock icon will bounce until either the application becomes
    /// active or the request is canceled. When <see cref="DockBounceType.Informational"/> is passed, the dock icon will bounce
    /// for one second. However, the request remains active until either the application becomes active or the request is canceled.
    /// <para/>
    /// Note: This method can only be used while the app is not focused; when the app is focused it will return -1.
    /// </summary>
    /// <param name="type">Can be critical or informational. The default is informational.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Return an ID representing the request.</returns>
    public async Task<int> BounceAsync(DockBounceType type, CancellationToken cancellationToken = default)
    {
        return await dock.BounceAsync(type, cancellationToken);
    }

    /// <summary>
    /// Cancel the bounce of id.
    /// </summary>
    /// <param name="id">Id of the request.</param>
    public void CancelBounce(int id)
    {
        dock.CancelBounce(id);
    }

    /// <summary>
    /// Bounces the Downloads stack if the filePath is inside the Downloads folder.
    /// </summary>
    /// <param name="filePath"></param>
    public void DownloadFinished(string filePath)
    {
        dock.DownloadFinished(filePath);
    }


    /// <summary>
    /// Sets the string to be displayed in the dock’s badging area.
    /// </summary>
    /// <param name="text"></param>
    public void SetBadge(string text)
    {
        dock.SetBadge(text);
    }

    /// <summary>
    /// Gets the string to be displayed in the dock’s badging area.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The badge string of the dock.</returns>
    public async Task<string> GetBadgeAsync(CancellationToken cancellationToken = default)
    {
        return await dock.GetBadgeAsync(cancellationToken);
    }

    /// <summary>
    /// Hides the dock icon.
    /// </summary>
    public void Hide()
    {
        dock.Hide();
    }

    /// <summary>
    /// Shows the dock icon.
    /// </summary>
    public void Show()
    {
        dock.Show();
    }


    /// <summary>
    /// Whether the dock icon is visible. The app.dock.show() call is asynchronous
    /// so this method might not return true immediately after that call.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Whether the dock icon is visible.</returns>
    public async Task<bool> IsVisibleAsync(CancellationToken cancellationToken = default)
    {
        return await dock.IsVisibleAsync(cancellationToken);
    }

    /// <summary>
    /// Gets the dock menu items.
    /// </summary>
    /// <value>
    /// The menu items.
    /// </value>
    public IReadOnlyCollection<MenuItem> MenuItems => dock.MenuItems;

    /// <summary>
    /// Sets the application's dock menu.
    /// </summary>
    public void SetMenu(MenuItem[] menuItems)
    {
        dock.SetMenu(menuItems);
    }

    /// <summary>
    /// TODO: Menu (macOS) still to be implemented
    /// Gets the application's dock menu.
    /// </summary>
    public async Task<Menu> GetMenu(CancellationToken cancellationToken = default)
    {
        return await dock.GetMenu(cancellationToken);
    }

    /// <summary>
    /// Sets the image associated with this dock icon.
    /// </summary>
    /// <param name="image"></param>
    public void SetIcon(string image)
    {
        dock.SetIcon(image);
    }
}