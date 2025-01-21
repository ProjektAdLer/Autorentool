namespace H5pPlayer.Presentation.PresentationLogic;

public class InvalidPathErrorViewModel
{
    
    
    
    
    public event Action OnChange;

    private void NotifyStateChanged()
    {
        OnChange?.Invoke();
    }

    public string ErrorTextForInvalidPath { get; set; }
    public string InvalidPath { get; set; }
}