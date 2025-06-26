using ElectronSharp.API;
using ElectronSharp.API.Entities;
using System;
using System.Threading.Tasks;

namespace ElectronWrapper;

interface IWebContentsWrapper
{
    int Id { get; }
    Session Session { get; }

    event Action OnDidFinishLoad;

    Task<string> GetUrl();
    void InsertCss(bool isBrowserWindow, string path);
    Task LoadUrlAsync(string url);
    Task LoadUrlAsync(string url, LoadURLOptions options);
    void OpenDevTools();
    void OpenDevTools(OpenDevToolsOptions openDevToolsOptions);
    Task<bool> PrintAsync(PrintOptions? options = null);
    Task<bool> PrintToPdfAsync(string path, PrintToPDFOptions? options = null);
}