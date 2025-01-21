namespace H5pPlayer.Presentation.PresentationLogic;

public class InvalidPathErrorViewModel
{

    public InvalidPathErrorViewModel(Action action)
    {
        ErrorTextForInvalidPath = "init";
        InvalidPath = "init";
        OnChange += action;
    }

 
    private string _errorTextForInvalidPath;
    public string ErrorTextForInvalidPath
    {
        get => _errorTextForInvalidPath;
        set
        {
            if (_errorTextForInvalidPath != value)
            {
                _errorTextForInvalidPath = value;
                NotifyStateChanged();
            }
        } 
    }
    
    private string _invalidPath;
    public string InvalidPath
    {
        get => _invalidPath;
        set
        {
            if (_invalidPath != value)
            {
                _invalidPath = value;
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