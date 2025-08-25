using H5pPlayer.BusinessLogic.Api.JavaScript;
using H5pPlayer.BusinessLogic.Entities;
using H5pPlayer.BusinessLogic.UseCases.TerminateH5pPlayer;
using Microsoft.Extensions.Logging;

namespace H5pPlayer.BusinessLogic.UseCases.DisplayH5p;

public class DisplayH5pUc : IDisplayH5pUc
{
    internal DisplayH5pUc(
        ICallJavaScriptAdapter iCallJavaScriptAdapter,
        ITerminateH5pPlayerUcPort terminateH5pPlayerUc,
        ILogger<DisplayH5pUc> logger)
    {
        ICallJavaScriptAdapter = iCallJavaScriptAdapter;
        TerminateH5pPlayerUc = terminateH5pPlayerUc;
        H5pEntity = null;
    }

    public async Task StartToDisplayH5p(H5pEntity h5pEntity)
    {
        var javaScriptAdapterTO = new CallJavaScriptAdapterTO(h5pEntity.UnzippedH5psPath, h5pEntity.H5pZipSourcePath);
        await ICallJavaScriptAdapter.DisplayH5p(javaScriptAdapterTO);
    }
    
    public void TerminateDisplayH5p()
    {
        TerminateH5pPlayerUc.TerminateH5pPlayer(H5pEntity!.ActiveH5pState);
    }
    private ICallJavaScriptAdapter ICallJavaScriptAdapter { get; }
    
    private ITerminateH5pPlayerUcPort TerminateH5pPlayerUc { get; }
    public H5pEntity? H5pEntity { get; set; }

}