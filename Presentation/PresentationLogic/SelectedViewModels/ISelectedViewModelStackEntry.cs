using BusinessLogic.Commands;

namespace Presentation.PresentationLogic.SelectedViewModels;

public interface ISelectedViewModelStackEntry
{
    public ICommand Command { get; }
    public void Apply();
}