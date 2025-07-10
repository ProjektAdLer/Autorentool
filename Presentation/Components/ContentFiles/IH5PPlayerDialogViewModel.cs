using H5pPlayer.BusinessLogic.Entities;
using H5pPlayer.BusinessLogic.UseCases.TerminateH5pPlayer;
using Microsoft.JSInterop;
using MudBlazor;

namespace Presentation.Components.ContentFiles;

public interface IH5PPlayerDialogViewModel
{
    Task<H5pPlayerResultTO?> OpenH5pPlayerDialogAsync(
        string h5pZipSourcePath,
        string unzippedH5psPath,
        H5pDisplayMode displayMode);

    ILogger<H5PPlayerDialogViewModel> Logger { get; }
    IJSRuntime JSRuntime { get; }
    IDialogService DialogService { get; }
}