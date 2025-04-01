using H5pPlayer.BusinessLogic.UseCases.ValidateH5p;
using H5pPlayer.Presentation.PresentationLogic.ValidateH5p;

namespace H5pPlayer.Main;

public interface IValidateH5pFactory
{
    void CreateValidateH5pPresentationStructure(IValidateH5pUc validateH5PUc);
    IValidateH5pViewModel ValidateH5pVm { get; set; }
    IValidateH5pController ValidateH5pController { get; set; }
}