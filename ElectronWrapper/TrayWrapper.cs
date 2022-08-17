using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;

namespace ElectronWrapper;

/// <summary>
/// Add icons and context menus to the system's notification area.
/// </summary>
class TrayWrapper : ITrayWrapper
{
    private Tray tray = Electron.Tray;


    /// <summary>
    /// Emitted when the tray icon is clicked.
    /// </summary>
    public event Action<TrayClickEventArgs, Rectangle> OnClick
    {
        add => tray.OnClick += value;
        remove => tray.OnClick -= value;
    }

    /// <summary>
    /// macOS, Windows: Emitted when the tray icon is right clicked.
    /// </summary>
    public event Action<TrayClickEventArgs, Rectangle> OnRightClick
    {
        add => tray.OnRightClick += value;
        remove => tray.OnRightClick -= value;
    }
    /// <summary>
    /// macOS, Windows: Emitted when the tray icon is double clicked.
    /// </summary>
    public event Action<TrayClickEventArgs, Rectangle> OnDoubleClick
    {
        add => tray.OnDoubleClick += value;
        remove => tray.OnDoubleClick -= value;
    }

    /// <summary>
    /// Windows: Emitted when the tray balloon shows.
    /// </summary>
    public event Action OnBalloonShow
    {
        add => tray.OnBalloonShow += value;
        remove => tray.OnBalloonShow -= value;
    }
    /// <summary>
    /// Windows: Emitted when the tray balloon is clicked.
    /// </summary>
    public event Action OnBalloonClick
    {
        add => tray.OnBalloonClick += value;
        remove => tray.OnBalloonClick -= value;
    }

    /// <summary>
    /// Windows: Emitted when the tray balloon is closed 
    /// because of timeout or user manually closes it.
    /// </summary>
    public event Action OnBalloonClosed
    {
        add => tray.OnBalloonClosed += value;
        remove => tray.OnBalloonClosed -= value;
    }

    /// <summary>
    /// Gets the menu items.
    /// </summary>
    /// <value>
    /// The menu items.
    /// </value>
    public IReadOnlyCollection<MenuItem> MenuItems => tray.MenuItems;

    /// <summary>
    /// Shows the Traybar.
    /// </summary>
    /// <param name="image">The image.</param>
    /// <param name="menuItem">The menu item.</param>
    public void Show(string image, MenuItem menuItem)
    {
        tray.Show(image, menuItem);
    }

    /// <summary>
    /// Shows the Traybar.
    /// </summary>
    /// <param name="image">The image.</param>
    /// <param name="menuItems">The menu items.</param>
    public void Show(string image, MenuItem[] menuItems)
    {
        tray.Show(image, menuItems);
    }

    /// <summary>
    /// Shows the Traybar (empty).
    /// </summary>
    /// <param name="image">The image.</param>
    public void Show(string image)
    {
        tray.Show(image);
    }

    /// <summary>
    /// Destroys the tray icon immediately.
    /// </summary>
    public void Destroy()
    {
        tray.Destroy();
    }

    /// <summary>
    /// Sets the image associated with this tray icon.
    /// </summary>
    /// <param name="image"></param>
    public void SetImage(string image)
    {
        tray.SetImage(image);
    }

    /// <summary>
    /// Sets the image associated with this tray icon when pressed on macOS.
    /// </summary>
    /// <param name="image"></param>
    public void SetPressedImage(string image)
    {
        tray.SetPressedImage(image);
    }

    /// <summary>
    /// Sets the hover text for this tray icon.
    /// </summary>
    /// <param name="toolTip"></param>
    public void SetToolTip(string toolTip)
    {
        tray.SetToolTip(toolTip);
    }

    /// <summary>
    /// macOS: Sets the title displayed aside of the tray icon in the status bar.
    /// </summary>
    /// <param name="title"></param>
    public void SetTitle(string title)
    {
        tray.SetTitle(title);
    }

    /// <summary>
    /// Windows: Displays a tray balloon.
    /// </summary>
    /// <param name="options"></param>
    public void DisplayBalloon(DisplayBalloonOptions options)
    {
        tray.DisplayBalloon(options);
    }

    /// <summary>
    /// Whether the tray icon is destroyed.
    /// </summary>
    /// <returns></returns>
    public Task<bool> IsDestroyedAsync()
    {
        return tray.IsDestroyedAsync();
    }

    /// <summary>
    /// Subscribe to an unmapped event on the <see cref="Tray"/> module.
    /// </summary>
    /// <param name="eventName">The event name</param>
    /// <param name="fn">The handler</param>
    public void On(string eventName, Action fn)
    { tray.On(eventName, fn); }

    /// <summary>
    /// Subscribe to an unmapped event on the <see cref="Tray"/> module.
    /// </summary>
    /// <param name="eventName">The event name</param>
    /// <param name="fn">The handler</param>
    public void On(string eventName, Action<object> fn)
    { tray.On(eventName, fn); }

    /// <summary>
    /// Subscribe to an unmapped event on the <see cref="Tray"/> module once.
    /// </summary>
    /// <param name="eventName">The event name</param>
    /// <param name="fn">The handler</param>
    public void Once(string eventName, Action fn)
    { tray.Once(eventName, fn); }

    /// <summary>
    /// Subscribe to an unmapped event on the <see cref="Tray"/> module once.
    /// </summary>
    /// <param name="eventName">The event name</param>
    /// <param name="fn">The handler</param>
    public void Once(string eventName, Action<object> fn)
    { tray.Once(eventName, fn); }
}