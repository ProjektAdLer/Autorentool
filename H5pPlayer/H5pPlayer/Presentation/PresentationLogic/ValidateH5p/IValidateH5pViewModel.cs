namespace H5pPlayer.Presentation.PresentationLogic.ValidateH5p;

public interface IValidateH5pViewModel
{
    event Action OnChange;
    bool IsCompletable { get; set; }
}