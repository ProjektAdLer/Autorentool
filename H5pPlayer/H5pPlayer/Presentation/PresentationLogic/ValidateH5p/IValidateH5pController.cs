using H5pPlayer.BusinessLogic.Entities;
using H5pPlayer.BusinessLogic.UseCases.ValidateH5p;

namespace H5pPlayer.Presentation.PresentationLogic.ValidateH5p;

public interface IValidateH5pController
{
    IValidateH5pUc ValidateH5pUc { get; }
    IValidateH5pPresenter ValidateH5PPresenter { get; }
    void SetActiveH5pStateToNotUsable();
    void SetActiveH5pStateToPrimitive();
}