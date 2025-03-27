namespace H5pPlayer.Presentation.PresentationLogic;

public class H5pPlayerViewModel
{

    public H5pPlayerViewModel(Action viewStateNotificationMethod)
    {
        IsCompletable = false;
        InvalidPathErrorVm = new InvalidPathErrorViewModel(viewStateNotificationMethod);
        OnChange += viewStateNotificationMethod;
    }
    public InvalidPathErrorViewModel InvalidPathErrorVm { get; }
    
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
    
    private event Action OnChange;

    private void NotifyStateChanged()
    {
        OnChange.Invoke();
    }

}