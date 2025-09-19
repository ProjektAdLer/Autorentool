using H5pPlayer.BusinessLogic.UseCases.ValidateH5p;

namespace H5pPlayer.Presentation.PresentationLogic.ValidateH5p;

public class ValidateH5pController : IValidateH5pController
{
    public ValidateH5pController(IValidateH5pUc validateH5pUc, IValidateH5pPresenter validateH5pPresenter)
    {
        ValidateH5pUc = validateH5pUc;
        ValidateH5pPresenter = validateH5pPresenter;
    }
    
    public void SetActiveH5pStateToNotUsable()
    {
        ValidateH5pUc.SetActiveH5pStateToNotUsable();
    }

    public void SetActiveH5pStateToPrimitive()
    {
        ValidateH5pUc.SetActiveH5pStateToPrimitive();
    }

    public void SetActiveH5pStateToCompletable()
    {
        ValidateH5pUc.SetActiveH5pStateToCompletable();
    }

    public void TerminateValidateH5p()
    {
        ValidateH5pUc.TerminateValidateH5p();
    }

    public IValidateH5pUc ValidateH5pUc { get; }
    public IValidateH5pPresenter ValidateH5pPresenter { get; }

}