using H5pPlayer.BusinessLogic.Entities;
using Microsoft.JSInterop;
using MudBlazor;

namespace Presentation.Components.ContentFiles;

public interface IH5PPlayerDialogViewModel
{
    void OpenH5pPlayerDialog(
        string h5pZipSourcePath, 
        string unzippedH5psPath, 
        H5pDisplayMode displayMode);

    ILogger<H5PPlayerDialogViewModel> Logger { get; }
    IJSRuntime JSRuntime { get; }
    IDialogService DialogService { get; }
}