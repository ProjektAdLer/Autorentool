using BusinessLogic.Commands;

namespace Presentation.PresentationLogic.SelectedViewModels;

public class ActiveSlotInSpaceStackEntry : ISelectedViewModelStackEntry
{
    public ActiveSlotInSpaceStackEntry(ICommand command, int activeSlotInSpace, Action<int> action)
    {
        Command = command;
        ActiveSlotInSpace = activeSlotInSpace;
        Action = action;
    }
    public ICommand Command { get; }
    private int ActiveSlotInSpace { get; }
    private Action<int> Action { get; }
    public void Apply()
    {
        Action(ActiveSlotInSpace);
    }
}