using ElectronSharp.API;
using ElectronSharp.API.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ElectronWrapper;

public interface IAppWrapper
{
    CommandLine CommandLine { get; }
    bool IsReady { get; }
    event Func<QuitEventArgs, Task> BeforeQuit;
    event Action BrowserWindowBlur;
    event Action BrowserWindowCreated;
    event Action BrowserWindowFocus;
    event Action<string> OpenUrl;
    event Func<Task> Quitting;
    event Action Ready;
    event Action WebContentsCreated;
    event Func<QuitEventArgs, Task> WillQuit;
    event Action WindowAllClosed;
    void Exit(int exitCode = 0);
    void Focus();
    void Focus(FocusOptions focusOptions);
    Task<ProcessMetric[]> GetAppMetricsAsync(CancellationToken cancellationToken = default);
    Task<string> GetAppPathAsync(CancellationToken cancellationToken = default);
    Task<GPUFeatureStatus> GetGpuFeatureStatusAsync(CancellationToken cancellationToken = default);
    Task<string> GetLocaleAsync(CancellationToken cancellationToken = default);
    Task<string> GetPathAsync(PathName pathName, CancellationToken cancellationToken = default);
    Task<string> GetVersionAsync(CancellationToken cancellationToken = default);
    Task<bool> HasSingleInstanceLockAsync(CancellationToken cancellationToken = default);
    void On(string eventName, Action fn);
    void On(string eventName, Action<object> fn);
    void Once(string eventName, Action fn);
    void Once(string eventName, Action<object> fn);
    void Quit();
    void Relaunch();
    void Relaunch(RelaunchOptions relaunchOptions);
    void ReleaseSingleInstanceLock();
    Task<bool> RequestSingleInstanceLockAsync(Action<string[], string> newInstanceOpened, CancellationToken cancellationToken = default);
    void SetAboutPanelOptions(AboutPanelOptions options);
    void SetAppLogsPath(string path);
    void SetPath(PathName name, string path);
    void ShowAboutPanel();
}