﻿using System;
using System.Threading.Tasks;
using ElectronSharp.API;
using ElectronSharp.API.Entities;

namespace ElectronWrapper;

/// <summary>
/// Read and respond to changes in Chromium's native color theme.
/// </summary>
class NativeThemeWrapper : INativeThemeWrapper
{
    private readonly NativeTheme _nativeTheme = Electron.NativeTheme;

    /// <summary>
    /// Setting this property to <see cref="ThemeSourceMode.System"/> will remove the override and everything will be reset to the OS default. By default 'ThemeSource' is <see cref="ThemeSourceMode.System"/>.
    /// <para/>
    /// Settings this property to <see cref="ThemeSourceMode.Dark"/> will have the following effects:
    /// <list type="bullet">
    /// <item>
    /// <description><see cref="ShouldUseDarkColorsAsync"/> will be <see langword="true"/> when accessed</description>
    /// </item>
    /// <item>
    /// <description>Any UI Electron renders on Linux and Windows including context menus, devtools, etc. will use the dark UI.</description>
    /// </item>
    /// <item>
    /// <description>Any UI the OS renders on macOS including menus, window frames, etc. will use the dark UI.</description>
    /// </item>
    /// <item>
    /// <description>The 'prefers-color-scheme' CSS query will match 'dark' mode.</description>
    /// </item>
    /// <item>
    /// <description>The 'updated' event will be emitted</description>
    /// </item> 
    /// </list>
    /// <para/>
    /// Settings this property to <see cref="ThemeSourceMode.Light"/> will have the following effects:
    /// <list type="bullet">
    /// <item>
    /// <description><see cref="ShouldUseDarkColorsAsync"/> will be <see langword="false"/> when accessed</description>
    /// </item>
    /// <item>
    /// <description>Any UI Electron renders on Linux and Windows including context menus, devtools, etc. will use the light UI.</description>
    /// </item>
    /// <item>
    /// <description>Any UI the OS renders on macOS including menus, window frames, etc. will use the light UI.</description>
    /// </item>
    /// <item>
    /// <description>The 'prefers-color-scheme' CSS query will match 'light' mode.</description>
    /// </item>
    /// <item>
    /// <description>The 'updated' event will be emitted</description>
    /// </item>
    /// </list> 
    /// The usage of this property should align with a classic "dark mode" state machine in your application where the user has three options.
    /// <para/>
    /// <list type="bullet">
    /// <item>
    /// <description>Follow OS: SetThemeSource(ThemeSourceMode.System);</description>
    /// </item>
    /// <item>
    /// <description>Dark Mode: SetThemeSource(ThemeSourceMode.Dark);</description>
    /// </item>
    /// <item>
    /// <description>Light Mode: SetThemeSource(ThemeSourceMode.Light);</description>
    /// </item>
    /// </list>
    /// Your application should then always use <see cref="ShouldUseDarkColorsAsync"/> to determine what CSS to apply.
    /// </summary>
    /// <param name="themeSourceMode">The new ThemeSource.</param>
    public void SetThemeSource(ThemeSourceMode themeSourceMode)
    {
        _nativeTheme.SetThemeSource(themeSourceMode);
    }

    /// <summary>
    /// A <see cref="ThemeSourceMode"/> property that can be <see cref="ThemeSourceMode.System"/>, <see cref="ThemeSourceMode.Light"/> or <see cref="ThemeSourceMode.Dark"/>. It is used to override (<seealso cref="SetThemeSource"/>) and
    /// supercede the value that Chromium has chosen to use internally.
    /// </summary>
    public Task<ThemeSourceMode> GetThemeSourceAsync()
    {
        return _nativeTheme.GetThemeSourceAsync();
    }

    /// <summary>
    /// A <see cref="bool"/> for if the OS / Chromium currently has a dark mode enabled or is
    /// being instructed to show a dark-style UI. If you want to modify this value you
    /// should use <see cref="SetThemeSource"/>.
    /// </summary>
    public Task<bool> ShouldUseDarkColorsAsync()
    {
        return _nativeTheme.ShouldUseDarkColorsAsync();
    }

    /// <summary>
    /// Emitted when something in the underlying NativeTheme has changed. This normally means that either the value of <see cref="ShouldUseDarkColorsAsync"/>,
    /// <see>
    ///     <cref>ShouldUseHighContrastColorsAsync</cref>
    /// </see>
    /// or <see>
    ///     <cref>ShouldUseInvertedColorSchemeAsync</cref>
    /// </see>
    /// has changed. You will have to check them to determine which one has changed.
    /// </summary>
    public event Action Updated
    {
        add => _nativeTheme.Updated += value;
        remove => _nativeTheme.Updated -= value;
    }
}