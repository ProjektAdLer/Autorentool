using H5pPlayer.BusinessLogic.Api.JavaScript;
using H5pPlayer.BusinessLogic.UseCases.TerminateH5pPlayer;
using H5pPlayer.BusinessLogic.UseCases.ValidateH5p;
using H5pPlayer.Presentation.PresentationLogic.ValidateH5p;
using Microsoft.Extensions.Logging;

namespace H5pPlayer.Main;

public class ValidateH5pFactory : IValidateH5pFactory
{
    public ValidateH5pFactory()
    {
        ValidateH5pVm = null;
        ValidateH5pPresenter = null;
        TerminateH5pPlayerUc = null;
        ValidateH5pUc = null;
        ValidateH5pController = null;
    }
     
    public void CreateValidateH5pStructure(
        ICallJavaScriptAdapter callJavaScriptAdapter,
        ITerminateH5pPlayerUcPort terminateH5pPlayerUc,
        ILoggerFactory loggerFactory)
    {
        ValidateH5pVm = new ValidateH5pViewModel();
        ValidateH5pPresenter = new ValidateH5pPresenter(ValidateH5pVm);
        TerminateH5pPlayerUc = terminateH5pPlayerUc;
        var logger = loggerFactory.CreateLogger<ValidateH5pUc>();
        ValidateH5pUc = new ValidateH5pUc(
            (ValidateH5pPresenter as IValidateH5pUcOutputPort)!,
            callJavaScriptAdapter,
            TerminateH5pPlayerUc,
            logger);
        ValidateH5pController = new ValidateH5pController(ValidateH5pUc, ValidateH5pPresenter);
    }



    public IValidateH5pViewModel? ValidateH5pVm { get; private set; }
    public IValidateH5pPresenter? ValidateH5pPresenter { get; private set; }
    public ITerminateH5pPlayerUcPort? TerminateH5pPlayerUc { get; private set; }
    public IValidateH5pUc? ValidateH5pUc { get; private set; }
    public IValidateH5pController? ValidateH5pController { get; private set; }
}

