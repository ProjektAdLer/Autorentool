using ElectronNET.API;
using ElectronNET.API.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ElectronWrapper;

public interface IAppWrapper
{
    CommandLine CommandLine { get; }
    bool IsReady { get; }
    string Name { get; set; }
    Task<string> NameAsync { get; }
    string UserAgentFallback { get; set; }
    Task<string> UserAgentFallbackAsync { get; }

    event Action<bool> AccessibilitySupportChanged;
    event Func<QuitEventArgs, Task> BeforeQuit;
    event Action BrowserWindowBlur;
    event Action BrowserWindowCreated;
    event Action BrowserWindowFocus;
    event Action<string> OpenFile;
    event Action<string> OpenUrl;
    event Func<Task> Quitting;
    event Action Ready;
    event Action WebContentsCreated;
    event Func<QuitEventArgs, Task> WillQuit;
    event Action WindowAllClosed;

    void AddRecentDocument(string path);
    void ClearRecentDocuments();
    void Exit(int exitCode = 0);
    void Focus();
    void Focus(FocusOptions focusOptions);
    Task<ProcessMetric[]> GetAppMetricsAsync(CancellationToken cancellationToken = default);
    Task<string> GetAppPathAsync(CancellationToken cancellationToken = default);
    Task<int> GetBadgeCountAsync(CancellationToken cancellationToken = default);
    Task<string> GetCurrentActivityTypeAsync(CancellationToken cancellationToken = default);
    Task<GPUFeatureStatus> GetGpuFeatureStatusAsync(CancellationToken cancellationToken = default);
    Task<JumpListSettings> GetJumpListSettingsAsync(CancellationToken cancellationToken = default);
    Task<string> GetLocaleAsync(CancellationToken cancellationToken = default);
    Task<LoginItemSettings> GetLoginItemSettingsAsync(CancellationToken cancellationToken = default);
    Task<LoginItemSettings> GetLoginItemSettingsAsync(LoginItemSettingsOptions options, CancellationToken cancellationToken = default);
    Task<string> GetPathAsync(PathName pathName, CancellationToken cancellationToken = default);
    Task<string> GetVersionAsync(CancellationToken cancellationToken = default);
    Task<bool> HasSingleInstanceLockAsync(CancellationToken cancellationToken = default);
    void Hide();
    Task<int> ImportCertificateAsync(ImportCertificateOptions options, CancellationToken cancellationToken = default);
    void InvalidateCurrentActivity();
    Task<bool> IsAccessibilitySupportEnabledAsync(CancellationToken cancellationToken = default);
    Task<bool> IsDefaultProtocolClientAsync(string protocol, CancellationToken cancellationToken = default);
    Task<bool> IsDefaultProtocolClientAsync(string protocol, string path, CancellationToken cancellationToken = default);
    Task<bool> IsDefaultProtocolClientAsync(string protocol, string path, string[] args, CancellationToken cancellationToken = default);
    Task<bool> IsUnityRunningAsync(CancellationToken cancellationToken = default);
    void On(string eventName, Action fn);
    void On(string eventName, Action<object> fn);
    void Once(string eventName, Action fn);
    void Once(string eventName, Action<object> fn);
    void Quit();
    void Relaunch();
    void Relaunch(RelaunchOptions relaunchOptions);
    void ReleaseSingleInstanceLock();
    Task<bool> RemoveAsDefaultProtocolClientAsync(string protocol, CancellationToken cancellationToken = default);
    Task<bool> RemoveAsDefaultProtocolClientAsync(string protocol, string path, CancellationToken cancellationToken = default);
    Task<bool> RemoveAsDefaultProtocolClientAsync(string protocol, string path, string[] args, CancellationToken cancellationToken = default);
    Task<bool> RequestSingleInstanceLockAsync(Action<string[], string> newInstanceOpened, CancellationToken cancellationToken = default);
    void ResignCurrentActivity();
    void SetAboutPanelOptions(AboutPanelOptions options);
    void SetAccessibilitySupportEnabled(bool enabled);
    void SetAppLogsPath(string path);
    void SetAppUserModelId(string id);
    Task<bool> SetAsDefaultProtocolClientAsync(string protocol, CancellationToken cancellationToken = default);
    Task<bool> SetAsDefaultProtocolClientAsync(string protocol, string path, CancellationToken cancellationToken = default);
    Task<bool> SetAsDefaultProtocolClientAsync(string protocol, string path, string[] args, CancellationToken cancellationToken = default);
    Task<bool> SetBadgeCountAsync(int count, CancellationToken cancellationToken = default);
    void SetJumpList(JumpListCategory[] categories);
    void SetLoginItemSettings(LoginSettings loginSettings);
    void SetPath(PathName name, string path);
    void SetUserActivity(string type, object userInfo);
    void SetUserActivity(string type, object userInfo, string webpageUrl);
    Task<bool> SetUserTasksAsync(UserTask[] userTasks, CancellationToken cancellationToken = default);
    void Show();
    void ShowAboutPanel();
}