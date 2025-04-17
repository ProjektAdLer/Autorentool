namespace H5pPlayer.Presentation.PresentationLogic;

public class H5pPlayerViewModel
{

    public H5pPlayerViewModel(Action viewStateNotificationMethod)
    {
        InvalidPathErrorVm = new InvalidPathErrorViewModel(viewStateNotificationMethod);
        OnChange += viewStateNotificationMethod;
        IsDisplayModeActive = false;
        IsValidationModeActive = false;
    }
    
    public InvalidPathErrorViewModel InvalidPathErrorVm { get; }

    
    private bool _isDisplayModeActive;
    public bool IsDisplayModeActive
    {
        get => _isDisplayModeActive;
        set
        {
            if (_isDisplayModeActive != value)
            {
                _isDisplayModeActive = value;
                NotifyStateChanged();
            }
        }
    }
    private bool _isValidationModeActive;



    public bool IsValidationModeActive
    {
        get => _isValidationModeActive;
        set
        {
            if (_isValidationModeActive != value)
            {
                _isValidationModeActive = value;
                NotifyStateChanged();
            }
        }
    }
    
    
    private event Action OnChange;

    private void NotifyStateChanged()
    {
        OnChange.Invoke();
    }

 
}