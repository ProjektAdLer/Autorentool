using System;
using System.Threading.Tasks;
using ElectronSharp.API;
using ElectronSharp.API.Entities;
using System.Threading;

namespace ElectronWrapper;

/// <summary>
/// Control your application's event lifecycle.
/// </summary>
class AppWrapper : IAppWrapper
{
    private App app;
    public AppWrapper()
    {
        app = Electron.App;
    }

    /// <summary>
    /// Emitted when all windows have been closed.
    /// <para/>
    /// If you do not subscribe to this event and all windows are closed, the default behavior is to quit
    /// the app; however, if you subscribe, you control whether the app quits or not.If the user pressed
    /// Cmd + Q, or the developer called <see cref="Quit"/>, Electron will first try to close all the windows
    /// and then emit the <see cref="WillQuit"/> event, and in this case the <see cref="WindowAllClosed"/> event
    /// would not be emitted.
    /// </summary>
    public event Action WindowAllClosed
    {
        add => app.WindowAllClosed += value;
        remove => app.WindowAllClosed -= value;
    }

    /// <summary>
    /// Emitted before the application starts closing its windows. 
    /// <para/>
    /// Note: If application quit was initiated by <see cref="AutoUpdater.QuitAndInstall"/> then <see cref="BeforeQuit"/>
    /// is emitted after emitting close event on all windows and closing them.
    /// <para/>
    /// Note: On Windows, this event will not be emitted if the app is closed due to a shutdown/restart of the system or a user logout.
    /// </summary>
    public event Func<QuitEventArgs, Task> BeforeQuit
    {
        add => app.BeforeQuit += value;
        remove => app.BeforeQuit -= value;
    }

    /// <summary>
    /// Emitted when all windows have been closed and the application will quit.
    /// <para/>
    /// See the description of the <see cref="WindowAllClosed"/> event for the differences between the <see cref="WillQuit"/>
    /// and <see cref="WindowAllClosed"/> events.
    /// <para/>
    /// Note: On Windows, this event will not be emitted if the app is closed due to a shutdown/restart of the system or a user logout.
    /// </summary>
    public event Func<QuitEventArgs, Task> WillQuit
    {
        add => app.WillQuit += value;
        remove => app.WillQuit -= value;
    }

    /// <summary>
    /// Emitted when the application is quitting.
    /// <para/>
    /// Note: On Windows, this event will not be emitted if the app is closed due to a shutdown/restart of the system or a user logout.
    /// </summary>
    public event Func<Task> Quitting
    {
        add => app.Quitting += value;
        remove => app.Quitting -= value;
    }
        
    /// <summary>
    /// Emitted when a <see cref="BrowserWindow"/> blurred.
    /// </summary>
    public event Action BrowserWindowBlur
    {
        add => app.BrowserWindowBlur += value;
        remove => app.BrowserWindowBlur -= value;
    }

    /// <summary>
    /// Emitted when a <see cref="BrowserWindow"/> gets focused.
    /// </summary>
    public event Action BrowserWindowFocus
    {
        add => app.BrowserWindowFocus += value;
        remove => app.BrowserWindowFocus -= value;
    }

    /// <summary>
    /// Emitted when a new <see cref="BrowserWindow"/> is created.
    /// </summary>
    public event Action BrowserWindowCreated
    {
        add => app.BrowserWindowCreated += value;

        remove => app.BrowserWindowCreated -= value;
    }

    /// <summary>
    /// Emitted when a new <see cref="WebContents"/> is created.
    /// </summary>
    public event Action WebContentsCreated
    {
        add => app.WebContentsCreated += value;
        remove => app.WebContentsCreated -= value;
    }

    /// <summary>
    /// Emitted when the application has finished basic startup.
    /// </summary>
    public event Action Ready
    {
        add => app.Ready += value;
        remove => app.Ready -= value;
    }

    /// <summary>
    /// Application host fully started.
    /// </summary>
    public bool IsReady => app.IsReady;

    /// <summary>
    /// Emitted when a MacOS user wants to open a URL with the application. Your application's Info.plist file must
    /// define the URL scheme within the CFBundleURLTypes key, and set NSPrincipalClass to AtomApplication.
    /// </summary>
    public event Action<string> OpenUrl
    {
        add => app.OpenUrl += value;
        remove => app.OpenUrl -= value;
    }

    /// <summary>
    /// Try to close all windows. The <see cref="BeforeQuit"/> event will be emitted first. If all windows are successfully
    /// closed, the <see cref="WillQuit"/> event will be emitted and by default the application will terminate. This method
    /// guarantees that all beforeunload and unload event handlers are correctly executed. It is possible
    /// that a window cancels the quitting by returning <see langword="false"/> in the beforeunload event handler.
    /// </summary>
    public void Quit()
    {
        app.Quit();
    }

    /// <summary>
    /// All windows will be closed immediately without asking user and the <see cref="BeforeQuit"/> and <see cref="WillQuit"/>
    /// events will not be emitted.
    /// </summary>
    /// <param name="exitCode">Exits immediately with exitCode. exitCode defaults to 0.</param>
    public void Exit(int exitCode = 0)
    {
        app.Exit(exitCode);
    }

    /// <summary>
    /// Relaunches the app when current instance exits. By default the new instance will use the same working directory
    /// and command line arguments with current instance.
    /// <para/>
    /// Note that this method does not quit the app when executed, you have to call <see cref="Quit"/> or <see cref="Exit"/>
    /// after calling <see cref="Relaunch()"/> to make the app restart.
    /// <para/>
    /// When <see cref="Relaunch()"/> is called for multiple times, multiple instances will be started after current instance
    /// exited.
    /// </summary>
    public void Relaunch()
    {
        app.Relaunch();
    }

    /// <summary>
    /// Relaunches the app when current instance exits. By default the new instance will use the same working directory
    /// and command line arguments with current instance. When <see cref="RelaunchOptions.Args"/> is specified, the
    /// <see cref="RelaunchOptions.Args"/> will be passed as command line arguments instead. When <see cref="RelaunchOptions.ExecPath"/>
    /// is specified, the <see cref="RelaunchOptions.ExecPath"/> will be executed for relaunch instead of current app.
    /// <para/>
    /// Note that this method does not quit the app when executed, you have to call <see cref="Quit"/> or <see cref="Exit"/>
    /// after calling <see cref="Relaunch()"/> to make the app restart.
    /// <para/>
    /// When <see cref="Relaunch()"/> is called for multiple times, multiple instances will be started after current instance
    /// exited.
    /// </summary>
    /// <param name="relaunchOptions">Options for the relaunch.</param>
    public void Relaunch(RelaunchOptions relaunchOptions)
    {
        app.Relaunch(relaunchOptions);
    }

    /// <summary>
    /// On Linux, focuses on the first visible window. On macOS, makes the application the active app. On Windows, focuses
    /// on the application's first window.
    /// </summary>
    public void Focus()
    {
        app.Focus();
    }

    /// <summary>
    /// On Linux, focuses on the first visible window. On macOS, makes the application the active app. On Windows, focuses
    /// on the application's first window.
    /// <para/>
    /// You should seek to use the <see cref="FocusOptions.Steal"/> option as sparingly as possible.
    /// </summary>
    public void Focus(FocusOptions focusOptions)
    {
        app.Focus();
    }

    /// <summary>
    /// The current application directory.
    /// </summary>
    public async Task<string> GetAppPathAsync(CancellationToken cancellationToken = default)
    {
        return await app.GetAppPathAsync(cancellationToken);
    }

    /// <summary>
    /// Sets or creates a directory your app's logs which can then be manipulated with <see cref="GetPathAsync"/>
    /// or <see cref="SetPath"/>.
    /// <para/>
    /// Calling <see cref="SetAppLogsPath"/> without a path parameter will result in this directory being set to
    /// ~/Library/Logs/YourAppName on macOS, and inside the userData directory on Linux and Windows.
    /// </summary>
    /// <param name="path">A custom path for your logs. Must be absolute.</param>
    public void SetAppLogsPath(string path)
    {
        app.SetAppLogsPath(path);
    }

    /// <summary>
    /// The path to a special directory. If <see cref="GetPathAsync"/> is called without called
    /// <see cref="SetAppLogsPath"/> being called first, a default directory will be created equivalent
    /// to calling <see cref="SetAppLogsPath"/> without a path parameter.
    /// </summary>
    /// <param name="pathName">Special directory.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A path to a special directory or file associated with name.</returns>
    public async Task<string> GetPathAsync(PathName pathName, CancellationToken cancellationToken = default)
    {
        return await app.GetPathAsync(pathName, cancellationToken);
    }

    /// <summary>
    /// Overrides the path to a special directory or file associated with name. If the path specifies a directory
    /// that does not exist, an Error is thrown. In that case, the directory should be created with fs.mkdirSync or similar.
    /// <para/>
    /// You can only override paths of a name defined in <see cref="GetPathAsync"/>.
    /// <para/>
    /// By default, web pages' cookies and caches will be stored under the <see cref="PathName.UserData"/> directory. If you
    /// want to change this location, you have to override the <see cref="PathName.UserData"/> path before the <see cref="Ready"/>
    /// event of the <see cref="App"/> module is emitted.
    /// <param name="name">Special directory.</param>
    /// <param name="path">New path to a special directory.</param>
    /// </summary>
    public void SetPath(PathName name, string path)
    {
        app.SetPath(name, path);
    }

    /// <summary>
    /// The version of the loaded application. If no version is found in the application’s package.json file, 
    /// the version of the current bundle or executable is returned.
    /// </summary>
    /// <returns>The version of the loaded application.</returns>
    public async Task<string> GetVersionAsync(CancellationToken cancellationToken = default)
    {
        return await app.GetVersionAsync(cancellationToken);
    }

    /// <summary>
    /// The current application locale. Possible return values are documented <see href="https://www.electronjs.org/docs/api/locales">here</see>.
    /// <para/>
    /// Note: When distributing your packaged app, you have to also ship the locales folder.
    /// <para/>
    /// Note: On Windows, you have to call it after the <see cref="Ready"/> events gets emitted.
    /// </summary>
    /// <returns>The current application locale.</returns>
    public async Task<string> GetLocaleAsync(CancellationToken cancellationToken = default)
    {
        return await app.GetLocaleAsync(cancellationToken);
    }

    /// <summary>
    /// The return value of this method indicates whether or not this instance of your application successfully obtained
    /// the lock. If it failed to obtain the lock, you can assume that another instance of your application is already
    /// running with the lock and exit immediately.
    /// <para/>
    /// I.e.This method returns <see langword="true"/> if your process is the primary instance of your application and your
    /// app should continue loading. It returns <see langword="false"/> if your process should immediately quit as it has
    /// sent its parameters to another instance that has already acquired the lock.
    /// <para/>
    /// On macOS, the system enforces single instance automatically when users try to open a second instance of your app
    /// in Finder, and the open-file and open-url events will be emitted for that.However when users start your app in
    /// command line, the system's single instance mechanism will be bypassed, and you have to use this method to ensure
    /// single instance.
    /// </summary>
    /// <param name="newInstanceOpened">Lambda with an array of the second instance’s command line arguments.
    /// The second parameter is the working directory path.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>This method returns false if your process is the primary instance of the application and your app
    /// should continue loading. And returns true if your process has sent its parameters to another instance, and
    /// you should immediately quit.
    /// </returns>
    public async Task<bool> RequestSingleInstanceLockAsync(Action<string[], string> newInstanceOpened, CancellationToken cancellationToken = default)
    {
        return await app.RequestSingleInstanceLockAsync(newInstanceOpened, cancellationToken);
    }

    /// <summary>
    /// Releases all locks that were created by makeSingleInstance. This will allow
    /// multiple instances of the application to once again run side by side.
    /// </summary>
    public void ReleaseSingleInstanceLock()
    {
        app.ReleaseSingleInstanceLock();
    }

    /// <summary>
    /// This method returns whether or not this instance of your app is currently holding the single instance lock.
    /// You can request the lock with <see cref="RequestSingleInstanceLockAsync"/> and release with
    /// <see cref="ReleaseSingleInstanceLock"/>.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async Task<bool> HasSingleInstanceLockAsync(CancellationToken cancellationToken = default)
    {
        return await app.HasSingleInstanceLockAsync(cancellationToken);
    }

    /// <summary>
    /// Memory and cpu usage statistics of all the processes associated with the app.
    /// </summary>
    /// <returns>
    /// Array of ProcessMetric objects that correspond to memory and cpu usage
    /// statistics of all the processes associated with the app.
    /// <param name="cancellationToken">The cancellation token.</param>
    /// </returns>
    public async Task<ProcessMetric[]> GetAppMetricsAsync(CancellationToken cancellationToken = default)
    {
        return await app.GetAppMetricsAsync(cancellationToken);
    }

    /// <summary>
    /// The Graphics Feature Status from chrome://gpu/.
    /// <para/>
    /// Note: This information is only usable after the gpu-info-update event is emitted.
    /// <param name="cancellationToken">The cancellation token.</param>
    /// </summary>
    public async Task<GPUFeatureStatus> GetGpuFeatureStatusAsync(CancellationToken cancellationToken = default)
    {
        return await app.GetGpuFeatureStatusAsync(cancellationToken);
    }

    /// <summary>
    /// A <see cref="CommandLine"/> object that allows you to read and manipulate the command line arguments that Chromium uses.
    /// </summary>
    public CommandLine CommandLine => app.CommandLine;

    /// <summary>
    /// Show the app's about panel options. These options can be overridden with
    /// <see cref="SetAboutPanelOptions"/>.
    /// </summary>
    public void ShowAboutPanel()
    {
        app.ShowAboutPanel();
    }

    /// <summary>
    /// Set the about panel options. This will override the values defined in the app's .plist file on macOS. See the
    /// <see href="https://developer.apple.com/reference/appkit/nsapplication/1428479-orderfrontstandardaboutpanelwith?language=objc">Apple docs</see>
    /// for more details. On Linux, values must be set in order to be shown; there are no defaults.
    /// <para/>
    /// If you do not set credits but still wish to surface them in your app, AppKit will look for a file named "Credits.html",
    /// "Credits.rtf", and "Credits.rtfd", in that order, in the bundle returned by the NSBundle class method main. The first file
    /// found is used, and if none is found, the info area is left blank. See Apple
    /// <see href="https://developer.apple.com/documentation/appkit/nsaboutpaneloptioncredits?language=objc">documentation</see> for more information.
    /// </summary>
    /// <param name="options">About panel options.</param>
    public void SetAboutPanelOptions(AboutPanelOptions options)
    {
        app.SetAboutPanelOptions(options);
    }

    /// <summary>
    /// Subscribe to an unmapped event on the <see cref="App"/> module.
    /// </summary>
    /// <param name="eventName">The event name</param>
    /// <param name="fn">The handler</param>
    public void On(string eventName, Action fn)
        => app.On(eventName, fn);

    /// <summary>
    /// Subscribe to an unmapped event on the <see cref="App"/> module.
    /// </summary>
    /// <param name="eventName">The event name</param>
    /// <param name="fn">The handler</param>
    public void On(string eventName, Action<object> fn)
        => app.On(eventName, fn);

    /// <summary>
    /// Subscribe to an unmapped event on the <see cref="App"/> module once.
    /// </summary>
    /// <param name="eventName">The event name</param>
    /// <param name="fn">The handler</param>
    public void Once(string eventName, Action fn)
        => app.Once(eventName, fn);

    /// <summary>
    /// Subscribe to an unmapped event on the <see cref="App"/> module once.
    /// </summary>
    /// <param name="eventName">The event name</param>
    /// <param name="fn">The handler</param>
    public void Once(string eventName, Action<object> fn)
        => app.Once(eventName, fn);
}