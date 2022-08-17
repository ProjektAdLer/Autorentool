using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;

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
    /// Whether to automatically download an update when it is found. (Default is true)
    /// </summary>
    public bool AutoDownload
    {
        get => autoUpdater.AutoDownload;
        set => autoUpdater.AutoDownload = value;
    }

    /// <summary>
    /// Whether to automatically install a downloaded update on app quit (if `QuitAndInstall` was not called before).
    /// 
    /// Applicable only on Windows and Linux.
    /// </summary>
    public bool AutoInstallOnAppQuit
    {
        get => autoUpdater.AutoInstallOnAppQuit;

        set => autoUpdater.AutoInstallOnAppQuit = value;
    }

    /// <summary>
    /// *GitHub provider only.* Whether to allow update to pre-release versions. 
    /// Defaults to "true" if application version contains prerelease components (e.g. "0.12.1-alpha.1", here "alpha" is a prerelease component), otherwise "false".
    /// 
    /// If "true", downgrade will be allowed("allowDowngrade" will be set to "true").
    /// </summary>
    public bool AllowPrerelease
    {
        get => autoUpdater.AllowPrerelease;
        set => autoUpdater.AllowPrerelease = value;
    }

    /// <summary>
    /// *GitHub provider only.* 
    /// Get all release notes (from current version to latest), not just the latest (Default is false).
    /// </summary>
    public bool FullChangelog
    {
        get => autoUpdater.FullChangelog;
        set => autoUpdater.FullChangelog = value;
    }

    /// <summary>
    /// Whether to allow version downgrade (when a user from the beta channel wants to go back to the stable channel).
    /// Taken in account only if channel differs (pre-release version component in terms of semantic versioning).
    /// Default is false.
    /// </summary>
    public bool AllowDowngrade
    {
        get => autoUpdater.AllowDowngrade;
        set => autoUpdater.AllowDowngrade = value;
    }

    /// <summary>
    /// For test only.
    /// </summary>
    public string UpdateConfigPath => autoUpdater.UpdateConfigPath;

    /// <summary>
    /// The current application version
    /// </summary>
    public Task<SemVer> CurrentVersionAsync => autoUpdater.CurrentVersionAsync;

    /// <summary>
    /// Get the update channel. Not applicable for GitHub. 
    /// Doesn’t return channel from the update configuration, only if was previously set.
    /// </summary>
    [Obsolete("Use the asynchronous version ChannelAsync instead")]
    public string Channel => autoUpdater.Channel;

    /// <summary>
    /// Get the update channel. Not applicable for GitHub. 
    /// Doesn’t return channel from the update configuration, only if was previously set.
    /// </summary>
    public Task<string> ChannelAsync => autoUpdater.ChannelAsync;

    /// <summary>
    /// The request headers.
    /// </summary>
    public Task<Dictionary<string, string>> RequestHeadersAsync => autoUpdater.RequestHeadersAsync;

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

    /// <summary>
    /// Start downloading update manually. You can use this method if "AutoDownload" option is set to "false".
    /// </summary>
    /// <returns>Path to downloaded file.</returns>
    public Task<string> DownloadUpdateAsync()
    {
        return autoUpdater.DownloadUpdateAsync();
    }

    /// <summary>
    /// Feed URL.
    /// </summary>
    /// <returns>Feed URL.</returns>
    public Task<string> GetFeedUrlAsync()
    {
        return autoUpdater.GetFeedURLAsync();
    }
}