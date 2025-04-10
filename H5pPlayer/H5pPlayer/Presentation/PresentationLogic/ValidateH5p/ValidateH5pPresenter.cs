using H5pPlayer.BusinessLogic.UseCases.ValidateH5p;

namespace H5pPlayer.Presentation.PresentationLogic.ValidateH5p;

public class ValidateH5pPresenter : IValidateH5pPresenter, IValidateH5pUcOutputPort
{
    public ValidateH5pPresenter(IValidateH5pViewModel validateH5pVm)
    {
        ValidateH5pVm = validateH5pVm;
    }
    public IValidateH5pViewModel ValidateH5pVm { get; private set; }

    public void SetH5pIsCompletable()
    {
       ValidateH5pVm.IsCompletable = true;
    }
}