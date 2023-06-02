using BusinessLogic.Commands;
using Presentation.PresentationLogic.LearningElement;

namespace Presentation.PresentationLogic.SelectedViewModels;

public class SelectedLearningElementViewModelStackEntry : ISelectedViewModelStackEntry
{
    public SelectedLearningElementViewModelStackEntry(ICommand command, ILearningElementViewModel? element, Action<ILearningElementViewModel?> action)
    {
        Command = command;
        Element = element;
        Action = action;
    }

    public ICommand Command { get; }
    private ILearningElementViewModel? Element { get; }
    private Action<ILearningElementViewModel?> Action { get; }

    public void Apply()
    {
        Action(Element);
    }
}