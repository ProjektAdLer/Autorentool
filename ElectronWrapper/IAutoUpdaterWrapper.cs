using ElectronNET.API.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElectronWrapper;

interface IAutoUpdaterWrapper
{
    bool AllowDowngrade { get; set; }
    bool AllowPrerelease { get; set; }
    bool AutoDownload { get; set; }
    bool AutoInstallOnAppQuit { get; set; }
    string Channel { get; }
    Task<string> ChannelAsync { get; }
    Task<SemVer> CurrentVersionAsync { get; }
    bool FullChangelog { get; set; }
    Dictionary<string, string> RequestHeaders { set; }
    Task<Dictionary<string, string>> RequestHeadersAsync { get; }
    string UpdateConfigPath { get; }

    event Action OnCheckingForUpdate;
    event Action<ProgressInfo> OnDownloadProgress;
    event Action<string> OnError;
    event Action<UpdateInfo> OnUpdateAvailable;
    event Action<UpdateInfo> OnUpdateDownloaded;
    event Action<UpdateInfo> OnUpdateNotAvailable;

    Task<UpdateCheckResult> CheckForUpdatesAndNotifyAsync();
    Task<UpdateCheckResult> CheckForUpdatesAsync();
    Task<string> DownloadUpdateAsync();
    Task<string> GetFeedUrlAsync();
    void QuitAndInstall(bool isSilent = false, bool isForceRunAfter = false);
}