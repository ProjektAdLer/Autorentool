namespace H5pPlayer.Presentation.PresentationLogic;

public class H5pPlayerViewModel
{

    public H5pPlayerViewModel()
    {
        IsCompletable = false;
        InvalidPathErrorVm = new InvalidPathErrorViewModel();
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
    
    public event Action OnChange;

    private void NotifyStateChanged()
    {
        OnChange?.Invoke();
    }

}