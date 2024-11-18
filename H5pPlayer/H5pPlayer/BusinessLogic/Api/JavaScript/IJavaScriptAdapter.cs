using H5pPlayer.BusinessLogic.Entities;

namespace H5pPlayer.BusinessLogic.Api.JavaScript;

public interface IJavaScriptAdapter
{
    Task DisplayH5p(H5pEntity h5pEntity);
    Task TerminateH5pJavaScriptPlayer();
}