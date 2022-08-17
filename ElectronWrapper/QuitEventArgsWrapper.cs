using ElectronNET.API;

namespace ElectronWrapper;

/// <summary>
/// Event arguments for the <see cref="App.BeforeQuit"/> / <see cref="App.WillQuit"/> event.
/// </summary>
class QuitEventArgsWrapper : IQuitEventArgsWrapper
{
    private QuitEventArgs quitEventArgs;

    public QuitEventArgsWrapper(QuitEventArgs args)
    {
        quitEventArgs = args;
    }
    /// <summary>
    /// Will prevent the default behaviour, which is terminating the application.
    /// </summary>
    public void PreventDefault()
    {
        quitEventArgs.PreventDefault();
    }
}