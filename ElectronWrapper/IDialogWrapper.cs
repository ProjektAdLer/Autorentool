﻿using System.Threading.Tasks;
using ElectronSharp.API.Entities;

namespace ElectronWrapper;

public interface IDialogWrapper
{
    Task<string[]> ShowOpenDialogAsync(BrowserWindow browserWindow, OpenDialogOptions options);
    Task<string> ShowSaveDialogAsync(BrowserWindow browserWindow, SaveDialogOptions options);
    Task<MessageBoxResult> ShowMessageBoxAsync(string message);
    Task<MessageBoxResult> ShowMessageBoxAsync(MessageBoxOptions messageBoxOptions);
    Task<MessageBoxResult> ShowMessageBoxAsync(BrowserWindow browserWindow, string message);
    Task<MessageBoxResult> ShowMessageBoxAsync(BrowserWindow browserWindow, MessageBoxOptions messageBoxOptions);
}