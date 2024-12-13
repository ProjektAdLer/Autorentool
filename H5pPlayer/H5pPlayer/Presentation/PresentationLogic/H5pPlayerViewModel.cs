namespace H5pPlayer.Presentation.PresentationLogic;

public class H5pPlayerViewModel
{
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