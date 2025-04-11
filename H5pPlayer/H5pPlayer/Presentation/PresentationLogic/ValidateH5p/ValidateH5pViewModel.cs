using H5pPlayer.BusinessLogic.Entities;

namespace H5pPlayer.Presentation.PresentationLogic.ValidateH5p;

public class ValidateH5pViewModel : IValidateH5pViewModel
{

    public ValidateH5pViewModel()
    {
        OnChange = null;
        IsCompletable = false;
        ActiveH5PState = H5pState.Unknown;
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
    
    
    private H5pState _activeH5PState;
    public H5pState ActiveH5PState
    {
        get => _activeH5PState;
        set
        {
            if (_activeH5PState != value)
            {
                _activeH5PState = value;
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