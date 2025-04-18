﻿using System;
using System.Threading.Tasks;
using ElectronSharp.API;

namespace ElectronWrapper;

/// <summary>
/// Detect keyboard events when the application does not have keyboard focus.
/// </summary>
class GlobalShortcutWrapper : IGlobalShortcutWrapper
{
    private readonly GlobalShortcut _globalShortcut = Electron.GlobalShortcut;

    /// <summary>
    /// Registers a global shortcut of accelerator. 
    /// The callback is called when the registered shortcut is pressed by the user.
    /// 
    /// When the accelerator is already taken by other applications, this call will 
    /// silently fail.This behavior is intended by operating systems, since they don’t
    /// want applications to fight for global shortcuts.
    /// </summary>
    public void Register(string accelerator, Action function)
    {
        _globalShortcut.Register(accelerator, function);
    }

    /// <summary>
    /// When the accelerator is already taken by other applications, 
    /// this call will still return false. This behavior is intended by operating systems,
    /// since they don’t want applications to fight for global shortcuts.
    /// </summary>
    /// <returns>Whether this application has registered accelerator.</returns>
    public Task<bool> IsRegisteredAsync(string accelerator)
    {
        return _globalShortcut.IsRegisteredAsync(accelerator);
    }

    /// <summary>
    /// Unregisters the global shortcut of accelerator.
    /// </summary>
    public void Unregister(string accelerator)
    {
        _globalShortcut.Unregister(accelerator);
    }

    /// <summary>
    /// Unregisters all of the global shortcuts.
    /// </summary>
    public void UnregisterAll()
    {
        _globalShortcut.UnregisterAll();
    }
}