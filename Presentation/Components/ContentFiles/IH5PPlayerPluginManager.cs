using H5pPlayer.BusinessLogic.Entities;
using H5pPlayer.BusinessLogic.UseCases.TerminateH5pPlayer;
using Microsoft.JSInterop;
using MudBlazor;

namespace Presentation.Components.ContentFiles;

public interface IH5PPlayerPluginManager
{

    Task StartH5pPlayerToValidateAsync(ParseH5PFileTO to);

    ILogger<H5PPlayerPluginManager> Logger { get; }
    IJSRuntime JSRuntime { get; }
    IDialogService DialogService { get; }
}