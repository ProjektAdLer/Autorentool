namespace H5pPlayer.Presentation.PresentationLogic;

public class InvalidPathErrorViewModel
{

    public InvalidPathErrorViewModel(Action action)
    {
        ErrorTextForInvalidPath = "init";
        InvalidPath = "init";
        InvalidPathErrorIsActive = false;
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
    
    
    private bool _invalidPathErrorIsActive;
    public bool InvalidPathErrorIsActive
    {
        get => _invalidPathErrorIsActive;
        set
        {
            if (_invalidPathErrorIsActive != value)
            {
                _invalidPathErrorIsActive = value;
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