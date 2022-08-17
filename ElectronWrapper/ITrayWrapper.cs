using ElectronNET.API;
using ElectronNET.API.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElectronWrapper;

interface ITrayWrapper
{
    IReadOnlyCollection<MenuItem> MenuItems { get; }

    event Action OnBalloonClick;
    event Action OnBalloonClosed;
    event Action OnBalloonShow;
    event Action<TrayClickEventArgs, Rectangle> OnClick;
    event Action<TrayClickEventArgs, Rectangle> OnDoubleClick;
    event Action<TrayClickEventArgs, Rectangle> OnRightClick;

    void Destroy();
    void DisplayBalloon(DisplayBalloonOptions options);
    Task<bool> IsDestroyedAsync();
    void On(string eventName, Action fn);
    void On(string eventName, Action<object> fn);
    void Once(string eventName, Action fn);
    void Once(string eventName, Action<object> fn);
    void SetImage(string image);
    void SetPressedImage(string image);
    void SetTitle(string title);
    void SetToolTip(string toolTip);
    void Show(string image);
    void Show(string image, MenuItem menuItem);
    void Show(string image, MenuItem[] menuItems);
}