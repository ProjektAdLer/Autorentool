namespace H5pPlayer.Presentation.PresentationLogic.ValidateH5p;

public class ValidateH5pViewModel : IValidateH5pViewModel
{

    public ValidateH5pViewModel()
    {
        OnChange = null;
        IsCompletable = false;
    }
    
    private bool _isCompletable;
    public bool IsCompletable
    {
        get => _isCompletable;
        set
        {
            if (_isCompletable != value)
            {
                _isCompletable = value;
                NotifyStateChanged();
            }
        }
    }
    
    public event Action? OnChange;
    
    
    private void NotifyStateChanged()
    {
        OnChange!.Invoke();
    }

}