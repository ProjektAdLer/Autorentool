using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;

namespace ElectronWrapper;

class NotificationWrapper : INotificationWrapper
{
    Notification notification;

    public NotificationWrapper()
    {
        notification = Electron.Notification;
    }

    /// <summary>
    /// Create OS desktop notifications
    /// </summary>
    /// <param name="notificationOptions"></param>
    public void Show(NotificationOptions notificationOptions)
    {
        notification.Show(notificationOptions);
    }

    /// <summary>
    /// Whether or not desktop notifications are supported on the current system.
    /// </summary>
    /// <returns></returns>
    public Task<bool> IsSupportedAsync()
    {
        return notification.IsSupportedAsync();
    }
}