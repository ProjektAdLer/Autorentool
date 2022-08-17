﻿using System;
using ElectronNET.API;

namespace ElectronWrapper;

/// <summary>
/// Communicate asynchronously from the main process to renderer processes.
/// </summary>
class IpcMainWrapper : IIpcMainWrapper
{
    private IpcMain ipcMain;
    public IpcMainWrapper()
    {
        ipcMain = Electron.IpcMain;
    }

    /// <summary>
    ///  Listens to channel, when a new message arrives listener would be called with 
    ///  listener(event, args...).
    /// </summary>
    /// <param name="channel">Channelname.</param>
    /// <param name="listener">Callback Method.</param>
    public void On(string channel, Action<object> listener)
    {
        ipcMain.On(channel, listener);
    }

    /// <summary>
    /// Send a message to the renderer process synchronously via channel, 
    /// you can also send arbitrary arguments.
    /// 
    /// Note: Sending a synchronous message will block the whole renderer process,
    /// unless you know what you are doing you should never use it.
    /// </summary>
    /// <param name="channel"></param>
    /// <param name="listener"></param>
    public void OnSync(string channel, Func<object, object> listener)
    {
        ipcMain.OnSync(channel, listener);
    }

    /// <summary>
    /// Adds a one time listener method for the event. This listener is invoked only
    ///  the next time a message is sent to channel, after which it is removed.
    /// </summary>
    /// <param name="channel">Channelname.</param>
    /// <param name="listener">Callback Method.</param>
    public void Once(string channel, Action<object> listener)
    {
        ipcMain.Once(channel, listener);
    }

    /// <summary>
    /// Removes listeners of the specified channel.
    /// </summary>
    /// <param name="channel">Channelname.</param>
    public void RemoveAllListeners(string channel)
    {
        ipcMain.RemoveAllListeners(channel);
    }

    /// <summary>
    /// Send a message to the renderer process asynchronously via channel, you can also send
    /// arbitrary arguments. Arguments will be serialized in JSON internally and hence
    /// no functions or prototype chain will be included. The renderer process handles it by
    /// listening for channel with ipcRenderer module.
    /// </summary>
    /// <param name="browserWindow">BrowserWindow with channel.</param>
    /// <param name="channel">Channelname.</param>
    /// <param name="data">Arguments data.</param>
    public void Send(BrowserWindow browserWindow, string channel, params object[] data)
    {
        ipcMain.Send(browserWindow.InnerBrowserWindow, channel, data);

    }

    /// <summary>
    /// Send a message to the BrowserView renderer process asynchronously via channel, you can also send
    /// arbitrary arguments. Arguments will be serialized in JSON internally and hence
    /// no functions or prototype chain will be included. The renderer process handles it by
    /// listening for channel with ipcRenderer module.
    /// </summary>
    /// <param name="browserView">BrowserView with channel.</param>
    /// <param name="channel">Channelname.</param>
    /// <param name="data">Arguments data.</param>
    public void Send(BrowserView browserView, string channel, params object[] data)
    {
        ipcMain.Send(browserView, channel, data);
    }
}