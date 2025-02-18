using H5pPlayer.BusinessLogic.Entities;
using H5pPlayer.BusinessLogic.UseCases.DisplayH5p;

namespace H5pPlayer.BusinessLogic.Api.JavaScript;

public interface ICallJavaScriptAdapter
{
    Task TerminateH5pJavaScriptPlayer();
    Task DisplayH5p(CallJavaScriptAdapterTO callJavaScriptAdapterTo);
    Task ValidateH5p(CallJavaScriptAdapterTO callJavaScriptAdapterTo);
}