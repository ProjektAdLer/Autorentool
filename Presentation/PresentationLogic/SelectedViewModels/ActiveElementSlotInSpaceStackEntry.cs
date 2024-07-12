using BusinessLogic.Commands;

namespace Presentation.PresentationLogic.SelectedViewModels;

public class ActiveElementSlotInSpaceStackEntry : ISelectedViewModelStackEntry
{
    public ActiveElementSlotInSpaceStackEntry(ICommand command, int activeElementSlotInSpace, Action<int> action)
    {
        Command = command;
        ActiveElementSlotInSpace = activeElementSlotInSpace;
        Action = action;
    }

    private int ActiveElementSlotInSpace { get; }
    private Action<int> Action { get; }
    public ICommand Command { get; }

    public void Apply()
    {
        Action(ActiveElementSlotInSpace);
    }
}