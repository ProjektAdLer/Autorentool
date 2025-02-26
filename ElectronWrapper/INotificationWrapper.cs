using System.Threading.Tasks;
using ElectronSharp.API.Entities;

namespace ElectronWrapper;

interface INotificationWrapper
{
    void Show(NotificationOptions notificationOptions);
    Task<bool> IsSupportedAsync();
}