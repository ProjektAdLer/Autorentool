using H5pPlayer.BusinessLogic.Entities;
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

    public void SetH5pActiveStateToNotUsable()
    {
        ValidateH5pVm.ActiveH5PState = H5pState.NotUsable;
    }

    public void SetH5pActiveStateToPrimitive()
    {
        ValidateH5pVm.ActiveH5PState = H5pState.Primitive;
    }
    public void SetH5pActiveStateToCompletable()
    {
        ValidateH5pVm.ActiveH5PState = H5pState.Completable;
    }
}