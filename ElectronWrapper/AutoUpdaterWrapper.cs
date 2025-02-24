using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ElectronSharp.API;
using ElectronSharp.API.Entities;

namespace ElectronWrapper;

/// <summary>
/// Enable apps to automatically update themselves. Based on electron-updater.
/// </summary>
class AutoUpdaterWrapper : IAutoUpdaterWrapper
{
    private AutoUpdater autoUpdater;

    public AutoUpdaterWrapper()
    {
        autoUpdater = Electron.AutoUpdater;
    }

    /// <summary>
    /// The request headers.
    /// </summary>
    public Dictionary<string, string> RequestHeaders
    {
        set => autoUpdater.RequestHeaders = value;
    }

    /// <summary>
    /// Emitted when there is an error while updating.
    /// </summary>
    public event Action<string> OnError
    {
        add => autoUpdater.OnError += value;
        remove => autoUpdater.OnError -= value;
    }

    /// <summary>
    /// Emitted when checking if an update has started.
    /// </summary>
    public event Action OnCheckingForUpdate
    {
        add => autoUpdater.OnCheckingForUpdate += value;
        remove => autoUpdater.OnCheckingForUpdate -= value;
    }

    /// <summary>
    /// Emitted when there is an available update. 
    /// The update is downloaded automatically if AutoDownload is true.
    /// </summary>
    public event Action<UpdateInfo> OnUpdateAvailable
    {
        add => autoUpdater.OnUpdateAvailable += value;
        remove => autoUpdater.OnUpdateAvailable -= value;
    }

    /// <summary>
    /// Emitted when there is no available update.
    /// </summary>
    public event Action<UpdateInfo> OnUpdateNotAvailable
    {
        add => autoUpdater.OnUpdateNotAvailable += value;
        remove => autoUpdater.OnUpdateNotAvailable -= value;
    }

    /// <summary>
    /// Emitted on download progress.
    /// </summary>
    public event Action<ProgressInfo> OnDownloadProgress
    {
        add => autoUpdater.OnDownloadProgress += value;
        remove => autoUpdater.OnDownloadProgress -= value;
    }

    /// <summary>
    /// Emitted on download complete.
    /// </summary>
    public event Action<UpdateInfo> OnUpdateDownloaded
    {
        add => autoUpdater.OnUpdateDownloaded += value;
        remove => autoUpdater.OnUpdateDownloaded -= value;
    }

    /// <summary>
    /// Asks the server whether there is an update.
    /// </summary>
    /// <returns></returns>
    public Task<UpdateCheckResult> CheckForUpdatesAsync()
    {
        return autoUpdater.CheckForUpdatesAsync();
    }

    /// <summary>
    /// Asks the server whether there is an update.
    /// 
    /// This will immediately download an update, then install when the app quits.
    /// </summary>
    /// <returns></returns>
    public Task<UpdateCheckResult> CheckForUpdatesAndNotifyAsync()
    {
        return autoUpdater.CheckForUpdatesAndNotifyAsync();
    }

    /// <summary>
    ///  Restarts the app and installs the update after it has been downloaded.
    ///  It should only be called after `update-downloaded` has been emitted.
    ///  
    ///  Note: QuitAndInstall() will close all application windows first and only emit `before-quit` event on `app` after that.
    ///  This is different from the normal quit event sequence.
    /// </summary>
    /// <param name="isSilent">*windows-only* Runs the installer in silent mode. Defaults to `false`.</param>
    /// <param name="isForceRunAfter">Run the app after finish even on silent install. Not applicable for macOS. Ignored if `isSilent` is set to `false`.</param>
    public void QuitAndInstall(bool isSilent = false, bool isForceRunAfter = false)
    {
        autoUpdater.QuitAndInstall(isSilent, isForceRunAfter);
    }
}