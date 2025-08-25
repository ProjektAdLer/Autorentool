using H5pPlayer.BusinessLogic.Api.JavaScript;
using H5pPlayer.BusinessLogic.Entities;
using H5pPlayer.BusinessLogic.UseCases.DisplayH5p;
using H5pPlayer.BusinessLogic.UseCases.TerminateH5pPlayer;
using Microsoft.Extensions.Logging;

namespace H5pPlayer.BusinessLogic.UseCases.ValidateH5p;

public class ValidateH5pUc : IValidateH5pUc
{

    internal ValidateH5pUc(
        IValidateH5pUcOutputPort validateH5PUcOutputPort,
        ICallJavaScriptAdapter callJavaScriptAdapter,
        ITerminateH5pPlayerUcPort terminateH5pPlayerUc,
        ILogger<ValidateH5pUc> logger)
    {
        ValidateH5pUcOutputPort = validateH5PUcOutputPort;
        ICallJavaScriptAdapter = callJavaScriptAdapter;
        TerminateH5pPlayerUc = terminateH5pPlayerUc;
        Logger = logger;
        EnsureBackCallOpportunityOfJsAdapterToCorrectInstanceOfValidateUc();
        H5pEntity = null;
        
    }

    private void EnsureBackCallOpportunityOfJsAdapterToCorrectInstanceOfValidateUc()
    {
        ReceiveFromJavaScriptAdapter.ValidateH5pUc = this;
    }

    public async Task  StartToValidateH5p(H5pEntity h5pEntity)
    {
        H5pEntity = h5pEntity;
        var javaScriptAdapterTO = new CallJavaScriptAdapterTO(h5pEntity.UnzippedH5psPath, h5pEntity.H5pZipSourcePath);
        await ICallJavaScriptAdapter.ValidateH5p(javaScriptAdapterTO);
        Logger.LogTrace("start ValidateH5pUCc");
    }
    
    /// <summary>
    /// I think this can remain synchronous -> does not have to be asynchronous
    ///
    /// in this first try we don't set the completed flag to the <see cref="H5pEntity"/>
    /// </summary>
    public void ValidateH5p(ValidateH5pTO validateH5pTo)
    {
        if (validateH5pTo.IsValidationCompleted)
        {
            ValidateH5pUcOutputPort.SetH5pIsCompletable();
            Logger.LogTrace("Validation is completed");
        }
    }

    public void TerminateValidateH5p()
    {
        TerminateH5pPlayerUc.TerminateH5pPlayer(H5pEntity!.ActiveH5pState);
        Logger.LogTrace("terminate ValidateH5pUc");
    }

    public void SetActiveH5pStateToNotUsable()
    {
        H5pEntity!.ActiveH5pState = H5pState.NotUsable;
        ValidateH5pUcOutputPort.SetH5pActiveStateToNotUsable();
        Logger.LogTrace("Set active h5p state to not usable");
    }

    public void SetActiveH5pStateToPrimitive()
    {
        H5pEntity!.ActiveH5pState = H5pState.Primitive;
        ValidateH5pUcOutputPort.SetH5pActiveStateToPrimitive();
        Logger.LogTrace("Set active h5p state to primitive");
    }
    public void SetActiveH5pStateToCompletable()
    {
        H5pEntity!.ActiveH5pState = H5pState.Completable;
        ValidateH5pUcOutputPort.SetH5pActiveStateToCompletable();
        Logger.LogTrace("Set active h5p state to completable");
    }

    private IValidateH5pUcOutputPort ValidateH5pUcOutputPort { get; }
    private ICallJavaScriptAdapter ICallJavaScriptAdapter { get; }
    private ITerminateH5pPlayerUcPort TerminateH5pPlayerUc { get; }
    public ILogger<ValidateH5pUc> Logger { get; }
    public H5pEntity? H5pEntity { get; set; }



}