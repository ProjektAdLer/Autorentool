﻿using H5pPlayer.BusinessLogic;
using H5pPlayer.BusinessLogic.Api.JavaScript;
using H5pPlayer.BusinessLogic.Entities;
using H5pPlayer.BusinessLogic.UseCases.DisplayH5p;
using H5pPlayer.BusinessLogic.UseCases.StartH5pPlayer;
using H5pPlayer.BusinessLogic.UseCases.ValidateH5p;
using H5pPlayer.DataAccess.FileSystem;
using Microsoft.JSInterop;

namespace H5pPlayer.Presentation.PresentationLogic;

public class H5pPlayerController
{
 

    public H5pPlayerController(IJSRuntime jsRuntime)
    {
        IJavaScriptAdapter javaScriptAdapter = new JavaScriptAdapter(jsRuntime);
        IDisplayH5pUC displayH5pUC = new DisplayH5pUC(javaScriptAdapter);
        IValidateH5pUc validateH5pUc = new ValidateH5pUc(javaScriptAdapter);
        StartH5PPlayerPresenter = new H5PPlayerPlayerPresenter();
        var fileSystemDataAccess = new FileSystemDataAccess();
        StartH5PPlayerUc = new StartH5pPlayerUC(
            validateH5pUc, fileSystemDataAccess, displayH5pUC, StartH5PPlayerPresenter);
    }

    public async Task StartH5pPlayer(string h5pSourcePath, string unzippedH5psPath)
    {
        var displayH5pTo = new StartH5pPlayerInputTO(H5pDisplayMode.Validate, h5pSourcePath, unzippedH5psPath);
        await StartH5PPlayerUc.StartH5pPlayer(displayH5pTo);
    }
    
    internal IStartH5pPlayerUCInputPort StartH5PPlayerUc { get; }
    internal IStartH5pPlayerUCOutputPort StartH5PPlayerPresenter { get; }

}