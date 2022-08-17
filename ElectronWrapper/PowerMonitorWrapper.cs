using System;
using ElectronNET.API;

namespace ElectronWrapper;

/// <summary>
/// Monitor power state changes..
/// </summary>
class PowerMonitorWrapper : IPowerMonitorWrapper
{
    private PowerMonitor powerMonitor;

    public PowerMonitorWrapper()
    {
        powerMonitor = Electron.PowerMonitor;
    }

    /// <summary>
    /// Emitted when the system is about to lock the screen. 
    /// </summary>
    public event Action OnLockScreen
    {
        add => powerMonitor.OnLockScreen += value;
        remove => powerMonitor.OnLockScreen -= value;
    }

    /// <summary>
    /// Emitted when the system is about to unlock the screen. 
    /// </summary>
    public event Action OnUnLockScreen
    {
        add => powerMonitor.OnUnLockScreen += value;
        remove => powerMonitor.OnUnLockScreen -= value;
    }


    /// <summary>
    /// Emitted when the system is suspending.
    /// </summary>
    public event Action OnSuspend
    {
        add => powerMonitor.OnSuspend += value;
        remove => powerMonitor.OnSuspend -= value;
    }

    /// <summary>
    /// Emitted when system is resuming.
    /// </summary>
    public event Action OnResume
    {
        add => powerMonitor.OnResume += value;
        remove => powerMonitor.OnResume -= value;
    }


    /// <summary>
    /// Emitted when the system changes to AC power.
    /// </summary>
    public event Action OnAc
    {
        add => powerMonitor.OnAC += value;
        remove => powerMonitor.OnAC -= value;
    }

    public event Action OnBattery
    {
        add => powerMonitor.OnBattery += value;
        remove => powerMonitor.OnBattery -= value;
    }


    /// <summary>
    /// Emitted when the system is about to reboot or shut down. If the event handler
    /// invokes `e.preventDefault()`, Electron will attempt to delay system shutdown in
    /// order for the app to exit cleanly.If `e.preventDefault()` is called, the app
    /// should exit as soon as possible by calling something like `app.quit()`.
    /// </summary>
    public event Action OnShutdown
    {
        add => powerMonitor.OnShutdown += value;
        remove => powerMonitor.OnShutdown -= value;
    }
}