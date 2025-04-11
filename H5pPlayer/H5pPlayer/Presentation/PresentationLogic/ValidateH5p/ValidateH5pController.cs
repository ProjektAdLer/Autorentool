using H5pPlayer.BusinessLogic.Entities;
using H5pPlayer.BusinessLogic.UseCases.ValidateH5p;

namespace H5pPlayer.Presentation.PresentationLogic.ValidateH5p;

public class ValidateH5pController : IValidateH5pController
{
    

    public ValidateH5pController(IValidateH5pUc validateH5pUc, IValidateH5pPresenter validateH5pPresenter)
    {
        ValidateH5pUc = validateH5pUc;
        ValidateH5PPresenter = validateH5pPresenter;
    }
    
    
    public void SetActiveH5pStateToNotUsable()
    {
        ValidateH5pUc.SetActiveH5pStateToNotUsable();
    }

    public void SetActiveH5pStateToPrimitive()
    {
        ValidateH5pUc.SetActiveH5pStateToPrimitive();
    }

    
    public IValidateH5pUc ValidateH5pUc { get; }
    public IValidateH5pPresenter ValidateH5PPresenter { get; }

}