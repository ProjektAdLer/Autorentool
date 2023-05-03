using BusinessLogic.Commands;
using Presentation.PresentationLogic.LearningWorld;

namespace Presentation.PresentationLogic.SelectedViewModels;

public class SelectedLearningWorldViewModelStackEntry : ISelectedViewModelStackEntry
{
    public SelectedLearningWorldViewModelStackEntry(ICommand command, LearningWorldViewModel? learningWorld, Action<LearningWorldViewModel?> action)
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

    private LearningWorldViewModel? LearningWorld { get; }
    private Action<LearningWorldViewModel?> Action { get; }
}