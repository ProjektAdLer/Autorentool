using System.Threading.Tasks;
using ElectronNET.API.Entities;

namespace ElectronWrapper;

interface INotificationWrapper
{
    void Show(NotificationOptions notificationOptions);
    Task<bool> IsSupportedAsync();
}