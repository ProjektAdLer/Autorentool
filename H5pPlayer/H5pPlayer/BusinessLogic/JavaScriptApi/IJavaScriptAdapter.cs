using H5pPlayer.BusinessLogic.Domain;

namespace H5pPlayer.BusinessLogic.JavaScriptApi;

public interface IJavaScriptAdapter
{
    Task DisplayH5p(H5pEntity h5pEntity);
}