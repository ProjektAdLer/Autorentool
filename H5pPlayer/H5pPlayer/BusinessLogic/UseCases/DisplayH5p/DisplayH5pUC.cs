using H5pPlayer.BusinessLogic.Api.JavaScript;
using H5pPlayer.BusinessLogic.Entities;

namespace H5pPlayer.BusinessLogic.UseCases.DisplayH5p;

public class DisplayH5pUC : IDisplayH5pUC
{
    internal DisplayH5pUC(ICallJavaScriptAdapter iCallJavaScriptAdapter)
    {
        ICallJavaScriptAdapter = iCallJavaScriptAdapter;
    }

    public async Task StartToDisplayH5p(H5pEntity h5pEntity)
    {
        var javaScriptAdapterTO = new CallJavaScriptAdapterTO(h5pEntity.UnzippedH5psPath, h5pEntity.H5pZipSourcePath);
        await ICallJavaScriptAdapter.DisplayH5p(javaScriptAdapterTO);
    }


    internal ICallJavaScriptAdapter ICallJavaScriptAdapter { get; }
}