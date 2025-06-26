using System.Threading.Tasks;
using ElectronSharp.API;
using ElectronSharp.API.Entities;

namespace ElectronWrapper;

class NotificationWrapper : INotificationWrapper
{
    readonly Notification _notification = Electron.Notification;

    /// <summary>
    /// Create OS desktop notifications
    /// </summary>
    /// <param name="notificationOptions"></param>
    public void Show(NotificationOptions notificationOptions)
    {
        _notification.Show(notificationOptions);
    }

    /// <summary>
    /// Whether or not desktop notifications are supported on the current system.
    /// </summary>
    /// <returns></returns>
    public Task<bool> IsSupportedAsync()
    {
        return _notification.IsSupportedAsync();
    }
}