using ElectronNET.API;
using ElectronNET.API.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ElectronWrapper;

interface IDockWrapper
{
    IReadOnlyCollection<MenuItem> MenuItems { get; }

    Task<int> BounceAsync(DockBounceType type, CancellationToken cancellationToken = default);
    void CancelBounce(int id);
    void DownloadFinished(string filePath);
    Task<string> GetBadgeAsync(CancellationToken cancellationToken = default);
    Task<Menu> GetMenu(CancellationToken cancellationToken = default);
    void Hide();
    Task<bool> IsVisibleAsync(CancellationToken cancellationToken = default);
    void SetBadge(string text);
    void SetIcon(string image);
    void SetMenu(MenuItem[] menuItems);
    void Show();
}