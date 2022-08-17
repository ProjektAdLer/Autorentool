using ElectronNET.API.Entities;
using System;
using System.Threading.Tasks;

namespace ElectronWrapper;

interface IScreenWrapper
{
    event Action<Display> OnDisplayAdded;
    event Action<Display, string[]> OnDisplayMetricsChanged;
    event Action<Display> OnDisplayRemoved;

    Task<Display[]> GetAllDisplaysAsync();
    Task<Point> GetCursorScreenPointAsync();
    Task<Display> GetDisplayMatchingAsync(Rectangle rectangle);
    Task<Display> GetDisplayNearestPointAsync(Point point);
    Task<int> GetMenuBarHeightAsync();
    Task<Display> GetPrimaryDisplayAsync();
}