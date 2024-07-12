using BusinessLogic.Commands;

namespace Presentation.PresentationLogic.SelectedViewModels;

public class ActiveStorySlotInSpaceStackEntry : ISelectedViewModelStackEntry
{
    public ActiveStorySlotInSpaceStackEntry(ICommand command, int activeStoryStorySlotInSpace, Action<int> action)
    {
        Command = command;
        ActiveStorySlotInSpace = activeStoryStorySlotInSpace;
        Action = action;
    }

    private int ActiveStorySlotInSpace { get; }
    private Action<int> Action { get; }
    public ICommand Command { get; }

    public void Apply()
    {
        Action(ActiveStorySlotInSpace);
    }
}