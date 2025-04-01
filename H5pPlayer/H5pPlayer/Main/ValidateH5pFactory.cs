using H5pPlayer.BusinessLogic.UseCases.ValidateH5p;
using H5pPlayer.Presentation.PresentationLogic.ValidateH5p;

namespace H5pPlayer.Main;

public class ValidateH5pFactory : IValidateH5pFactory
{
    public void CreateValidateH5pPresentationStructure(IValidateH5pUc validateH5PUc)
    {
        ValidateH5pVm = new ValidateH5pViewModel();
        var validateH5PPresenter = new ValidateH5pPresenter(ValidateH5pVm);
        ValidateH5pController = new ValidateH5pController(validateH5PUc, validateH5PPresenter);
    }

    public IValidateH5pViewModel ValidateH5pVm { get; set; }
    public IValidateH5pController ValidateH5pController { get; set; }
}

