using H5pPlayer.BusinessLogic.Api.JavaScript;
using H5pPlayer.BusinessLogic.UseCases.ValidateH5p;
using H5pPlayer.Presentation.PresentationLogic.ValidateH5p;

namespace H5pPlayer.Main;

public class ValidateH5pFactory : IValidateH5pFactory
{
    public ValidateH5pFactory()
    {
        ValidateH5pController = null;
        ValidateH5pVm = null;
        ValidateH5pPresenter = null;
    }
     
    public void CreateValidateH5pStructure(ICallJavaScriptAdapter callJavaScriptAdapter)
    {
        ValidateH5pVm = new ValidateH5pViewModel();
        ValidateH5pPresenter = new ValidateH5pPresenter(ValidateH5pVm);
        ValidateH5pUc = new ValidateH5pUc((ValidateH5pPresenter as IValidateH5pUcOutputPort)! , callJavaScriptAdapter);
        ValidateH5pController = new ValidateH5pController(ValidateH5pUc, ValidateH5pPresenter);
    }

    public IValidateH5pPresenter? ValidateH5pPresenter { get; private set; }
    public IValidateH5pViewModel? ValidateH5pVm { get; private set; }
    public IValidateH5pUc? ValidateH5pUc { get; private set; }
    public IValidateH5pController? ValidateH5pController { get; private set; }
}

