using BusinessLogic.Commands;

namespace Presentation.PresentationLogic.SelectedViewModels;

public class SelectedLearningObjectInPathWayViewModelStackEntry : ISelectedViewModelStackEntry
{
    public SelectedLearningObjectInPathWayViewModelStackEntry(ICommand command, ISelectableObjectInWorldViewModel? objectInPathWay, Action<ISelectableObjectInWorldViewModel?> action)
    {
        Command = command;
        ObjectInPathWay = objectInPathWay;
        Action = action;
    }

    public ICommand Command { get; }
    private ISelectableObjectInWorldViewModel? ObjectInPathWay { get; }
    private Action<ISelectableObjectInWorldViewModel?> Action { get; }
    public void Apply()
    {
        Action(ObjectInPathWay);
    }
}