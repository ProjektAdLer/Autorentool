using ElectronSharp.API;

namespace ElectronWrapper;

/// <summary>
/// Event arguments for the <see cref="App.BeforeQuit"/> / <see cref="App.WillQuit"/> event.
/// </summary>
class QuitEventArgsWrapper : IQuitEventArgsWrapper
{
    private readonly QuitEventArgs _quitEventArgs;

    public QuitEventArgsWrapper(QuitEventArgs args)
    {
        _quitEventArgs = args;
    }
    /// <summary>
    /// Will prevent the default behaviour, which is terminating the application.
    /// </summary>
    public void PreventDefault()
    {
        _quitEventArgs.PreventDefault();
    }
}