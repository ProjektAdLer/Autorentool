using H5pPlayer.BusinessLogic.UseCases.ValidateH5p;

namespace H5pPlayer.Presentation.PresentationLogic.ValidateH5p;

public interface IValidateH5pController
{
    IValidateH5pUc ValidateH5PUc { get; }
    IValidateH5pPresenter ValidateH5PPresenter { get; }
}