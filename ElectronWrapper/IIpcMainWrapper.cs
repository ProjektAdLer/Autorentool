using ElectronNET.API;
using System;

namespace ElectronWrapper;

interface IIpcMainWrapper
{
    void On(string channel, Action<object> listener);
    void Once(string channel, Action<object> listener);
    void OnSync(string channel, Func<object, object> listener);
    void RemoveAllListeners(string channel);
    void Send(BrowserView browserView, string channel, params object[] data);
    void Send(BrowserWindow browserWindow, string channel, params object[] data);
}