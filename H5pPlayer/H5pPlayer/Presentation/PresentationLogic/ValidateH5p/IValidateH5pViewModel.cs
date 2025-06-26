using H5pPlayer.BusinessLogic.Entities;

namespace H5pPlayer.Presentation.PresentationLogic.ValidateH5p;

public interface IValidateH5pViewModel
{
    event Action OnChange;
    bool IsCompletable { get; set; }
    H5pState ActiveH5PState { get; set; }
}