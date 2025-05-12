using H5pPlayer.BusinessLogic.Api.JavaScript;
using H5pPlayer.BusinessLogic.Entities;
using H5pPlayer.BusinessLogic.UseCases.DisplayH5p;

namespace H5pPlayer.BusinessLogic.UseCases.ValidateH5p;

public class ValidateH5pUc : IValidateH5pUc
{

    internal ValidateH5pUc(IValidateH5pUcOutputPort validateH5PUcOutputPort,
        ICallJavaScriptAdapter iCallJavaScriptAdapter)
    {
        ValidateH5pUcOutputPort = validateH5PUcOutputPort;
        ICallJavaScriptAdapter = iCallJavaScriptAdapter;
        EnsureBackCallOpportunityOfJsAdapterToCorrectInstanceOfValidateUc();
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
    }
    
    /// <summary>
    /// I think this can remain synchronous -> does not have to be asynchronous
    ///
    /// in this first try we don't set the completed flag to the <see cref="H5pEntity"/>
    /// </summary>
    public void ValidateH5p(ValidateH5pTO validateH5pTo)
    {
        if(validateH5pTo.IsValidationCompleted)
            ValidateH5pUcOutputPort.SetH5pIsCompletable();
    }

    public void SetActiveH5pStateToNotUsable()
    {
        H5pEntity.ActiveH5pState = H5pState.NotUsable;
        ValidateH5pUcOutputPort.SetH5pActiveStateToNotUsable();
    }

    public void SetActiveH5pStateToPrimitive()
    {
        H5pEntity.ActiveH5pState = H5pState.Primitive;
        ValidateH5pUcOutputPort.SetH5pActiveStateToPrimitive();
    }

    public IValidateH5pUcOutputPort ValidateH5pUcOutputPort { get; }
    private ICallJavaScriptAdapter ICallJavaScriptAdapter { get; }
    public H5pEntity H5pEntity { get; set; }



 
    
    



}