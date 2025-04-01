namespace H5pPlayer.Presentation.PresentationLogic.ValidateH5p;

public class ValidateH5pPresenter : IValidateH5pPresenter
{
    public ValidateH5pPresenter(IValidateH5pViewModel validateH5pVm)
    {
        ValidateH5pVm = validateH5pVm;
    }
    public IValidateH5pViewModel ValidateH5pVm { get; set; } 
}