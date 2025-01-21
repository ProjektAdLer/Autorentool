namespace H5pPlayer.Presentation.PresentationLogic;

public class H5pPlayerViewModel
{

    public H5pPlayerViewModel(Action stateHasChanged)
    {
        IsCompletable = false;
        InvalidPathErrorVm = new InvalidPathErrorViewModel();
        OnChange += stateHasChanged;
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
        OnChange?.Invoke();
    }

}