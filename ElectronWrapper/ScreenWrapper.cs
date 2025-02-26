using System;
using System.Threading.Tasks;
using ElectronSharp.API;
using ElectronSharp.API.Entities;

namespace ElectronWrapper;

/// <summary>
/// Retrieve information about screen size, displays, cursor position, etc.
/// </summary>
class ScreenWrapper : IScreenWrapper
{
    private readonly Screen _screen = Electron.Screen;

    /// <summary>
    /// Emitted when an new Display has been added.
    /// </summary>
    public event Action<Display> OnDisplayAdded
    {
        add => _screen.OnDisplayAdded += value;

        remove => _screen.OnDisplayAdded -= value;
    }

    /// <summary>
    /// Emitted when oldDisplay has been removed.
    /// </summary>
    public event Action<Display> OnDisplayRemoved
    {
        add => _screen.OnDisplayRemoved += value;
        remove => _screen.OnDisplayRemoved -= value;
    }

    /// <summary>
    /// Emitted when one or more metrics change in a display. 
    /// The changedMetrics is an array of strings that describe the changes. 
    /// Possible changes are bounds, workArea, scaleFactor and rotation.
    /// </summary>
    public event Action<Display, string[]> OnDisplayMetricsChanged
    {
        add => _screen.OnDisplayMetricsChanged += value;
        remove => _screen.OnDisplayMetricsChanged -= value;
    }

    /// <summary>
    /// The current absolute position of the mouse pointer.
    /// </summary>
    /// <returns></returns>
    public Task<Point> GetCursorScreenPointAsync()
    {
        return _screen.GetCursorScreenPointAsync();
    }

    /// <summary>
    /// macOS: The height of the menu bar in pixels.
    /// </summary>
    /// <returns>The height of the menu bar in pixels.</returns>
    public Task<int> GetMenuBarHeightAsync()
    {
        return _screen.GetMenuBarHeightAsync();
    }

    /// <summary>
    /// The primary display.
    /// </summary>
    /// <returns></returns>
    public Task<Display> GetPrimaryDisplayAsync()
    {
        return _screen.GetPrimaryDisplayAsync();
    }


    /// <summary>
    /// An array of displays that are currently available.
    /// </summary>
    /// <returns>An array of displays that are currently available.</returns>
    public Task<Display[]> GetAllDisplaysAsync()
    {
        return _screen.GetAllDisplaysAsync();
    }

    /// <summary>
    /// The display nearest the specified point.
    /// </summary>
    /// <returns>The display nearest the specified point.</returns>
    public Task<Display> GetDisplayNearestPointAsync(Point point)
    {
        return _screen.GetDisplayNearestPointAsync(point);
    }


    /// <summary>
    /// The display that most closely intersects the provided bounds.
    /// </summary>
    /// <param name="rectangle"></param>
    /// <returns>The display that most closely intersects the provided bounds.</returns>
    public Task<Display> GetDisplayMatchingAsync(Rectangle rectangle)
    {
        return _screen.GetDisplayMatchingAsync(rectangle);
    }
}