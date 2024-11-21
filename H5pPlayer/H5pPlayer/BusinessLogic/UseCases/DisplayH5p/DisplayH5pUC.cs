using H5pPlayer.BusinessLogic.Api.JavaScript;
using H5pPlayer.BusinessLogic.Entities;

namespace H5pPlayer.BusinessLogic.UseCases.DisplayH5p;

public class DisplayH5pUC : IDisplayH5pUC
{
    public DisplayH5pUC(IJavaScriptAdapter javaScriptAdapter)
    {
        JavaScriptAdapter = javaScriptAdapter;
    }

    public async Task StartToDisplayH5pUC(H5pEntity h5pEntity)
    {
        var javaScriptAdapterTO = new JavaScriptAdapterTO(h5pEntity.UnzippedH5psPath, h5pEntity.H5pZipSourcePath);
        await JavaScriptAdapter.DisplayH5p(javaScriptAdapterTO);
    }


    internal IJavaScriptAdapter JavaScriptAdapter { get; }
}