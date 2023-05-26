using BusinessLogic.Commands;
using Presentation.PresentationLogic.LearningContent;

namespace Presentation.PresentationLogic.SelectedViewModels;

public class SelectedLearningContentViewModelStackEntry : ISelectedViewModelStackEntry
{
    public SelectedLearningContentViewModelStackEntry(ICommand command, ILearningContentViewModel? content, Action<ILearningContentViewModel?> action)
    {
        Command = command;
        Content = content;
        Action = action;
    }

    public ICommand Command { get; }
    private ILearningContentViewModel? Content { get; }
    private Action<ILearningContentViewModel?> Action { get; }

    public void Apply()
    {
        Action(Content);
    }
}