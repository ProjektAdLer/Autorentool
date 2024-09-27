using H5pPlayer.BusinessLogic.Domain;
using H5pPlayer.BusinessLogic.JavaScriptApi;

namespace H5pPlayer.BusinessLogic.UseCases.DisplayH5p;

public class DisplayH5pUC : IDisplayH5pUC
{

    public DisplayH5pUC(IJavaScriptAdapter javaScriptAdapter)
    {
        JavaScriptAdapter = javaScriptAdapter;
    }

    public void StartToDisplayH5pUC(H5pEntity h5pEntity)
    {
        JavaScriptAdapter.DisplayH5p(h5pEntity);
    }
    
    
    
    internal IJavaScriptAdapter JavaScriptAdapter { get; }

}