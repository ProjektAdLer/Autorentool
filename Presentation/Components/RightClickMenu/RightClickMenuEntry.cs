namespace Presentation.Components.RightClickMenu;

public class RightClickMenuEntry
{
    public RightClickMenuEntry(string displayedText, Action action)
    {
        DisplayedText = displayedText;
        Action = action;
    }
    public string DisplayedText { get; }
    public Action Action { get; }
}