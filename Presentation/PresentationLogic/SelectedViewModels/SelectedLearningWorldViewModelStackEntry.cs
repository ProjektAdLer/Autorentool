using BusinessLogic.Commands;
using Presentation.PresentationLogic.LearningWorld;

namespace Presentation.PresentationLogic.SelectedViewModels;

public class SelectedLearningWorldViewModelStackEntry : ISelectedViewModelStackEntry
{
    public SelectedLearningWorldViewModelStackEntry(ICommand command, ILearningWorldViewModel? learningWorld, Action<ILearningWorldViewModel?> action)
    {
        Command = command;
        LearningWorld = learningWorld;
        Action = action;
    }

    public ICommand Command { get; }
    public void Apply()
    {
        Action(LearningWorld);
    }

    private ILearningWorldViewModel? LearningWorld { get; }
    private Action<ILearningWorldViewModel?> Action { get; }
}