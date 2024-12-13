using H5pPlayer.BusinessLogic.Api.JavaScript;
using H5pPlayer.BusinessLogic.Entities;
using H5pPlayer.BusinessLogic.UseCases.DisplayH5p;

namespace H5pPlayer.BusinessLogic.UseCases.ValidateH5p;

public class ValidateH5pUc : IValidateH5pUc
{

    internal ValidateH5pUc(ICallJavaScriptAdapter iCallJavaScriptAdapter)
    {
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
    
    
    
    
    
    private ICallJavaScriptAdapter ICallJavaScriptAdapter { get; }

}