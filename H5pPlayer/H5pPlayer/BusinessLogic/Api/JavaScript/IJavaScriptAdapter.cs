using H5pPlayer.BusinessLogic.Entities;
using H5pPlayer.BusinessLogic.UseCases.DisplayH5p;

namespace H5pPlayer.BusinessLogic.Api.JavaScript;

public interface IJavaScriptAdapter
{
    Task TerminateH5pJavaScriptPlayer();
    Task DisplayH5p(JavaScriptAdapterTO h5PEntity);
}