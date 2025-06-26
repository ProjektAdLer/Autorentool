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
    private readonly AutoUpdater _autoUpdater = Electron.AutoUpdater;

    /// <summary>
    /// The request headers.
    /// </summary>
    public Dictionary<string, string> RequestHeaders
    {
        set => _autoUpdater.RequestHeaders = value;
    }

    /// <summary>
    /// Emitted when there is an error while updating.
    /// </summary>
    public event Action<string> OnError
    {
        add => _autoUpdater.OnError += value;
        remove => _autoUpdater.OnError -= value;
    }

    /// <summary>
    /// Emitted when checking if an update has started.
    /// </summary>
    public event Action OnCheckingForUpdate
    {
        add => _autoUpdater.OnCheckingForUpdate += value;
        remove => _autoUpdater.OnCheckingForUpdate -= value;
    }

    /// <summary>
    /// Emitted when there is an available update. 
    /// The update is downloaded automatically if AutoDownload is true.
    /// </summary>
    public event Action<UpdateInfo> OnUpdateAvailable
    {
        add => _autoUpdater.OnUpdateAvailable += value;
        remove => _autoUpdater.OnUpdateAvailable -= value;
    }

    /// <summary>
    /// Emitted when there is no available update.
    /// </summary>
    public event Action<UpdateInfo> OnUpdateNotAvailable
    {
        add => _autoUpdater.OnUpdateNotAvailable += value;
        remove => _autoUpdater.OnUpdateNotAvailable -= value;
    }

    /// <summary>
    /// Emitted on download progress.
    /// </summary>
    public event Action<ProgressInfo> OnDownloadProgress
    {
        add => _autoUpdater.OnDownloadProgress += value;
        remove => _autoUpdater.OnDownloadProgress -= value;
    }

    /// <summary>
    /// Emitted on download complete.
    /// </summary>
    public event Action<UpdateInfo> OnUpdateDownloaded
    {
        add => _autoUpdater.OnUpdateDownloaded += value;
        remove => _autoUpdater.OnUpdateDownloaded -= value;
    }

    /// <summary>
    /// Asks the server whether there is an update.
    /// </summary>
    /// <returns></returns>
    public Task<UpdateCheckResult> CheckForUpdatesAsync()
    {
        return _autoUpdater.CheckForUpdatesAsync();
    }

    /// <summary>
    /// Asks the server whether there is an update.
    /// 
    /// This will immediately download an update, then install when the app quits.
    /// </summary>
    /// <returns></returns>
    public Task<UpdateCheckResult> CheckForUpdatesAndNotifyAsync()
    {
        return _autoUpdater.CheckForUpdatesAndNotifyAsync();
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
        _autoUpdater.QuitAndInstall(isSilent, isForceRunAfter);
    }
}