using ElectronSharp.API.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElectronWrapper;

interface IAutoUpdaterWrapper
{
    Dictionary<string, string> RequestHeaders { set; }
    event Action OnCheckingForUpdate;
    event Action<ProgressInfo> OnDownloadProgress;
    event Action<string> OnError;
    event Action<UpdateInfo> OnUpdateAvailable;
    event Action<UpdateInfo> OnUpdateDownloaded;
    event Action<UpdateInfo> OnUpdateNotAvailable;

    Task<UpdateCheckResult> CheckForUpdatesAndNotifyAsync();
    Task<UpdateCheckResult> CheckForUpdatesAsync();
    void QuitAndInstall(bool isSilent = false, bool isForceRunAfter = false);
}