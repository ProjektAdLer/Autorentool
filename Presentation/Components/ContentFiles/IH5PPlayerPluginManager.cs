using H5pPlayer.BusinessLogic.Entities;
using H5pPlayer.BusinessLogic.UseCases.TerminateH5pPlayer;
using Microsoft.JSInterop;
using MudBlazor;

namespace Presentation.Components.ContentFiles;

public interface IH5PPlayerPluginManager
{
    Task<H5pPlayerResultTO?> OpenH5pPlayerDialogAsync(
        string h5pZipSourcePath,
        string unzippedH5psPath,
        H5pDisplayMode displayMode);

    Task<H5pPlayerResultTO> ParseH5PFile(ParseH5PFileTO to);

    ILogger<H5PPlayerPluginManager> Logger { get; }
    IJSRuntime JSRuntime { get; }
    IDialogService DialogService { get; }
}