﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using ElectronSharp.API;
using ElectronSharp.API.Entities;

namespace ElectronWrapper;

interface IMenuWrapper
{
    IReadOnlyCollection<MenuItem> MenuItems { get; }
    void SetApplicationMenu(MenuItem[] menuItems);
    IReadOnlyDictionary<int, ReadOnlyCollection<MenuItem>> ContextMenuItems { get;}
    void SetContextMenu(BrowserWindow browserWindow, MenuItem[] menuItems);
    void ContextMenuPopup(BrowserWindow browserWindow);
}

class MenuWrapper: IMenuWrapper
{
    private readonly Menu _menu = Electron.Menu;

    /// <summary>
    /// Gets the menu items.
    /// </summary>
    /// <value>
    /// The menu items.
    /// </value>
    public IReadOnlyCollection<MenuItem> MenuItems => _menu.MenuItems;


    /// <summary>
    /// Sets the application menu.
    /// </summary>
    /// <param name="menuItems">The menu items.</param>
    public void SetApplicationMenu(MenuItem[] menuItems)
    {
        _menu.SetApplicationMenu(menuItems);
    }

    /// <summary>
    /// Gets the context menu items.
    /// </summary>
    /// <value>
    /// The context menu items.
    /// </value>
    public IReadOnlyDictionary<int, ReadOnlyCollection<MenuItem>> ContextMenuItems => _menu.ContextMenuItems;

    /// <summary>
    /// Sets the context menu.
    /// </summary>
    /// <param name="browserWindow">The browser window.</param>
    /// <param name="menuItems">The menu items.</param>
    public void SetContextMenu(BrowserWindow browserWindow, MenuItem[] menuItems)
    {
        _menu.SetContextMenu(browserWindow.InnerBrowserWindow, menuItems);
    }

    /// <summary>
    /// Contexts the menu popup.
    /// </summary>
    /// <param name="browserWindow">The browser window.</param>
    public void ContextMenuPopup(BrowserWindow browserWindow)
    {
        _menu.ContextMenuPopup(browserWindow.InnerBrowserWindow);
    }
}