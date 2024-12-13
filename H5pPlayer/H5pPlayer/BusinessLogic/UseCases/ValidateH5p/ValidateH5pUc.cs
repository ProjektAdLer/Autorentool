using H5pPlayer.BusinessLogic.Api.JavaScript;
using H5pPlayer.BusinessLogic.Entities;
using H5pPlayer.BusinessLogic.UseCases.DisplayH5p;

namespace H5pPlayer.BusinessLogic.UseCases.ValidateH5p;

public class ValidateH5pUc : IValidateH5pUc
{

    internal ValidateH5pUc(IValidateH5pUcOutputPort validateH5PUcOutputPort,
        ICallJavaScriptAdapter iCallJavaScriptAdapter)
    {
        ValidateH5PUcOutputPort = validateH5PUcOutputPort;
        ICallJavaScriptAdapter = iCallJavaScriptAdapter;
        EnsureBackCallOpportunityOfJsAdapterToCorrectInstanceOfValidateUc();
    }

    private void EnsureBackCallOpportunityOfJsAdapterToCorrectInstanceOfValidateUc()
    {
        ReceiveFromJavaScriptAdapter.VaidateH5pUc = this;
    }

    public async Task  StartToValidateH5p(H5pEntity h5pEntity)
    {
        var javaScriptAdapterTO = new CallJavaScriptAdapterTO(h5pEntity.UnzippedH5psPath, h5pEntity.H5pZipSourcePath);
        await ICallJavaScriptAdapter.ValidateH5p(javaScriptAdapterTO);
    }
    
    /// <summary>
    /// I think this can remain synchronous -> does not have to be asynchronous
    ///
    /// in this first try we don't set the completed flag to the <see cref="H5pEntity"/>
    /// </summary>
    public void ValidateH5p(ValidateH5pTO validateH5PTo)
    {
        if(validateH5PTo.IsValidationCompleted)
            ValidateH5PUcOutputPort.SetH5pIsCompletable();
    }


    public IValidateH5pUcOutputPort ValidateH5PUcOutputPort { get; }
    private ICallJavaScriptAdapter ICallJavaScriptAdapter { get; }

    
}