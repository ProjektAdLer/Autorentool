using H5pPlayer.BusinessLogic.Entities;
using H5pPlayer.BusinessLogic.UseCases.TerminateH5pPlayer;
using Microsoft.JSInterop;
using MudBlazor;

namespace Presentation.Components.ContentFiles;

public interface IH5PPlayerPluginManager
{
    /// <exception cref="ArgumentNullException">If any member of<see cref="StartH5PPlayerTO"/> Is Null!</exception>
    Task StartH5pPlayerToValidateAsync(StartH5PPlayerTO to);

    ILogger<H5PPlayerPluginManager> Logger { get; }
    IJSRuntime JSRuntime { get; }
    IDialogService DialogService { get; }
}